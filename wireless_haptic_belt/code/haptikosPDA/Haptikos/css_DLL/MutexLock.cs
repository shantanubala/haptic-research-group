/*****************************************************************************
 * FILE:   MutexLock.cs
 * AUTHOR: Nathan J. Edwards (nathan.edwards@asu.edu)
 *         
 * DESCR:  Class provides synchronization Monitor utility so that 
 *         DLL synchronization does not rely on the slower performance 
 *         OS system call (or wrapper classes)
 * LOG:    20091109 - initial version
 * 
 ****************************************************************************/

//using System;

namespace HapticDriver
{
    internal class MutexLock
    {
        byte s; // mutex lock (also known as binary semaphore)

        //Constructors
        internal MutexLock() {
            s = 0x1; 
        }

        //Synchronization Methods
        internal void GetLock() {
            if (s == 0x1)
                s = 0x0;
            else {
                while (s == 0x0)
                    System.Threading.Thread.Sleep(0); // wait for other threads
            }
        }
        internal void Unlock() {
            s = 0x1;
        }
    }
}