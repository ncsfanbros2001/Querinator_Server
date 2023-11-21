using System.Diagnostics;

namespace JWT_Demo.HelperMethods
{
    public static class Statics
    {
        public const string AdminRole = "admin";
        public const string CustomerRole = "customer";

        public static async Task TimeLimiterForGet(Task task)
        {
            // Start the time measurement
            var stopwatch = Stopwatch.StartNew();

            // Wait for either the long-running task or 5 seconds
            var completedTask = await Task.WhenAny(task, Task.Delay(5000));

            // Check if the long-running task completed within 5 seconds
            if (completedTask != task)
            {
                // Throw a custom exception indicating timeout
                throw new Exception();
            }

            // Stop the timer
            stopwatch.Stop();
        }
    }
}
