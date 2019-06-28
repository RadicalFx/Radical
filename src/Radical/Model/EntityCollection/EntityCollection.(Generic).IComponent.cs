using System;
using System.ComponentModel;

namespace Radical.Model
{
    partial class EntityCollection<T>
    {

        /// <summary>
        /// Represents the method that handles the <see cref="E:System.ComponentModel.IComponent.Disposed"/> event of a component.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Called to raise the Disposed event.
        /// </summary>
        protected virtual void OnDisposed()
        {
            if( this.Disposed != null )
            {
                this.Disposed( this, EventArgs.Empty );
            }
        }

        private ISite site;

        /// <summary>
        /// Gets or sets the <see cref="T:System.ComponentModel.ISite"/> associated with the <see cref="T:System.ComponentModel.IComponent"/>.
        /// </summary>
        /// <item></item>
        /// <returns>The <see cref="T:System.ComponentModel.ISite"/> object associated with the component; or null, if the component does not have a site.</returns>
        [Browsable( false )]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        ISite IComponent.Site
        {
            get
            {
                this.EnsureNotDisposed();
                return this.site;
            }
            set
            {
                this.EnsureNotDisposed();
                this.site = value;
            }
        }
    }
}