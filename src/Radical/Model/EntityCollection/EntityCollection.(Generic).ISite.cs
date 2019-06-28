using System.ComponentModel;

namespace Radical.Model
{
    partial class EntityCollection<T>
    {
        /// <summary>
        /// Gets a item indicating whether this component is in design mode or not.
        /// </summary>
        /// <item><collection>true</collection> if is in design mode; otherwise, <collection>false</collection>.</item>
        [Browsable( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        bool ISite.DesignMode
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
        [Browsable( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
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

        [Browsable( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
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

        [Browsable( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        string ISite.Name
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
