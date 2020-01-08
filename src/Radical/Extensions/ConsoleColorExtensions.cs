using System;

namespace Radical
{
    /// <summary>
    /// Adds behaviors to the <see cref="ConsoleColor"/> enum.
    /// </summary>
    public static class ConsoleColorExtensions
    {
        /// <summary>
        /// Using the given color as the console foreground.
        /// </summary>
        /// <param name="color">The color to use.</param>
        /// <returns>A IDisposable instance to automatically revert the foreground color on dispose.</returns>
        public static IDisposable AsForegroundColor(this ConsoleColor color)
        {
            return new Colorizer(color);
        }

        class Colorizer : IDisposable
        {
            readonly ConsoleColor backup;

            public Colorizer(ConsoleColor newColor)
            {
                backup = Console.ForegroundColor;
                Console.ForegroundColor = newColor;
            }

            public void Dispose()
            {
                Console.ForegroundColor = backup;
            }
        }
    }
}
