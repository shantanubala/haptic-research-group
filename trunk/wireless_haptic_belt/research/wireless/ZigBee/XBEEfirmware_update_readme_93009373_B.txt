
**************************************************************************

        Firmware Revision History - XBee/XBee-PRO ZB Radio Modems

**************************************************************************


** XBee-ZB Firmware Overview **

The XBee ZB firmware is based off the EmberZNet 3.x.x ZigBee-PRO stack.  

XBee-ZNet 2.5 modules can be upgraded to support XBee-ZB functionality through a firmware upgrade.  
While the AT commands and API frames are nearly identical between ZNet 2.5 and ZB firmware, 
XBee-ZNet 2.5 and XBee-ZB are not over-the-air compatible.


** XBee-ZB Firmware Versions **

XBee version numbers will have 4 significant digits. A version number is reported by issuing an 
ATVR command.  The response returns 3 or 4 numbers. All numbers are hexadecimal and can have a 
range from 0-0xF. A version is reported as "ABCD". Digits ABC are the main release number and D 
is the revision number from the main release. "B" is a variant designator. The following 
variants exist in ZB firmware:

•	“0" - Coordinator, AT Command Mode (AP=0) 
•	“1" - Coordinator, API Mode (AP=1,2) 
•	“2" - Router AT Command Mode (AP=0) 
•	“3" - Router API Mode (AP=1,2) 
•	“8” – End Device, AT Command Mode (AP=0)
•	“9” – End Device, API Mode (AP=1,2)

Digi has developed an assortment of sensor adapter products that use the ZB firmware.  (See
www.digi.com for details.)  Adapter firmware versions include the following variants:
•	“4" - Router/End Device Sensor Adapter
•	“5" - End Device Power Harvester Adapter
•	“6" - Router/End Device Analog IO Adapter
•	“7" - Router/End Device Digital IO Adapter

All releases will have an even number for C. All internal development will have an odd number 
for C. Field D is always present, even when D is 0. 


** Released Versions **

Released ZigBee firmware versions are available from the X-CTU program for general download.  
To download released versions:

1.  Go to the ‘Modem Configuration’ page in X-CTU.
2.  Click on ‘Download New Versions’ and select ‘Web’.
3.  You may need to disable your firewall in order to download new versions.



**************************************************************************
*                       Version 2x41 Release Notes                       *
**************************************************************************
Release Date:  September 2nd, 2008
ZigBee Stack:  EmberZNet 3.2.0 ZigBee-PRO stack


FEATURES
-  Added SI (sleep immediately) command to put cyclic sleep XBee to sleep immediately, instead 
   of waiting for ST to expire.
-  Poll timeout implemented on parent router / coordinator.  If an end device child does not 
   poll within this timeout, it is removed from the child table.  The timeout is (3 * SP * SN) 
   tenths of a second (since SP is measured in tenths of a second).
-  API targets support source routing.  Two new API frames created to support source routing:
   *  0x21 - Create Source Route
   *  0xA2 - Source Route Record Indicator
-  End devices support IO sampling, network discovery command (ATND).
-  End devices can receive broadcast messages on stack profile 0 (ATZS).


BUG FIXES (fixed since last release)
-  API devices could only transmit up to 72 bytes.  Can now transmit more (see NP command).
-  Broadcast transmissions were not received relialbly by end devices when operating on stack
   profile 0 (stack fix).
-  Fixed memory leak during over-the-air firmware updates.
-  Fixed API memory leak where corrupt API frames caused occasional buffer loss.
-  API end device, when disassociated from network, did not return correct frame ID in tx-status.
-  API end device issued tx-status messages with 16-bit address set to 0xFFFD instead of the real 
   16-bit address.
-  NR1 caused immediate router leaves.  Now, routers that receive NR1 wait 6 seconds before 
   leaving.
-  Changing SN did not change sleep time (SP*SN) immediately.
-  Fixed bug in NH (router unicast timeout).
-  Sending multiple API broadcasts with different APP ID values yielded tx-status messages with 
   the same APP ID value (last APP ID).
-  API router with RTS enabled, de-asserted since powerup would not respond to network discovery.
-  Commissioning button 2-button press caused joining to be disabled after 1 minute, even on 
   devices where NJ=0xFF.
-  Fixed bug where transmitting a large block of data to a remote resulted in data loss on the
   remote.
-  Loopback cluster ID (0x0012) only supported within Digi Profile ID (0xC105).



**************************************************************************
*                       Version 2x21 Release Notes                       *
**************************************************************************
Release Date:  June 13th, 2008
ZigBee Stack:  EmberZNet 3.1.1


FEATURES
-  Added the ID command to improve compatibility with other XBee variations.  ATID can set / read 
   the ZigBee extended PAN ID.  This replaces the EI command in 2x20.


KNOWN ISSUES
-  IO sampling not yet supported on end devices.
-  End devices cannot initiate a network discovery command (ATND).
-  End devices do not receive broadcast messages on stack profile 0 (ATZS).
-  Incompatible with 2x20 if EI is set (in 2x20) to a non-default value (EI > 0).


BUG FIXES (fixed since last release)
-  The EI command (now ID) set the extended PAN ID in little endian byte order in 2x20.
-  Certain conditions would cause end devices and routers to stop attempting to join a network.
-  Sending multiple serial API transmit frames could cause frames to not be processed.
-  Fixed problems with ATNR0 command.
   *  Issuing ATNR0 reset the entire network.
   *  End devices did not support the NR1 command.
-  Improved sleep current on XBee-PRO modules.


OTHER CHANGES (since last release)
-  KY now sets the link key on all devices.
-  Added the NK command to set the network key on the coordinator.
-  SC defaults to enable 12 channels (0x1FFE) instead of 14.
-  JN default set to 0 to help prevent excess broadcasting.



**************************************************************************
*                       Version 2x20 Release Notes                       *
**************************************************************************
Release Date:  April 8th, 2008
ZigBee Stack:  EmberZNet 3.1.1


FEATURES
-  Functionality similar to ZNet 2.5 firmware.
-  AT and API versions.
-  Remote configuration command support.
-  Over-the-air firmware updates supported.
-  Supports interoperability with other ZigBee products.
   *  API Data transmissions allow flexibility in specifying endpoints, cluster ID, and 
      profile ID values.
   *  API receive frame (0x91) indicates endpoints, cluster ID, and profile ID of each 
      received RF packet.
   *  The stack profile is configurable (ZS command).
   *  Many ZDO commands can be issued to devices on the network.







**************************************************************************
                             Known Issues
**************************************************************************
The following are known issues in the ZB hardware / software.


******  Channel Crosstalk  ******
The EM250 suffers from a crosstalk issue where data received on a channel that is 12 channels 
above or below the current channel appears to be received on the current channel.  RF data is 
detected on the erroneous channel only if the signal level of the received RF data is around 
- 30dBm (XBee devices separated by a few feet).  For example, RF traffic on channel 0x0B 
could be seen on channels 0x0B and 0x17 if its detected signal level is around -30dBm. This 
could result in routers or end devices that join a PAN, but reporting an incorrect channel. 

The problem can be avoided by setting the SC (scan channels) bitmask to only include 12 
continuous channels. If SC is left at its default value (0x1FFE), the crosstalk issue will 
never occur.



******   Simulated EEPROM Erases   ******
The Ember ZigBee stack occasionally writes information to non-volatile memory.  These writes
may require performing a flash page erase which can block up to 20ms (worst case).  The 
EM250 has a 4-byte FIFO to collect incoming serial data.  However, if serial uart interrupts 
are disabled for 20ms, it is possible for incoming serial data to be dropped once the 4-byte
FIFO filles.  ZB firmware de-asserts CTS about 40 microseconds before an erase operation 
begins.  The application should observe CTS flow control as quickly as possible to prevent 
data loss.  

Note that PCs often make use of a FIFO buffer where serial data can be buffered prior to 
transmission to the serial port.  If FIFO buffers are used, PC applications will not be 
immediately responsive to CTS and may experience rare data loss.





