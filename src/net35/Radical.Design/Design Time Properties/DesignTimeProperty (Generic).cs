using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Topics.Radical.Linq;
using Topics.Radical.Reflection;
using System.Diagnostics;

namespace Topics.Radical.Design
{
    abstract class DesignTimeProperty<T, TValue> : DesignTimeProperty
    {
        protected readonly DesignTimeHost<T> host;

        protected DesignTimeProperty( DesignTimeHost<T> host, DesignTimeProperty property )
            : base( typeof( T ), property )
        {
            this.host = host;
        }

        protected DesignTimeProperty( DesignTimeHost<T> host, String propertyName )
            : base( typeof( T ), propertyName )
        {
            this.host = host;
        }

        public override Type PropertyType
        {
            get
            {
                Debug.WriteLine
                ( 
                    String.Format( "Returning typeof( {0} ) from {1} named {2}",
                        typeof( TValue ).ToShortNameString(),
                        this.GetType().ToShortNameString(),
                        this.Name ) 
                );

                return typeof( TValue );
            }
        }
    }
}