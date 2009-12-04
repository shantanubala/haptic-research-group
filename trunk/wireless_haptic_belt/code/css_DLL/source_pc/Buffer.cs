/*****************************************************************************
 * FILE:   Buffer.cs
 * AUTHOR: Nathan J. Edwards (nathan.edwards@asu.edu)
 *         
 * DESCR:  Class provides synchronized binary data structure (byte[]) used 
 *         for incoming and outgoing serial communications.
 * LOG:    20091109 - initial version
 * 
 ****************************************************************************/

namespace HapticDriver
{
    internal class Buffer
    {
        private byte[] _byteBuffer;
        private byte _byteBufferType;
        private static int _defaultBufferSize = 512;
        MutexLock s;

        //Constructors
        internal Buffer(MutexLock s) {
            _byteBuffer = new byte[_defaultBufferSize];
            _byteBufferType = 0x0;
            this.s = s;
        }
        internal Buffer(MutexLock s, int bufferSize) {
            _byteBuffer = new byte[bufferSize];
            _byteBufferType = 0x0;
            this.s = s;
        }

        //Methods
        internal void SetBuffer(byte type, byte[] new_value) {
            s.GetLock();
            _byteBufferType = type;
            
            // Clear garbage
            for (int i = 0; i < _byteBuffer.Length; i++)
                _byteBuffer[i] = 0x0;
            
            // Set new Value
            _byteBuffer = new_value;
            s.Unlock();
        }
        internal void AppendBuffer(byte type, byte[] append_value) {
            s.GetLock();
            _byteBufferType = type;
            for (int i = 0; i < append_value.Length; i++) {
                // if current location is less that allocated memory size then concatenate
                if (_byteBuffer[_byteBuffer.Length + i] < _byteBuffer.Rank)
                    _byteBuffer[_byteBuffer.Length + i] = append_value[i];
            }
            s.Unlock();
        }

        internal byte[] GetBuffer() {
            s.GetLock();
            byte[] ret_value = _byteBuffer;
            s.Unlock();
            return ret_value;
        }

        internal byte GetBufferType() {
            s.GetLock();
            byte ret_value = _byteBufferType;
            s.Unlock();
            return ret_value;
        }

    }
}
