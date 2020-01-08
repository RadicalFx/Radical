//extern alias tpx;

namespace Radical.Tests.ChangeTracking
{
    using Radical.ComponentModel.ChangeTracking;
    using Radical.Model;
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

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

            OnDisposed();
        }

        #region IComponent Members

        /*
         * Non possiamo usare la EventHaldlerList
         * perchè in fase di dispose ce la perdiamo...
         */
        public event EventHandler Disposed;

        protected virtual void OnDisposed()
        {
            if (Disposed != null)
            {
                Disposed(this, EventArgs.Empty);
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
            nameRejectCallback = (pcr) =>
            {
                CacheChangeOnRejectCallback("property-name", Name, nameRejectCallback, null, pcr);
                _name = pcr.CachedValue;
            };
        }

        readonly TransientRegistration transientRegistration = TransientRegistration.AsTransparent;

        public Person(IChangeTrackingService memento, ChangeTrackingRegistration registration, TransientRegistration transientRegistration)
            : base(memento, registration)
        {
            this.transientRegistration = transientRegistration;

            nameRejectCallback = (pcr) =>
            {
                CacheChangeOnRejectCallback("property-name", Name, nameRejectCallback, null, pcr);
                _name = pcr.CachedValue;
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
            get { return _name; }
            set
            {
                if (value != Name)
                {
                    CacheChange("property-name", Name, nameRejectCallback);
                    _name = value;
                }
            }
        }

        public string FirstName
        {
            get { return GetPropertyValue(() => FirstName); }
            set { SetPropertyValue(() => FirstName, value); }
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
