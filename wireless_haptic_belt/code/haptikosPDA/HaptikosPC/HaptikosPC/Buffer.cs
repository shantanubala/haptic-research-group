using System;


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
            this.s = s;
        }

        //Methods
        internal void SetBuffer(byte type, byte[] new_value) {
            s.GetLock();
            _byteBufferType = type;
            _byteBuffer = new_value;
            s.Unlock();
        }
        internal void AppendBuffer(byte type, byte[] append_value) {
            s.GetLock();
            //for (int i = 0; i < msg.Length; i++) {
            //    if (!_append_msg)   // empty garbage ReturnData
            //        _statusBuffer[i] = 0x00;
            //    _statusBuffer[i] = msg[i];
            //}
            //_byteBufferType = type;
            //_byteBuffer = append_value;
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
