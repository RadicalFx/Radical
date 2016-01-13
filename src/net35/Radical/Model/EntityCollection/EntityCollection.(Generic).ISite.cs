using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Topics.Radical.ComponentModel;
using Topics.Radical.Linq;
using Topics.Radical.Validation;
using System.Linq.Expressions;

namespace Topics.Radical.Model
{
    partial class EntityCollection<T>
    {
        /// <summary>
        /// Gets a item indicating whether this component is in design mode or not.
        /// </summary>
        /// <item><collection>true</collection> if is in design mode; otherwise, <collection>false</collection>.</item>
#if !SILVERLIGHT
        [Browsable( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
#endif
        Boolean ISite.DesignMode
        {
            get
            {
                this.EnsureNotDisposed();
                if( this.site != null )
                {
                    return this.site.DesignMode;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <item>The container.</item>
#if !SILVERLIGHT
        [Browsable( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
#endif
        IContainer ISite.Container
        {
            get
            {
                this.EnsureNotDisposed();
                if( this.site != null )
                {
                    return this.site.Container;
                }
                return null;
            }
        }

#if !SILVERLIGHT
        [Browsable( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
#endif
        IComponent ISite.Component
        {
            get
            {
                this.EnsureNotDisposed();
                if( this.site != null )
                {
                    return this.site.Component;
                }
                return null;
            }
        }

#if !SILVERLIGHT
        [Browsable( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
#endif
        String ISite.Name
        {
            get
            {
                this.EnsureNotDisposed();
                if( this.site != null )
                {
                    return this.site.Name;
                }
                return null;
            }
            set
            {
                this.EnsureNotDisposed();
                if( this.site != null )
                {
                    this.site.Name = value;
                }
            }
        }
    }
}
