using System.Threading;

namespace AxBatchRunner.AxWrapper
{
    /// <summary>
    ///   Implementation of thread safe flag (bool)
    /// </summary>
    internal sealed class ThreadSafeFlag
    {
        private long _value;
        private readonly object _locker = new object();
        
        public void Clear()
        {
            Value = false;
        }

        public bool IsSet()
        {
            return Value;
        }

        public void Set()
        {
            Value = true;
        }

        public bool Value
        {
            get { return (Interlocked.Read(ref _value) != 0L); }
            set
            {
                lock (_locker)
                {
                    _value = value ? (1) : (0);
                }
            }
        }
    }
}