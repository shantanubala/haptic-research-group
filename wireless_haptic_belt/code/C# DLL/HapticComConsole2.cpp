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
#include <vector>\
\
using namespace std; //remove\
using namespace System;\
using namespace System::Runtime::InteropServices;\
\
\
\
\
int _tmain(int argc, _TCHAR* argv[])\
\{\
\
	HapticOutput::HapticBelt^ belt  = gcnew HapticOutput::HapticBelt();\
	//look into this,  does this mean unmanaged, do I need to free it?\
\
	System::String^ inboundPort = "COM6";\
	System::String^ outboundPort = "COM6";\
	System::String^ baud_string = "9600";\
	System::String^ parity_string = "None";\
	System::String^ stopbits_string = "1";\
	System::String^ databits_string = "8";\
	System::String^ readTimeout_string = "1000";\
	array<System::String ^> ^  response; //this is not necesarrily working\
	System::String^ motor = "2";\
	System::String^ rhy="A";\
	System::String^ mag="A";\
	int cycles=1;\
\
	belt->SetupPorts(inboundPort,outboundPort,baud_string,databits_string,stopbits_string,parity_string,readTimeout_string);\
	\
	belt->OpenPorts();\
\
	response=belt->Query_All(); \
\
	cout << belt->Vibrate_Motor("A",rhy,mag,cycles);\
	cout << belt->Vibrate_Motor("1",rhy,mag,cycles);\
	cout << belt->Vibrate_Motor("2",rhy,mag,cycles);\
	cout << belt->Vibrate_Motor("3",rhy,mag,cycles);\
	cout << belt->Vibrate_Motor("4",rhy,mag,cycles);\
	cout << belt->Vibrate_Motor("5",rhy,mag,cycles);\
\
	std::cin.get(); //pause for user interaction\
\
   return 0;\
\}\
\
}