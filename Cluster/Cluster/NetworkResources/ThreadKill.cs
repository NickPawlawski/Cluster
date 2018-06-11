namespace Cluster.NetworkResources
{
    internal static class ThreadKill
    {
        public static bool ThreadKillFlag { get; private set; }

        public static void KillThread(bool status)
        {
            ThreadKillFlag = status;
        }
    }
}
