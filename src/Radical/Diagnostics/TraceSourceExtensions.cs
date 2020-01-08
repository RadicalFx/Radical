using System;
using System.Diagnostics;

namespace Radical.Diagnostics
{
    public static class TraceSourceExtensions
    {
        public static void Information(this TraceSource source, int eventId, string message)
        {
            source.TraceEvent(TraceEventType.Information, eventId, message);
        }

        public static void Information(this TraceSource source, int eventId, string format, params object[] args)
        {
            source.TraceEvent(TraceEventType.Information, eventId, format, args);
        }

        public static void Information(this TraceSource source, string message)
        {
            source.TraceEvent(TraceEventType.Information, 0, message);
        }

        public static void Information(this TraceSource source, string format, params object[] args)
        {
            source.TraceEvent(TraceEventType.Information, 0, format, args);
        }

        public static void Debug(this TraceSource source, int eventId, string message)
        {
            source.TraceEvent(TraceEventType.Verbose, eventId, message);
        }

        public static void Debug(this TraceSource source, int eventId, string format, params object[] args)
        {
            source.TraceEvent(TraceEventType.Verbose, eventId, format, args);
        }

        public static void Debug(this TraceSource source, string message)
        {
            source.TraceEvent(TraceEventType.Verbose, 0, message);
        }

        public static void Debug(this TraceSource source, string format, params object[] args)
        {
            source.TraceEvent(TraceEventType.Verbose, 0, format, args);
        }

        public static void Debug(this TraceSource source, string format, Func<object[]> args)
        {
            if (source.Switch.ShouldTrace(TraceEventType.Error))
            {
                source.TraceEvent(TraceEventType.Verbose, 0, format, args());
            }
        }

        public static void Warning(this TraceSource source, string message)
        {
            source.TraceEvent(TraceEventType.Warning, 0, message);
        }

        public static void Warning(this TraceSource source, string format, params object[] args)
        {
            source.TraceEvent(TraceEventType.Warning, 0, format, args);
        }

        public static void Error(this TraceSource source, string message)
        {
            source.TraceEvent(TraceEventType.Error, 0, message);
        }

        public static void Error(this TraceSource source, string format, params object[] args)
        {
            source.TraceEvent(TraceEventType.Error, 0, format, args);
        }
        
        public static void Error(this TraceSource source, string message, Exception e)
        {
            if (source.Switch.ShouldTrace(TraceEventType.Error))
            {
                var evt = string.Format("{0}{1}{2}{1}{3}", message, Environment.NewLine, e.Message, e.StackTrace);

                source.TraceEvent(TraceEventType.Error, 0, evt);
            }
        }
    }
}
