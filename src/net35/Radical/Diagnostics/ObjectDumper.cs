using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Diagnostics;

namespace Topics.Radical.Diagnostics
{
    /// <summary>
    /// Dumps values of an object graph for debuggin/diagnostics purposes.
    /// </summary>
    public class ObjectDumper : IDisposable
    {
        /// <summary>
        /// Dumps values of the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>A string representing the object dump.</returns>
        public static String Dump( Object target )
        {
            return ObjectDumper.Dump( target, Int32.MaxValue );
        }

        /// <summary>
        /// Dumps values of the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="depth">The depth of the dump.</param>
        /// <returns>A string representing the object dump.</returns>
        public static String Dump( object target, int depth )
        {
            try
            {
                using( var dumper = new ObjectDumper( depth ) )
                {
                    dumper.WriteObject( null, target );

                    return dumper
                        .ToString()
                        .TrimEnd( Environment.NewLine.ToCharArray() );
                }
            }
            catch( Exception ex )
            {
                return String.Format( "Dump failure: {0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace );
            }
        }


        public void Dispose()
        {
            this.visitedReferences.Clear();
            this.visitedReferences = null;
            this.builder = null;
        }

        private HashSet<Object> visitedReferences = new HashSet<Object>();
        private StringBuilder builder = new StringBuilder();

        //int pos;
        int level;
        int maxDepth;

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.builder.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDumper"/> class.
        /// </summary>
        /// <param name="depth">The depth.</param>
        private ObjectDumper( int depth )
        {
            this.maxDepth = depth;
        }

        void Write( string s )
        {
            if( s != null )
            {
                Debug.Write( s );
                this.builder.Append( s );
                //this.pos += s.Length;
            }
        }

        void WriteLine( string s )
        {
            if( s != null )
            {
                Debug.WriteLine( s );
                this.builder.AppendLine( s );
                //this.pos += s.Length;
            }
        }

        void WriteIndent()
        {
            this.builder.Append( ' ', level * 3 );
        }

        void WriteLine()
        {
            Debug.WriteLine( "" );
            this.builder.AppendLine();
            //this.pos = 0;
        }

        void WriteTab()
        {
            this.Write( "\t" );
        }

        private void WriteObject( string prefix, object target )
        {
            try
            {
                if( target != null && !target.GetType().IsValueType && !( target is string ) )
                {
                    if( this.visitedReferences.Contains( target ) )
                    {
                        return;
                    }

                    this.visitedReferences.Add( target );
                }

                if( target == null || target.GetType().IsPrimitive || target is string )
                {
                    this.WriteIndent();
                    this.Write( prefix );
                    this.WriteValue( target );
                    this.WriteLine();
                }
                else if( target is IEnumerable )
                {
                    foreach( object element in ( IEnumerable )target )
                    {
                        if( element is IEnumerable && !( element is string ) )
                        {
                            this.WriteIndent();
                            this.Write( prefix );
                            this.Write( "[...]" );
                            this.WriteLine();
                            if( this.level < this.maxDepth )
                            {
                                this.level++;
                                this.WriteObject( prefix, element );
                                this.level--;
                            }
                        }
                        else
                        {
                            this.WriteObject( prefix, element );
                        }
                    }
                }
                else
                {
                    var type = target.GetType();
                    //this.WriteIndent();
                    this.Write( prefix );
                    this.Write( String.Format( "Type: {0} ({1})", type.Name, type ) );
                    this.WriteLine();

                    if( target is Assembly )
                    {
                        var a = ( Assembly )target;
                        this.WriteIndent();
                        this.Write( "FullName: " + a.FullName );
                    }
                    else if( target is RuntimeTypeHandle || target is RuntimeMethodHandle || target is RuntimeArgumentHandle || target is RuntimeFieldHandle )
                    {
                        //this.WriteIndent();
                        //this.Write( "<RuntimeTypeHandle>" );
                    }
                    else
                    {
                        var members = type.GetMembers( BindingFlags.Public | BindingFlags.Instance );
                        //this.WriteIndent();
                        //this.Write( prefix );

                        //bool propWritten = false;
                        //foreach( var m in members )
                        //{
                        //    var f = m as FieldInfo;
                        //    var p = m as PropertyInfo;
                        //    if( f != null || p != null )
                        //    {
                        //        propWritten = true;

                        //        //if( propWritten )
                        //        //{
                        //        //    this.WriteLine();
                        //        //    //this.WriteTab();
                        //        //}
                        //        //else
                        //        //{
                        //        //    propWritten = true;
                        //        //}

                        //        this.Write( m.Name );
                        //        this.Write( "=" );

                        //        //var t = f != null ? f.FieldType : p.PropertyType;
                        //        var value = f != null ? f.GetValue( target ) : p.GetValue( target, null );
                        //        this.WriteObject( prefix, value );

                        //        //if( t.IsValueType || t == typeof( string ) )
                        //        //{
                        //        //    this.WriteValue( f != null ? f.GetValue( target ) : p.GetValue( target, null ) );
                        //        //}
                        //        //else
                        //        //{
                        //        //    //if( typeof( IEnumerable ).IsAssignableFrom( t ) )
                        //        //    //{
                        //        //    //    this.Write( "..." );
                        //        //    //}
                        //        //    //else
                        //        //    //{
                        //        //    //    this.Write( "{ }" );
                        //        //    //}
                        //        //}

                        //        this.WriteLine();
                        //    }
                        //}

                        //if( propWritten )
                        //{
                        //    this.WriteLine();
                        //}

                        if( this.level < this.maxDepth )
                        {
                            foreach( MemberInfo m in members )
                            {
                                var f = m as FieldInfo;
                                var p = m as PropertyInfo;
                                if( f != null || p != null )
                                {
                                    this.level++;
                                    try
                                    {
                                        object value = f != null ? f.GetValue( target ) : p.GetValue( target, null );
                                        this.WriteObject( m.Name + ": ", value );
                                    }
                                    catch( TargetInvocationException )
                                    {
                                        this.Write( String.Format( "{0}: <Cannot get value>.", m.Name ) );
                                        this.WriteLine();
                                    }

                                    this.level--;

                                    //Type t = f != null ? f.FieldType : p.PropertyType;
                                    //if( !( t.IsValueType || t == typeof( string ) ) )
                                    //{
                                    //    object value = f != null ? f.GetValue( target ) : p.GetValue( target, null );
                                    //    if( value != null )
                                    //    {
                                    //        this.level++;
                                    //        this.WriteObject( m.Name + ": ", value );
                                    //        this.level--;
                                    //    }
                                    //}
                                }
                            }
                        }
                    }
                }
            }
            catch( Exception ex )
            {
                throw;
            }
        }

        private void WriteValue( object value )
        {
            if( value == null )
            {
                this.Write( "<null>" );
            }
            else if( value is DateTime )
            {
                this.Write( ( ( DateTime )value ).ToShortDateString() );
            }
            else if( value is ValueType || value is string )
            {
                this.Write( value.ToString() );
            }
            //else if( value is IEnumerable )
            //{
            //    this.Write( "<...>" );
            //}
            else
            {
                this.Write( "{ }" );
            }
        }
    }
}
