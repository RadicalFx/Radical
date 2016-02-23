using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Topics.Radical.Diagnostics
{
    public static class TraceSourceExtensions
    {
        public static void Information( this TraceSource source, Int32 eventId, String message )
        {
            source.TraceEvent( TraceEventType.Information, eventId, message );
        }

        public static void Information( this TraceSource source, Int32 eventId, String format, params Object[] args )
        {
            source.TraceEvent( TraceEventType.Information, eventId, format, args );
        }

        public static void Information( this TraceSource source, String message )
        {
            source.TraceEvent( TraceEventType.Information, 0, message );
        }

        public static void Information( this TraceSource source, String format, params Object[] args )
        {
            source.TraceEvent( TraceEventType.Information, 0, format, args );
        }

        [Obsolete]
        public static void Verbose( this TraceSource source, Int32 eventId, String message )
        {
            source.TraceEvent( TraceEventType.Verbose, eventId, message );
        }

        [Obsolete]
        public static void Verbose( this TraceSource source, Int32 eventId, String format, params Object[] args )
        {
            source.TraceEvent( TraceEventType.Verbose, eventId, format, args );
        }

        [Obsolete]
        public static void Verbose( this TraceSource source, String message )
        {
            source.TraceEvent( TraceEventType.Verbose, 0, message );
        }

        [Obsolete]
        public static void Verbose( this TraceSource source, String format, params Object[] args )
        {
            source.TraceEvent( TraceEventType.Verbose, 0, format, args );
        }

        [Obsolete]
        public static void Verbose( this TraceSource source, String format, Func<Object[]> args )
        {
            if( source.Switch.ShouldTrace( TraceEventType.Error ) )
            {
                source.TraceEvent( TraceEventType.Verbose, 0, format, args() );
            }
        }

        public static void Debug( this TraceSource source, Int32 eventId, String message )
        {
            source.TraceEvent( TraceEventType.Verbose, eventId, message );
        }

        public static void Debug( this TraceSource source, Int32 eventId, String format, params Object[] args )
        {
            source.TraceEvent( TraceEventType.Verbose, eventId, format, args );
        }

        public static void Debug( this TraceSource source, String message )
        {
            source.TraceEvent( TraceEventType.Verbose, 0, message );
        }

        public static void Debug( this TraceSource source, String format, params Object[] args )
        {
            source.TraceEvent( TraceEventType.Verbose, 0, format, args );
        }

        public static void Debug( this TraceSource source, String format, Func<Object[]> args )
        {
            if ( source.Switch.ShouldTrace( TraceEventType.Error ) )
            {
                source.TraceEvent( TraceEventType.Verbose, 0, format, args() );
            }
        }

        public static void Warning( this TraceSource source, String message )
        {
            source.TraceEvent( TraceEventType.Warning, 0, message );
        }

        public static void Warning( this TraceSource source, String format, params Object[] args )
        {
            source.TraceEvent( TraceEventType.Warning, 0, format, args );
        }

        public static void Error( this TraceSource source, String message )
        {
            source.TraceEvent( TraceEventType.Error, 0, message );
        }

        public static void Error( this TraceSource source, String format, params Object[] args )
        {
            source.TraceEvent( TraceEventType.Error, 0, format, args );
        }

        public static void Error( this TraceSource source, String message, Exception e )
        {
            TraceSourceExtensions.Error( source, message, e, false );
        }

        public static void Error( this TraceSource source, String message, Exception e, Boolean dumpException )
        {
            if( source.Switch.ShouldTrace( TraceEventType.Error ) )
            {
                String evt = null;
                if( dumpException )
                {
                    try
                    {
                        var dump = ObjectDumper.Dump( e );
                        evt = String.Format( "{0}{1}{2}", message, Environment.NewLine, dump );
                    }
                    catch 
                    {
                        evt = String.Format( "Error with Exception dump, reverting.{1}{0}{1}{2}{1}{3}", message, Environment.NewLine, e.Message, e.StackTrace );
                    }
                }
                else
                {
                    evt = String.Format( "{0}{1}{2}{1}{3}", message, Environment.NewLine, e.Message, e.StackTrace );
                }

                source.TraceEvent( TraceEventType.Error, 0, evt );
            }
        }
    }
}
