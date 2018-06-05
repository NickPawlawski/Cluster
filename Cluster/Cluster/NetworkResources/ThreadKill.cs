using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cluster.NetworkResources
{
    static class ThreadKill
    {
        private static bool threadKill = false;
        public static bool ThreadKillFlag => threadKill;

        public static void KillThread(bool status)
        {
            threadKill = status;
        }
    }
}
