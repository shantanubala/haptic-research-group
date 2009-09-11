/****************************************************************************************************
DATE		USER	CHANGE LOG
====================================================================================================
22 Jul 2008 Vineeth	Created change log comments

23 Jul 2008 Vineeth Added code to interface with parallel port for haptic belt in drawFaces function
					Added input32.dll as reference, added inpout32.lib to debug folder, and linked
					library in project properties

24 Sep 2008 Sreekar Modified code written by Vineeth on 23 Jul. Changes are noted in the code.

****************************************************************************************************/
#include "StdAfx.h"
#include "FaceDetector.h"
#include <windows.h>
#include <iostream>
#include <sstream>

using namespace System;
using namespace System::Runtime::InteropServices;

// 23 Jul 2008, Vineeth
// Added prototype declaration to use inpout32.dll
//void _stdcall Out32(short portaddr, short datum);


namespace OpenCV
{
	//workaround for 'global or static variable may not have managed type 'HapticOutput::HapticBelt ^''
	public ref class Globals abstract sealed {
		public:
		static HapticOutput::HapticBelt^ belt;
	};

	FaceDetector::FaceDetector(CvSize imageSize, char *cascade_path, double scaleFactor, int minNeighbors, int flags, CvSize winSize)
	{
		constructorInit(imageSize, cascade_path, scaleFactor, minNeighbors, flags, winSize);
	}
	FaceDetector::FaceDetector(int width, int height, char *cascade_path, double scaleFactor, int minNeighbors, 
						int flags, CvSize winSize)
	{
		constructorInit(cvSize(width, height), cascade_path, scaleFactor, minNeighbors, flags, winSize);
	}
	void FaceDetector::constructorInit(CvSize imageSize, char *cascade_path, double scaleFactor, int minNeighbors, int flags, CvSize winSize)
	{
		this->imgBuffer = NULL;		// Make sure setImageSize() doesn't try to release this
		setImageSize(imageSize);	// This also initializes the imgBuffer

		initCascade(cascade_path);
		storage = cvCreateMemStorage(0);

	Globals::belt  = gcnew HapticOutput::HapticBelt();
	//look into this,  does this mean unmanaged, do I need to free it?

	System::String^ inboundPort = "COM4";
	System::String^ outboundPort = "COM4";
	System::String^ baud_string = "9600";
	System::String^ parity_string = "None";
	System::String^ stopbits_string = "1";
	System::String^ databits_string = "8";
	System::String^ readTimeout_string = "1000";
	Globals::belt->SetupPorts(inboundPort,outboundPort,baud_string,databits_string,stopbits_string,parity_string,readTimeout_string);
	Globals::belt->OpenPorts();
	Globals::belt->Vibrate_Motor("1","A","A",1);
	Globals::belt->Vibrate_Motor("2","A","A",1);
	Globals::belt->Vibrate_Motor("3","A","A",1);
	Globals::belt->Vibrate_Motor("4","A","A",1);
	Globals::belt->Vibrate_Motor("5","A","A",1);

		// Set the face detection parameters
		this->scaleFactor = scaleFactor;
		this->minNeighbors = minNeighbors;
		this->flags = flags;
		this->winSize = winSize;

		// Set face image variables
		this->faceWidth = 128;
		this->faceHeight = 128;
		this->maxFaces = 2;
		this->faceBuffers = (IplImage**)calloc(maxFaces, sizeof(IplImage*));	// Allocate array of pointers
		this->faces = NULL;
		
		// Allocate the individual face buffers
		for(int i = 0; i < maxFaces; i++)
			faceBuffers[i] = cvCreateImage(cvSize(faceWidth, faceHeight), IPL_DEPTH_8U, 3);
		
		this->detectionTime = 0;

		bufferLock = new CCritSec();
	}
	FaceDetector::~FaceDetector()
	{
		for(int i = 0; i < maxFaces; i++)
			cvReleaseImage(&faceBuffers[i]);
		free(faceBuffers);
	
		cvReleaseMemStorage(&storage);

		cvReleaseImage(&imgBuffer);
	}	

	bool FaceDetector::initCascade(char *cascade_path)
	{
		if(cascade_path == NULL)
			cascade = (CvHaarClassifierCascade*)cvLoad("..\\OpenCV\\data\\haarcascades\\haarcascade_frontalface_alt.xml");
		else	
			cascade = (CvHaarClassifierCascade*)cvLoad(cascade_path);

		return true;
	}

	int FaceDetector::findFaces(char *pBuffer)
	{
		// Make sure nobody is using our memory first.
		bufferLock->Lock();
		
		imgBuffer->imageData = pBuffer;

		int scale = 2;
		CvSize img_size = cvGetSize(imgBuffer);
		IplImage* temp = cvCreateImage(cvSize(img_size.width / scale,img_size.height / scale), IPL_DEPTH_8U, 3);
		cvPyrDown(imgBuffer, temp);
		
		cvClearMemStorage(storage);

		detectionTime = (double)cvGetTickCount();
		faces = cvHaarDetectObjects( temp, cascade, storage, scaleFactor, minNeighbors, flags, winSize);
		detectionTime = (double)cvGetTickCount() - detectionTime;
		detectionTime = detectionTime / ((double)cvGetTickFrequency() * 1000.0);

        cvReleaseImage(&temp);
		
		// See if detect objects failed
		if(!faces)
			return -1;

		// Fix the scaling on the rectangles and update face buffers
		CvRect *faceRect;
		for (int faceIndex = 0; faceIndex < faces->total && faceIndex < maxFaces; faceIndex++) {
			// fix scaling
			faceRect = (CvRect*)cvGetSeqElem(faces, faceIndex);
			faceRect->x *= scale;
			faceRect->y *= scale;
			faceRect->width *= scale;
			faceRect->height *= scale;
			
			// set roi to the face
			cvSetImageROI(imgBuffer, *faceRect);
			
			//IplImage * tempImage = cvCreateImage(cvSize(faceRect->width, faceRect->height), IPL_DEPTH_8U, 3);
			// copy the face to a buffer
			//cvCopy(imgBuffer, tempImage);
			cvResize(imgBuffer, faceBuffers[faceIndex]);
			
			//cvReleaseImage(&tempImage);
			//cvFlip(faceBuffers[faceIndex]);
		}

		// reset the roi
		cvResetImageROI(imgBuffer);
		
		bufferLock->Unlock();

		return faces->total;
	}
	
	void FaceDetector::drawFaces(char *pBuffer)
	{
		imgBuffer->imageData = pBuffer;

		CvRect faceRect;
		int scale = 2;

		// 23 Jul 2008, Vineeth, Haptic Belt Interface
		// Divide the width of the video frame into 7 (number of vibrators on the haptic belt) equal segments
		int numVibrators = 5; 
		double segmentWidth = imgBuffer->width/numVibrators;

		for (int faceIndex = 0; faceIndex < faces->total; faceIndex++) 
		{
			faceRect = *(CvRect*)cvGetSeqElem(faces, faceIndex);
			cvRectangle(imgBuffer, cvPoint(faceRect.x, faceRect.y), 
							cvPoint(faceRect.x + faceRect.width, faceRect.y + faceRect.height), cvScalar(0.0, 255.0, 0.0), 3);

			// 23 Jul 2008, Vineeth, Haptic Belt Interface
			// Identify the vibrators that need to be activated

			// 24 Sep 2008, Sreekar, Modified after internal user testing
			// Use the center of the face rectangle and not the left corner for activating the segments
			// Removed this: double segActivate = ceil(faceRect.x/segmentWidth);
			double segActivate = ceil((faceRect.x + faceRect.width/2)/segmentWidth);

			// 24 Sep 2008, Sreekar, Modified after internal user testing
			// Add all the parallel port addresses to which the device automatically assumes
			//Out32(0x378, (short)pow(2,segActivate)); // Automatic port address one
			//Out32(0x378, (short)pow(2,segActivate));
			//Out32(0x, (short)pow(2,segActivate));

			std::string motorNo;
			std::stringstream ss; //string stream to convert integer to standard string
			
			//ss << (short)pow(2,segActivate); why?  turns into //2 4 8 16 32
			ss << segActivate;
			ss >> motorNo; //put new string in my variable

			//guess what though!  we actually need an unmanaged String^
			System::String^ motorM =gcnew String(motorNo.c_str()); //do I need to free this?

			Globals::belt->Vibrate_Motor(motorM,"A","A",1);
			/******************************************************************************/			
			//cvCircle(imgBuffer, cvPoint(faceRect.x + faceRect.width/2, faceRect.y + faceRect.height/2), faceRect.width, cvScalar(0.0,255.0,0.0));
		}		

		//cvFlip(imgBuffer);
	}

	void FaceDetector::flipBuffer(char *pBuffer)
	{
		imgBuffer->imageData = pBuffer;

		cvFlip(imgBuffer);
	}

	// THIS IS AN UNSAFE POINTER UNLESS THE CCRITSEC HAS BEEN LOCKED
	char* FaceDetector::getFace(int faceNum, int *xOffset, int *yOffset, int *width, int *height)
	{
		// Make sure their is currently a valid face at the index requested
		if(faceNum + 1 > maxFaces || faceNum + 1 > faces->total)
		{
			return NULL;
		}

		// Set the output parameters for the image attributes
		CvRect* r = (CvRect*)cvGetSeqElem( faces, faceNum );
		*xOffset = r->x;
		*yOffset = r->y;
		*width = r->width;
		*height = r->height;

		return faceBuffers[faceNum]->imageData;
	}

	// THIS IS AN UNSAFE POINTER UNLESS THE CCRITSEC HAS BEEN LOCKED
	IplImage* FaceDetector::getFace(int faceNum)
	{
		// Make sure their is currently a valid face at the index requested
		if(faceNum + 1 > maxFaces || faceNum + 1 > faces->total)
			return NULL;

		return faceBuffers[faceNum];
	}


	/////////////////////////////////////////////////////////////////////////
	/////////////////////ACCESSOR/MUTATOR FUNCTIONS//////////////////////////
	/////////////////////////////////////////////////////////////////////////
	void FaceDetector::setWinSize(CvSize winSize) { this->winSize = winSize; }
	void FaceDetector::setWinSize(int width, int height) { this->winSize = cvSize(width, height); }
	CvSize FaceDetector::getWinSize() { return this->winSize; }
	void FaceDetector::setMinNeighbors(int minNeighbors) { this->minNeighbors = minNeighbors; }
	int FaceDetector::getMinNeighbors() { return this->minNeighbors; }
	void FaceDetector::setScaleFactor(double scaleFactor) { this->scaleFactor = scaleFactor; }	
	double FaceDetector::getScaleFactor() { return this->scaleFactor; }
	void FaceDetector::setFlags(int flags) { this->flags = flags; }
	int FaceDetector::getFlags() { return this->flags; }
	CCritSec* FaceDetector::getBufferLock() { return this->bufferLock; }
	double FaceDetector::getDetectionTime() { return this->detectionTime; }
	IplImage* FaceDetector::getImageBuffer() { return this->imgBuffer; }
	CvSeq* FaceDetector::getFaceRects() { return this->faces; }
	void FaceDetector::lockBuffer() { this->bufferLock->Lock(); }
	void FaceDetector::unlockBuffer() { this->bufferLock->Unlock(); }

	void FaceDetector::setImageSize(int width, int height)
	{
		setImageSize(cvSize(width, height));
	}
	void FaceDetector::setImageSize(CvSize imageSize)
	{
		this->imageSize = imageSize;
		// Release the image if one is initalized
		if(this->imgBuffer)
			cvReleaseImage(&this->imgBuffer);
		this->imgBuffer = cvCreateImage(imageSize, 8, 3);
	}
	CvSize FaceDetector::getImageSize() { return this->imageSize; }

	int FaceDetector::getNumFaces()
	{
		if(faces != NULL)
			return faces->total;
		else
			return -1;
	}

	
	
}