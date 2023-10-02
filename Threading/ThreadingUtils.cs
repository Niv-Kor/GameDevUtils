using System.Threading.Tasks;
using UnityEngine.Events;

namespace GameDevUtils.Threading
{
    public static class ThreadingUtils
    {
        /// <summary>
        /// Invoke a function after a set delay.
        /// </summary>
        /// <param name="action">The function to invoke</param>
        /// <param name="delay">The time to wait before invoking the function [s]</param>
        public static async Task SetTimeout(UnityAction action, float delay) {
            int msDelay = (int)(delay * 1000);
            await Task.Delay(msDelay);
            action();
        }
    }
}
