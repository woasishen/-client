using System;
using System.Threading;

namespace TcpConnect.Socket
{
    internal class ReadWriteLockUtil
    {
        private readonly ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim();

        public void WriteWithLock(Action action)
        {
            _readerWriterLock.EnterWriteLock();
            action.Invoke();
            _readerWriterLock.ExitWriteLock();
        }

        public T WriteWithLock<T>(Func<T> action)
        {
            _readerWriterLock.EnterWriteLock();
            var result = action.Invoke();
            _readerWriterLock.ExitWriteLock();
            return result;
        }

        public void ReadWithLock(Action action)
        {
            _readerWriterLock.EnterReadLock();
            action.Invoke();
            _readerWriterLock.ExitReadLock();
        }

        public T ReadWithLock<T>(Func<T> action)
        {
            _readerWriterLock.EnterReadLock();
            var result = action.Invoke();
            _readerWriterLock.ExitReadLock();
            return result;
        }
    }
}
