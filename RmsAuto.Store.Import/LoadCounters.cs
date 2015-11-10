using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.Store.Import
{
	public sealed class LoadCounters
	{
		public LoadCounters()
		{
			Added = 0;
			Updated = 0;
			Deleted = 0;
			Skipped = 0;
            Aborted = 0;

            BatchCount = 0;
            BatchTime = new TimeSpan();
            DelayTime = new TimeSpan();
		}

        public int Added { get; internal set; }
        public int Updated { get; internal set; }
        public int Deleted { get; internal set; }
        public int Skipped { get; internal set; }
        //Флаг -1 означает, что при проверке лока на базу данных его получить не удалось
        public int Aborted { get; internal set; }

        public int BatchCount { get; internal set; }
        public TimeSpan BatchTime { get; internal set; }
        public TimeSpan DelayTime { get; internal set; }
    }
}
