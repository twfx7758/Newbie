using System;
using System.Diagnostics;
using System.Threading;

namespace Newbie.Util.Lock
{
    public class CheapUnfairReaderWriterLock
    {
        object writerFinishedEvent;

        int readersIn;
        int readersOut;
        bool writerPresent;

        object syncRoot;

        // Spin lock params
        const int MAX_SPIN_COUNT = 100;
        const int SLEEP_TIME = 500;


        public CheapUnfairReaderWriterLock()
        {
        }


        object SyncRoot
        {
            get
            {
                if (this.syncRoot == null)
                {
                    Interlocked.CompareExchange(ref this.syncRoot, new object(), null);
                }
                return this.syncRoot;
            }
        }


        bool ReadersPresent
        {
            get
            {
                return this.readersIn != this.readersOut;
            }
        }


        ManualResetEvent WriterFinishedEvent
        {
            get
            {
                if (this.writerFinishedEvent == null)
                {
                    Interlocked.CompareExchange(ref this.writerFinishedEvent, new ManualResetEvent(true), null);
                }
                return (ManualResetEvent)this.writerFinishedEvent;
            }
        }


        public int AcquireReaderLock()
        {
            int readerIndex = 0;
            do
            {
                if (this.writerPresent)
                {
                    WriterFinishedEvent.WaitOne();
                }

                readerIndex = Interlocked.Increment(ref this.readersIn);

                if (!this.writerPresent)
                {
                    break;
                }

                Interlocked.Decrement(ref this.readersIn);
            }
            while (true);

            return readerIndex;
        }


        public void AcquireWriterLock()
        {
#pragma warning disable 0618
            //@
            Monitor.Enter(this.SyncRoot);
#pragma warning restore 0618

            this.writerPresent = true;
            this.WriterFinishedEvent.Reset();

            do
            {
                int i = 0;
                while (ReadersPresent && i < MAX_SPIN_COUNT)
                {
                    Thread.Sleep(0);
                    i++;
                }

                if (ReadersPresent)
                {
                    Thread.Sleep(SLEEP_TIME);
                }
            }
            while (ReadersPresent);
        }


        public void ReleaseReaderLock()
        {
            Interlocked.Increment(ref this.readersOut);
        }


        public void ReleaseWriterLock()
        {
            try
            {
                this.writerPresent = false;
                this.WriterFinishedEvent.Set();
            }
            finally
            {
                Monitor.Exit(this.SyncRoot);
            }
        }
    }
}
