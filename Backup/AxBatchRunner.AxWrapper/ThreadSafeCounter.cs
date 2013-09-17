using System.Threading;

namespace AxBatchRunner.AxWrapper
{
    /// <summary>
    ///   Implement thread safe counter (long)
    /// </summary>
    internal sealed class ThreadSafeCounter
    {
        private long _value;
        private readonly object _locker = new object();

        public void Clear()
        {
            lock (_locker)
            {
                _value = 0;
            }
        }

        public void Decrement()
        {
            lock (_locker)
            {
                if (_value > 0L)
                {
                    _value -= 1L;
                }
            }
        }

        public void Increment()
        {
            Interlocked.Increment(ref _value);
        }

        public long Value
        {
            get { return Interlocked.Read(ref _value); }
        }
    }
}