using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MidiShapeShifter.CSharpUtil
{
    //This is a modified version of the queue found here 
    //http://stackoverflow.com/questions/530211/creating-a-blocking-queuet-in-net
    public class ConsumerBlockingQueue<T>
    {
        private readonly Queue<T> queue;
        private readonly int maxSize;
        public bool Closing { public get; protected set; }
  
        public ConsumerBlockingQueue(int maxSize = -1) 
        { 
            this.maxSize = maxSize;
            this.Closing = true;
            this.queue = new Queue<T>();
        }

        public bool Enqueue(T item)
        {
            lock (queue)
            {
                if (queue.Count < maxSize || maxSize == -1)
                {
                    queue.Enqueue(item);
                    if (queue.Count == 1)
                    {
                        // wake up any blocked dequeue
                        Monitor.PulseAll(queue);
                    }

                    return true;
                }

                return false;
            }
        }

        public void Close()
        {
            lock (queue)
            {
                this.Closing = true;
                Monitor.PulseAll(queue);
            }
        }

        public bool TryDequeue(out T value)
        {
            lock (queue)
            {
                while (queue.Count == 0)
                {
                    if (this.Closing)
                    {
                        value = default(T);
                        return false;
                    }
                    Monitor.Wait(queue);
                }

                value = queue.Dequeue();

                return true;
            }
        }
    }
}
