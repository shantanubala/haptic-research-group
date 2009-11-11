{\rtf1\ansi\ansicpg1252\cocoartf949\cocoasubrtf460
{\fonttbl\f0\fswiss\fcharset0 Helvetica;}
{\colortbl;\red255\green255\blue255;}
\margl1440\margr1440\vieww9000\viewh8400\viewkind0
\pard\tx720\tx1440\tx2160\tx2880\tx3600\tx4320\tx5040\tx5760\tx6480\tx7200\tx7920\tx8640\ql\qnatural\pardirnatural

\f0\fs24 \cf0 // HapticComConsole.cpp : Defines the entry point for the console application.\
//\
\
#include "StdAfx.h"\
#include <stdio.h>\
#include <tchar.h>\
#include <iostream>\
#include <windows.h>\
#include<string.h>\
#include <strsafe.h>\
#include<string>\
\
using namespace std; //as I now understand this is bad form\
\
\
\
	typedef string* (*getPortNamesFunc)();\
	typedef void (*SetupPortsFunc)(string,string,string,string,string,string,string);\
	typedef void (*OpenPortsFunc)();\
	typedef void (*ClosePortsFunc)();\
	typedef string* (*Query_AllFunc)();\
	typedef string* (*StopAllFunc)();\
	typedef string* (*Vibrate_MotorFunc)(string,string,string,int);\
\
\
\
	//function to grab and decode last error code\
	//from msdn somwhere\
void ErrorExit(LPTSTR lpszFunction) \
\{ \
    // Retrieve the system error message for the last-error code\
\
    LPVOID lpMsgBuf;\
    LPVOID lpDisplayBuf;\
    DWORD dw = GetLastError(); \
\
    FormatMessage(\
        FORMAT_MESSAGE_ALLOCATE_BUFFER | \
        FORMAT_MESSAGE_FROM_SYSTEM |\
        FORMAT_MESSAGE_IGNORE_INSERTS,\
        NULL,\
        dw,\
        MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),\
        (LPTSTR) &lpMsgBuf,\
        0, NULL );\
\
    // Display the error message and exit the process\
\
    lpDisplayBuf = (LPVOID)LocalAlloc(LMEM_ZEROINIT, \
        (lstrlen((LPCTSTR)lpMsgBuf) + lstrlen((LPCTSTR)lpszFunction) + 40) * sizeof(TCHAR)); \
    StringCchPrintf((LPTSTR)lpDisplayBuf, \
        LocalSize(lpDisplayBuf) / sizeof(TCHAR),\
        TEXT("%s failed with error %d: %s"), \
        lpszFunction, dw, lpMsgBuf); \
    MessageBox(NULL, (LPCTSTR)lpDisplayBuf, TEXT("Error"), MB_OK); \
\
    LocalFree(lpMsgBuf);\
    LocalFree(lpDisplayBuf);\
    //ExitProcess(dw); \
\}\
\
\
int _tmain(int argc, _TCHAR* argv[])\
\{\
\
	string inboundPort = "COM6";\
	string outboundPort = "COM6";\
	string baud_string = "9600";\
	string parity_string = "None";\
	string stopbits_string = "1";\
	string databits_string = "8";\
	string readTimeout_string = "1000";\
	string response[2]; //just a guess..compiles anyway\
	string motor = "A";\
	string rhy="A";\
	string mag="A";\
	int cycles=1;\
\
	getPortNamesFunc  _getPortNamesFunc;\
	SetupPortsFunc    _SetupPortsFunc;\
	OpenPortsFunc     _OpenPortsFunc;\
	ClosePortsFunc    _ClosePortsFunc;\
	Query_AllFunc     _Query_AllFunc;\
	StopAllFunc       _StopAllFunc;\
	Vibrate_MotorFunc _Vibrate_MotorFunc;\
\
   HINSTANCE hInstLibrary = LoadLibrary(_T("HapticDriver.DLL"));\
\
   if (hInstLibrary)\
   \{\
	  _getPortNamesFunc = (getPortNamesFunc)GetProcAddress(hInstLibrary,"getPortNames");\
	  _SetupPortsFunc   = (SetupPortsFunc)GetProcAddress(hInstLibrary,"SetupPorts");\
	  _OpenPortsFunc    = (OpenPortsFunc)GetProcAddress(hInstLibrary,"OpenPorts");\
	  _ClosePortsFunc   = (ClosePortsFunc)GetProcAddress(hInstLibrary,"ClosePorts");\
	  _Query_AllFunc    = (Query_AllFunc)GetProcAddress(hInstLibrary,"Query_All");\
	  _StopAllFunc      = (StopAllFunc)GetProcAddress(hInstLibrary,"StopAll");\
	  _Vibrate_MotorFunc= (Vibrate_MotorFunc)GetProcAddress(hInstLibrary,"Vibrate_Motor");\
\
      cout << "Startup Belt";\
\
      if (_getPortNamesFunc)\
      \{\
         cout << " Get Port Names " <<_getPortNamesFunc();\
      \}\
	  else  ErrorExit(TEXT("GetProcessId"));\
\
      if (_SetupPortsFunc)\
      \{\
		  cout << " Setup Ports ";\
         _SetupPortsFunc(inboundPort,outboundPort,baud_string,databits_string,stopbits_string,parity_string, readTimeout_string);\
      \}\
	  else  ErrorExit(TEXT("GetProcessId"));\
\
      if (_OpenPortsFunc)\
      \{\
		  cout << " Open Ports ";\
         _OpenPortsFunc();\
      \}\
	  else  ErrorExit(TEXT("GetProcessId"));\
\
      if (_Query_AllFunc)\
      \{\
		 cout << " Query All ";\
         cout << _Query_AllFunc();\
      \}\
	  else  ErrorExit(TEXT("GetProcessId"));\
\
	  if (_Vibrate_MotorFunc)\
      \{\
		 cout << " Vibrate ";\
		 cout << _Vibrate_MotorFunc(motor,rhy,mag,cycles);\
      \}\
  	  else  ErrorExit(TEXT("GetProcessId"));\
\
	  if (_StopAllFunc)\
      \{\
		cout << " Stop All ";\
        cout <<  _StopAllFunc();\
      \}\
  	  else  ErrorExit(TEXT("GetProcessId"));\
	        \
	  \
	  if (_ClosePortsFunc)\
      \{\
		  cout << " Close Ports ";\
         _ClosePortsFunc();\
      \}\
  	  else  ErrorExit(TEXT("GetProcessId"));\
\
\
      FreeLibrary(hInstLibrary);\
   \}\
   else\
   \{\
      std::cout << "Haptic Driver DLL Failed To Load!" << std::endl;\
   \}\
\
   \
	std::cin.get(); //pause for user input\
\
   return 0;\
\}\
\
}