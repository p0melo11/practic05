using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace producerconsumer
{
    public class producerconsumer<T>
    {
        private object _mutex = new object();
        private Queue<T> _queue = new Queue<T>();
        private bool _isDead = false;

        public void Enqueue(T task)
        {
            if (task == null)
                throw new ArgumentNullException("task");
            lock (_mutex)
            {
                if (_isDead)
                    throw new InvalidOperationException("Queue already stopped");
                _queue.Enqueue(task);
                Monitor.Pulse(_mutex);
            }
        }

        public T Dequeue()
        {
            lock (_mutex)
            {
                while (_queue.Count == 0 && !_isDead)
                    Monitor.Wait(_mutex);

                if (_queue.Count == 0)
                    throw new Exception("Queue empty");

                return _queue.Dequeue();
            }
        }

        public void Stop()
        {
            lock (_mutex)
            {
                _isDead = true;
                Monitor.PulseAll(_mutex);
            }
        }
    }
}
