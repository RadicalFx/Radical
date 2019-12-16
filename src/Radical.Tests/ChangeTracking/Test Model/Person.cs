//extern alias tpx;

namespace Radical.Tests.ChangeTracking
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using Radical.ComponentModel.ChangeTracking;
    using Radical.Model;

    class Person : MementoEntity, IComponent
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                //if( this.Site != null && this.Site.Container != null )
                //{
                //    this.Site.Container.Remove( this );
                //}
            }

            this.OnDisposed();
        }

        #region IComponent Members

        /*
         * Non possiamo usare la EventHaldlerList
         * perchè in fase di dispose ce la perdiamo...
         */
        public event EventHandler Disposed;

        protected virtual void OnDisposed()
        {
            if (this.Disposed != null)
            {
                this.Disposed(this, EventArgs.Empty);
            }
        }

        public ISite Site
        {
            get;
            set;
        }

        #endregion

        public Person(IChangeTrackingService memento)
            : this(memento, true)
        {

        }

        public Person(IChangeTrackingService memento, bool registerAsTransient)
            : base(memento, registerAsTransient)
        {
            this.nameRejectCallback = (pcr) =>
            {
                this.CacheChangeOnRejectCallback("property-name", this.Name, nameRejectCallback, null, pcr);
                this._name = pcr.CachedValue;
            };
        }

        TransientRegistration transientRegistration = TransientRegistration.AsTransparent;

        public Person(IChangeTrackingService memento, ChangeTrackingRegistration registration, TransientRegistration transientRegistration)
            : base(memento, registration)
        {
            this.transientRegistration = transientRegistration;

            this.nameRejectCallback = (pcr) =>
            {
                this.CacheChangeOnRejectCallback("property-name", this.Name, nameRejectCallback, null, pcr);
                this._name = pcr.CachedValue;
            };
        }

        protected override void OnRegisterTransient(TransientRegistration transientRegistration)
        {
            base.OnRegisterTransient(this.transientRegistration);
        }

        private readonly RejectCallback<string> nameRejectCallback = null;
        private string _name = string.Empty;

        public string Name
        {
            get { return this._name; }
            set
            {
                if (value != this.Name)
                {
                    this.CacheChange("property-name", this.Name, nameRejectCallback);
                    this._name = value;
                }
            }
        }

        public string FirstName
        {
            get { return this.GetPropertyValue(() => this.FirstName); }
            set { this.SetPropertyValue(() => this.FirstName, value); }
        }

        public void SetInitialPropertyValueForTest<T>(string propertyName, T value)
        {
            base.SetInitialPropertyValue<T>(propertyName, value);
        }

        public void SetInitialPropertyValueForTest<T>(Expression<Func<T>> property, T value)
        {
            base.SetInitialPropertyValue<T>(property, value);
        }
    }
}
