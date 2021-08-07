using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;


namespace DataCollection.Helpers
{
    public class Enums
    {

        /// <summary>
        /// HangFire için görev çalıştırma durumları
        /// </summary>
        public enum ProcessStatus
        {
            All = 1,
            Processing = 2,
            Failed = 3,
            Succeeded = 4
        }

    }
}
