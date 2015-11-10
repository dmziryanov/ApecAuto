using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RmsAuto.Store.Import
{
    internal sealed class BatchUtil
    {
        public static void ExecBatched(Func<bool> processBatch,
            BatchParams batchParams, LoadCounters counters)
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (processBatch())
            {
                sw.Stop();
                int delay = batchParams.Delay;
                if (batchParams.DelayMode == DelayMode.Percentage)
                {
                    delay = (int)(sw.ElapsedMilliseconds * delay / 100m);
                }
                counters.BatchCount++;
                counters.BatchTime += sw.Elapsed;
                counters.DelayTime += new TimeSpan(0, 0, 0, 0, delay);

                System.Threading.Thread.Sleep(delay);

                sw.Reset();
                sw.Start();
            }
        }
    }
}
