/****************************************************************************************************
	DATE		USER	CHANGE LOG
====================================================================================================
23 Jul 2008 Vineeth	Created change log comments

23 Jul 2008 Vineeth Added code to interface with parallel port for haptic belt in drawFaces function
					Added input32.dll as reference, added inpout32.lib to debug folder, and linked
					library in project properties

****************************************************************************************************/
#include "StdAfx.h"
#include "VideoCapture.h"

// 23 Jul 2008, Vineeth
// Added prototype declaration to use inpout32.dll
//void _stdcall Out32(short portaddr, short datum);

/*
 *VideoCapture;
 */
VideoCapture::VideoCapture(void)
{
	HRESULT hr;
	this->filter = new TransInPlaceFilter(&hr);
}

VideoCapture::VideoCapture(TransInPlaceFilter transformFilter)
{

}

VideoCapture::~VideoCapture(void)
{
}

HRESULT VideoCapture::AddToRot(IUnknown *pUnkGraph)
{
	IMoniker* pMoniker;
    IRunningObjectTable *pROT;
    if (FAILED(GetRunningObjectTable(0, &pROT))) {
        return E_FAIL;
    }
    WCHAR wsz[256];
    wsprintfW(wsz, L"FilterGraph %08x pid %08x", (DWORD_PTR)pUnkGraph, GetCurrentProcessId());
    HRESULT hr = CreateItemMoniker(L"!", wsz, &pMoniker);
    if (SUCCEEDED(hr)) {
        hr = pROT->Register(ROTFLAGS_REGISTRATIONKEEPSALIVE, pUnkGraph,
            pMoniker, &dwRegister);
        pMoniker->Release();
    }
    pROT->Release();
    return hr;
}

bool VideoCapture::RenderVideo(HWND hwnd)
{
	this->hwnd = hwnd;
	CoInitialize(NULL);
	CoCreateInstance(CLSID_FilterGraph, NULL, CLSCTX_INPROC_SERVER, IID_IGraphBuilder, (void**)&pGraph);
	AddToRot(pGraph);
	pGraph->QueryInterface(IID_IMediaControl, (void**)&pControl);
	pGraph->QueryInterface(IID_IMediaEvent, (void**)&pEvent);
	
	// create a "builder"
	TRY(CoCreateInstance(CLSID_CaptureGraphBuilder2, NULL, CLSCTX_INPROC, IID_ICaptureGraphBuilder2, (void**)&pBuilder));
	TRY(pBuilder->SetFiltergraph(pGraph));
	
	// find a camera
	ICreateDevEnum* pDevEnum;
	CoCreateInstance(CLSID_SystemDeviceEnum, NULL, CLSCTX_INPROC, IID_ICreateDevEnum, (void**)&pDevEnum);
	IEnumMoniker* pClassEnum;
	pDevEnum->CreateClassEnumerator(CLSID_VideoInputDeviceCategory, &pClassEnum, 0);
	IMoniker* pMoniker;
	ULONG cFetched;	
	if (pClassEnum->Next(1, &pMoniker, &cFetched) == S_OK) {
	} else {
		return false;
	}
	pMoniker->BindToObject(0, 0, IID_IBaseFilter, (void**)&pCamera);	
	pGraph->AddFilter(pCamera, L"Camera");

	// deal with frame rate
	IAMStreamConfig* sc;
	TRY(pBuilder->FindInterface(&PIN_CATEGORY_CAPTURE, &MEDIATYPE_Video, pCamera, IID_IAMStreamConfig, (void **)&sc));
	
	AM_MEDIA_TYPE *pmt;
	int iCount, iSize;
	VIDEO_STREAM_CONFIG_CAPS caps;
	TRY(sc->GetNumberOfCapabilities(&iCount, &iSize));
	//IBasicVideo bv = (IBasicVideo)pGraph;
	//GetVideoSize(&width, &height);
	for (int i = 0; i < iCount; i++) {
		sc->GetStreamCaps(i, &pmt, (BYTE *)&caps);
		
		if (caps.InputSize.cx == 320) {
			// Set the capturing frame rate
			VIDEOINFOHEADER *pvi = (VIDEOINFOHEADER *)pmt->pbFormat;
			pvi->AvgTimePerFrame = (LONGLONG)(10000000 / 15);
			TRY(sc->SetFormat(pmt));
			DeleteMediaType(pmt);
			break; // found default setup, quit the loop
		}
		DeleteMediaType(pmt);
	}

	// init the face finder
	// (which needs to know how big the video stream is)
	width = caps.InputSize.cx;
	height = caps.InputSize.cy;
	filter->InitializeFilter(width, height);
//	InitFaceFinder(width, height);

	
	
	
	
	// add my filter
	HRESULT hr;
	
	pGraph->AddFilter(filter, L"My Filter");
	
	// "build" a preview thing (which passes each frame through "my filter")
	TRY(pBuilder->RenderStream(&PIN_CATEGORY_PREVIEW, &MEDIATYPE_Video, pCamera, filter, NULL));

	// set the window
	if (hwnd != 0) {
		IVideoWindow* v;
		TRY(pGraph->QueryInterface(IID_IVideoWindow, (void**)&v));
		TRY(v->put_Owner((OAHWND)hwnd));
		//TRY(v->put_WindowStyle(WS_CHILD|WS_CLIPSIBLINGS));
		TRY(v->put_WindowStyle(WS_CHILD));
		TRY(v->put_MessageDrain((OAHWND)hwnd));

		RECT rect;
		GetWindowRect(hwnd, &rect);
		TRY(v->SetWindowPosition(0, 0, rect.right - rect.left, rect.bottom - rect.top));

		TRY(v->Release());
	}
	
	// go
	pControl->Run();

	return true;
}

char * VideoCapture::getFace(int i, BYTE * image)
{
	int x;
	int y;
	int width;
	int height;
	char * buffer;

	CCritSec* lock = filter->detector->getBufferLock();
	lock->Lock();
	buffer = this->filter->detector->getFace(i, &x, &y, &width, &height);
	if(buffer != NULL)
	{
		memcpy(image, buffer, 128*128*3);
	}
	int maskSize = 0;

	lock->Unlock();
	return buffer;
}

int VideoCapture::getNumFaces()
{
	return this->filter->detector->getNumFaces();
}


/*
 *TransInPlaceFilter
 */
TransInPlaceFilter::TransInPlaceFilter(HRESULT *phr) : CTransInPlaceFilter(NAME("My Filter"), NULL, __uuidof(CLSID_TransInPlaceFilter), phr)
{
	//this->detector = new FaceDetector(this->width, this->height);
	bSaveFrameData = false;
	imgCount = 0;
}

TransInPlaceFilter::TransInPlaceFilter(): CTransInPlaceFilter(NAME("My Filter"), NULL, __uuidof(CLSID_TransInPlaceFilter), NULL)
{
	bSaveFrameData = false;
	imgCount = 0;
}

TransInPlaceFilter::~TransInPlaceFilter() 
{
	// If we were saving frame data...stop
	if(bSaveFrameData)		
		stopSavingFrameData();
}

void TransInPlaceFilter::InitializeFilter(int width, int height)
{
	this->detector = new FaceDetector(width, height);
	detector->setWinSize(20, 20);
	setSize(width, height);
}


void TransInPlaceFilter::setSize(int width, int height)
{
	this->width = width;
	this->height = height;
}

HRESULT TransInPlaceFilter::CheckInputType(const CMediaType *pmt)
{
		HRESULT   hr = E_FAIL;
		VIDEOINFO *pvi=0;
		
		CheckPointer(pmt,E_POINTER);
	
		// Reject the connection if this is not a video type
		if( *pmt->FormatType() != FORMAT_VideoInfo ) 
		{
			return E_INVALIDARG;
		}
		
		// Only accept RGB24 or YUYV video
		pvi = (VIDEOINFO *)pmt->Format();
	
		if( IsEqualGUID( *pmt->Type(), MEDIATYPE_Video) )
		{
			hr = S_OK;
	
			if( IsEqualGUID( *pmt->Subtype(), MEDIASUBTYPE_RGB24) )
			{
			}
			else
			{
				hr = DDERR_INVALIDPIXELFORMAT;
			}
		}
		if( FAILED(hr))
		{
			return hr;
		}
		return hr;
}

HRESULT TransInPlaceFilter::Transform(IMediaSample *pSample)
{
		if( !pSample )
			return E_POINTER;
		
		//TODO: Implment Transform filter
		BYTE * pSampleBuffer;
		TRY(pSample->GetPointer( &pSampleBuffer ));


		detector->flipBuffer((char*)pSampleBuffer);
		int numFaces = detector->findFaces((char*)pSampleBuffer);
		
		// 23 Jul 2008, Vineeth, Haptic Belt Interface
		// Send a "00000000" signal to the parallel port to reset all vibrators
		//Out32(0x378, 0); 	
		// 25 Sep 2008, Sreekar, Haptic Belt Interface
		// Reset All ports
		//Out32(0x378, 0);

		if(bSaveFrameData)
		{
			detector->lockBuffer();	// Make sure this frame isn't changed
			if(numFaces > 0)
			{
				frameDataFile << "frame=" << imgCount << "\n";
				CvSeq* faces = detector->getFaceRects();
				frameDataFile << "+numfaces=" << faces->total << "\n";
				CvRect faceRect;
				for(int i = 0; i < faces->total; i++)
				{
					faceRect = *(CvRect*)cvGetSeqElem(faces, i);
					frameDataFile << "++{x=" << faceRect.x << " y=" << faceRect.y <<
									" width=" << faceRect.width << " height=" << faceRect.height << "}\n";
				}
				frameDataFile << "\n";
				IplImage *curr = detector->getImageBuffer();
				
				char buffer[MAX_PATH];
				sprintf(buffer, "%s\\%d%s", folder, imgCount, ".jpg");
				cvSaveImage(buffer, curr);
			}
		}
		detector->drawFaces((char*)pSampleBuffer);
		
		if(bSaveFrameData)
		{
			if(numFaces > 0)
			{
				IplImage *curr = detector->getImageBuffer();
				
				char buffer[MAX_PATH];
				sprintf(buffer, "%s\\%d%s", folder, imgCount, " w rects.jpg");
				cvSaveImage(buffer, curr);
				imgCount++;

			}
			detector->unlockBuffer();	// Allow the frame to be changed again
		}

		detector->flipBuffer((char*)pSampleBuffer);
		
		return S_OK;
}

FaceDetector* VideoCapture::getFaceDetector() { return this->filter->detector; }
TransInPlaceFilter* VideoCapture::getFilter() { return this->filter; }
bool TransInPlaceFilter::saveFrameData(CString folderW)
{
	// Check to see if we're already saving
	if(bSaveFrameData)
		stopSavingFrameData();

	imgCount = 0; // reset the image count
	
	folderW = L"frame info\\" + folderW;
	
	//folderW.Format(L"frame info\\%s", folderW);
	CreateDirectory(folderW, NULL);
	CString filename;
	filename.Format(L"%s\\%s", folderW, L"frame info.txt");

	// Convert the filename to single byte characters
	char buffer[MAX_PATH];
	int i;
	for(i = 0; i < filename.GetLength(); i++)
		buffer[i] = filename[i];
	buffer[i] = '\0';
	
	frameDataFile.open(buffer, ios::out);
	if(frameDataFile.fail())
		return false;
	
	// Convert just the folder to single byte character
	for(i = 0; i < folderW.GetLength(); i++)
		folder[i] = folderW[i];
	folder[i] = '\0';

	this->bSaveFrameData = true;

	return true;
}

bool TransInPlaceFilter::stopSavingFrameData()
{
	if(!bSaveFrameData)
		return false;
	
	frameDataFile.close();

	bSaveFrameData = false;
}