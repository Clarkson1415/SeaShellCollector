using UnityEngine;

namespace Assets.Scripts
{
    public static class MyLog
    {
        private static bool LogEnabled = true; // So can toggle on off for performance

        public static void Log(string message)
        {
            if (!LogEnabled)
            {
                return;
            }

            Debug.Log(message);
        }

        public static void LogError(string message)
        {
            if (!LogEnabled)
            {
                return;
            }

            Debug.LogError(message);
        }

        public static void LogWarning(string message)
        {
            if (!LogEnabled)
            {
                return;
            }

            Debug.LogWarning(message);
        }
    }
}
