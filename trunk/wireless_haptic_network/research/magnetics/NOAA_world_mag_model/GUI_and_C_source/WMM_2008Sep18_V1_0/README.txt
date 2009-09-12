The World Magnetic Model
------------------------

GUI:
To run the Graphical User Interface for the WMM-2005, open the subfolder 'exe' and click on WMM.exe

Source code:
The subfolders 'StringGrid' and 'WMM2005GUI' contain the source code. It can be re-compiled by the follwing steps:
1. Open StringGrid/CustomStringGrid.bdsproj with Borland C++ 2006.
2. Go to Project>Build Configurations.
3. Select the available project and select Edit.
4. Highlight Release Build and click Activate.
5. Exit out of those windows back to the main window.
6. Right click on CustomStringGrid.bpl in the project manager.
7. Select install from the right click menu.
8. Expect message "C:\Documents and Settings\...\bpl\CustomGrid.bpl has been installed.  The following new components have been registered: TCustomStringGrid."
9. Select File>Close All.
10. Open WMM2005GUI/WMM.bdsproj with Borland C++ 2006.
11. Repeat steps 2-5.
12. Open the WMMForm1.cpp file for editing.  (If the GUI form is in view instead of the GUI souce code, look for options "WMMForm1.cpp/WMMForm1.h/Design/History" at the bottom of the WMMForm1 window, and choose "WMMForm1.cpp".)
13. Change lines 13-15 to point to the indicated files on your machine.
14. Open the WMMForm1.h file for editing.  (If the GUI form is in view instead of the GUI souce code, look for options "WMMForm1.cpp/WMMForm1.h/Design/History" at the bottom of the WMMForm1 window, and choose "WMMForm1.h".)
15. Change line 14 to point to the correct path for file "CStringGrid.h"
16. Select Project > Build
17. Select Run > Run.



Background:
The World Magnetic Model is a product of the United States National Geospatial-Intelligence Agency (NGA). The U.S. National Geophysical Data Center (NGDC) and the British Geological Survey (BGS) produced the WMM with funding provided by NGA in the USA and by the Defence Geographic Imagery and Intelligence Agency (DGIA) in the UK.

The World Magnetic Model is the standard model of the US Department of Defense, the UK Ministry of Defence, the North Atlantic Treaty Organization (NATO), and the World Hydrographic Office (WHO) navigation and attitude/heading referencing systems. It is also used widely in civilian navigation systems. The model, associated software, and documentation are distributed by NGDC on behalf of NGA. The model is produced at 5-year intervals, with the current model expiring on December 31, 2009.

Contact:
The current model, WMM-2005 (published 12/2004), is available from
http://www.ngdc.noaa.gov/seg/WMM/DoDWMM.shtml

Please address questions to Stefan.Maus@noaa.gov