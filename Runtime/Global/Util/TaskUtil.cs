using System;
using System.Threading.Tasks;

namespace Lumos.DevPack
{
    public class TaskUtil
    {
        public static async Task WaitUntil(Func<bool> predicate, int checkIntervalMs = 50)
        {
            while (!predicate())
            {
                await Task.Delay(checkIntervalMs);
            }
        }
    }
}