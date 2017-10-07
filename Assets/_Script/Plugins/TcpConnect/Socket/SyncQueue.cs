using System.Collections.Generic;
using System.Threading;

namespace TcpConnect.Socket
{
    public class SyncQueue<T> : Queue<T>
    {
        private readonly Semaphore _semaphore = new Semaphore(0, int.MaxValue);
        private readonly ReadWriteLockUtil _lockUtil = new ReadWriteLockUtil();
        public new void Enqueue(T item)
        {
            _lockUtil.WriteWithLock(() => base.Enqueue(item));
            _semaphore.Release();
        }

        public new T Dequeue()
        {
            _semaphore.WaitOne();
            return _lockUtil.WriteWithLock(() => base.Dequeue());
        }
    }
}
