using System;
using System.Diagnostics;

namespace Radical.Diagnostics
{
    /// <summary>
    /// Provides extension methods for <see cref="TraceSource"/> to simplify tracing at common severity levels.
    /// </summary>
    public static class TraceSourceExtensions
    {
        /// <summary>
        /// Writes an informational trace event with the specified event identifier and message.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="eventId">A numeric identifier for the event.</param>
        /// <param name="message">The trace message.</param>
        public static void Information(this TraceSource source, int eventId, string message)
        {
            source.TraceEvent(TraceEventType.Information, eventId, message);
        }

        /// <summary>
        /// Writes an informational trace event with the specified event identifier, format string, and arguments.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="eventId">A numeric identifier for the event.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to format.</param>
        public static void Information(this TraceSource source, int eventId, string format, params object[] args)
        {
            source.TraceEvent(TraceEventType.Information, eventId, format, args);
        }

        /// <summary>
        /// Writes an informational trace event with the specified message.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="message">The trace message.</param>
        public static void Information(this TraceSource source, string message)
        {
            source.TraceEvent(TraceEventType.Information, 0, message);
        }

        /// <summary>
        /// Writes an informational trace event with the specified format string and arguments.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to format.</param>
        public static void Information(this TraceSource source, string format, params object[] args)
        {
            source.TraceEvent(TraceEventType.Information, 0, format, args);
        }

        /// <summary>
        /// Writes a verbose (debug) trace event with the specified event identifier and message.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="eventId">A numeric identifier for the event.</param>
        /// <param name="message">The trace message.</param>
        public static void Debug(this TraceSource source, int eventId, string message)
        {
            source.TraceEvent(TraceEventType.Verbose, eventId, message);
        }

        /// <summary>
        /// Writes a verbose (debug) trace event with the specified event identifier, format string, and arguments.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="eventId">A numeric identifier for the event.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to format.</param>
        public static void Debug(this TraceSource source, int eventId, string format, params object[] args)
        {
            source.TraceEvent(TraceEventType.Verbose, eventId, format, args);
        }

        /// <summary>
        /// Writes a verbose (debug) trace event with the specified message.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="message">The trace message.</param>
        public static void Debug(this TraceSource source, string message)
        {
            source.TraceEvent(TraceEventType.Verbose, 0, message);
        }

        /// <summary>
        /// Writes a verbose (debug) trace event with the specified format string and arguments.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to format.</param>
        public static void Debug(this TraceSource source, string format, params object[] args)
        {
            source.TraceEvent(TraceEventType.Verbose, 0, format, args);
        }

        /// <summary>
        /// Writes a verbose (debug) trace event using a deferred argument factory, only evaluating arguments
        /// when the trace source is configured to emit error-level events or above.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">A factory function that produces the format arguments.</param>
        public static void Debug(this TraceSource source, string format, Func<object[]> args)
        {
            if (source.Switch.ShouldTrace(TraceEventType.Error))
            {
                source.TraceEvent(TraceEventType.Verbose, 0, format, args());
            }
        }

        /// <summary>
        /// Writes a warning trace event with the specified message.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="message">The trace message.</param>
        public static void Warning(this TraceSource source, string message)
        {
            source.TraceEvent(TraceEventType.Warning, 0, message);
        }

        /// <summary>
        /// Writes a warning trace event with the specified format string and arguments.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to format.</param>
        public static void Warning(this TraceSource source, string format, params object[] args)
        {
            source.TraceEvent(TraceEventType.Warning, 0, format, args);
        }

        /// <summary>
        /// Writes an error trace event with the specified message.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="message">The trace message.</param>
        public static void Error(this TraceSource source, string message)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            source.TraceEvent(TraceEventType.Error, 0, message);
        }

        /// <summary>
        /// Writes an error trace event with the specified format string and arguments.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to format.</param>
        public static void Error(this TraceSource source, string format, params object[] args)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            source.TraceEvent(TraceEventType.Error, 0, format, args);
        }

        /// <summary>
        /// Writes an error trace event that includes the exception message and stack trace.
        /// </summary>
        /// <param name="source">The <see cref="TraceSource"/> to write to.</param>
        /// <param name="message">The trace message.</param>
        /// <param name="e">The exception to include in the trace output.</param>
        public static void Error(this TraceSource source, string message, Exception e)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (source.Switch.ShouldTrace(TraceEventType.Error))
            {
                var evt = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}{2}{1}{3}", message, Environment.NewLine, e.Message, e.StackTrace);

                source.TraceEvent(TraceEventType.Error, 0, evt);
            }
        }
    }
}
