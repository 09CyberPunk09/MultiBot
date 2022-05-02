namespace Kernel
{
    public static class Constants
    {
        public static bool IsDebugMode
        {
            get
            {
#if DEBUG
                return true;
#else
            return false;
#endif
            }
        }
    }
}
