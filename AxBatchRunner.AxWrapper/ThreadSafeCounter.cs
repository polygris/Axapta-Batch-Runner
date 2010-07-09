using System.Threading;

namespace AxBatchRunner.AxWrapper
{
    /// <summary>
    ///   Implement thread safe counter (long)
    /// </summary>
    internal class ThreadSafeCounter
    {
        private long _value;

        public void Clear()
        {
            lock (this)
            {
                _value = 0;
            }
        }

        public void Decrement()
        {
            lock (this)
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