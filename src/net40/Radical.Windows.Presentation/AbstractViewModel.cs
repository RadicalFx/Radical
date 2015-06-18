using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Topics.Radical.Linq;
using Topics.Radical.Model;
using Topics.Radical.Reflection;
using Topics.Radical.Validation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Services.Validation;

namespace Topics.Radical.Windows.Presentation
{
    /// <summary>
    /// A base abstract ViewModel with builtin support for validation, error notification.
    /// </summary>
    public abstract class AbstractViewModel :
        Entity,
        IViewModel,
        ISupportInitialize
    {
        /// <summary>
        /// Gets or sets the view. The view property is intended only for
        /// infrastructural purpose. It is required to hold the one-to-one
        /// relation beteewn the view and the view model.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        [Bindable( false )]
        [SkipPropertyValidation]
        System.Windows.DependencyObject IViewModel.View { get; set; }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnPropertyChanged( PropertyChangedEventArgs e )
        {
            if( this.IsValidationEnabled
                && this.RunValidationOnPropertyChanged
                && !this.IsResettingValidation
                && !this.IsTriggeringValidation
                && !this.SkipPropertyValidation( e.PropertyName )
                && !this.validationState.IsValidatingProperty( e.PropertyName ) )
            {
                this.ValidateProperty( e.PropertyName );
            }

            base.OnPropertyChanged( e );
        }

        /// <summary>
        /// Determines if property validation should be skipped for the given property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        protected virtual Boolean SkipPropertyValidation( String propertyName )
        {
            var pi = this.GetType().GetProperty( propertyName );
            if( pi == null )
            {
                return true;
            }

            if( pi != null )
            {
                return pi.IsAttributeDefined<SkipPropertyValidationAttribute>();
            }

            return false;
        }

        /// <summary>
        /// Gets a value indication if validation is enabled or not.
        /// </summary>
        [SkipPropertyValidation]
        protected virtual Boolean IsValidationEnabled
        {
            get
            {
#if FX45
                return this is IDataErrorInfo
                    || this is ICanBeValidated
                    || this is INotifyDataErrorInfo
                    || this is IRequireValidation;
#else
                return this is IDataErrorInfo
                    || this is ICanBeValidated;
#endif
            }
        }

        IValidationService _validationService;

        /// <summary>
        /// Gets the validation service.
        /// </summary>
        /// <value>The validation service.</value>
        [SkipPropertyValidation]
        protected IValidationService ValidationService
        {
            get
            {
                if( this._validationService == null )
                {
                    this._validationService = this.GetValidationService();
                    this._validationService.StatusChanged += ( s, e ) =>
                    {
                        this.ValidationErrors.Clear();
                        foreach( var error in this._validationService.ValidationErrors )
                        {
                            this.ValidationErrors.Add( error );
                        }

#if FX45
                        this.OnErrorsChanged( null );
                        this.OnPropertyChanged( () => this.HasErrors );
#endif
                    };

                    this._validationService.ValidationReset += ( s, e ) =>
                    {
                        var shouldSetStatus = !this.IsResettingValidation;
                        if( shouldSetStatus )
                        {
                            this.IsResettingValidation = true;
                        }

                        this.ValidationErrors.Clear();
                        this.GetType()
                            .GetProperties()
                            .Where( p => !SkipPropertyValidation( p.Name ) )
                            .Select( p => p.Name )
                            .ForEach( p => this.OnPropertyChanged( p ) );

#if FX45
                        this.OnErrorsChanged( null );
                        this.OnPropertyChanged( () => this.HasErrors );
#endif

                        if( shouldSetStatus )
                        {
                            this.IsResettingValidation = false;
                        }
                    };
                }

                return this._validationService;
            }
        }

        /// <summary>
        /// Gets the validation service, this method is called once the first time
        /// the validation service is accessed, inheritors should override this method
        /// in order to provide a <see cref="IValidationService"/> implementation.
        /// </summary>
        /// <returns>The validation service to use to validate this view model.</returns>
        protected virtual IValidationService GetValidationService()
        {
            return NullValidationService.Instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractViewModel"/> class.
        /// </summary>
        protected AbstractViewModel()
        {
            this.ValidationErrors = new ObservableCollection<ValidationError>();
            this.RunValidationOnPropertyChanged = true;
        }

        /// <summary>
        /// Signals the object that initialization is starting.
        /// </summary>
        void ISupportInitialize.BeginInit()
        {

        }

        /// <summary>
        /// Signals the object that initialization is complete.
        /// </summary>
        void ISupportInitialize.EndInit()
        {

        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>The error.</value>
        /// <remarks>Used only in order to satisfy IDataErrorInfo interface implementation, the default implementation always returns null.</remarks>
        [Bindable( false ), SkipPropertyValidation]
        public virtual String Error
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the error message, if any, for the property with the given name.
        /// </summary>
        [Bindable( false ), SkipPropertyValidation]
        public virtual String this[ String propertyName ]
        {
            get
            {
                var error = this.ValidationErrors
                    .Where( e => e.Key == propertyName )
                    .Select( err => err.ToString() )
                    .FirstOrDefault();

                return error;
            }
        }

        /// <summary>
        /// Validates the given property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// The first validation error, if any; Otherwise <c>null</c>.
        /// </returns>
        protected String ValidateProperty( String propertyName )
        {
            return this.ValidateProperty( propertyName, ValidationBehavior.Default );
        }

        PropertyValidationState validationState = new PropertyValidationState();

        /// <summary>
        /// Validates the given property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The first validation error, if any; Otherwise <c>null</c>.
        /// </returns>
        protected virtual String ValidateProperty( String propertyName, ValidationBehavior behavior )
        {
            String error = null;

            if( this.ValidationService.IsValidationSuspended )
            {
                return error;
            }

            using( this.validationState.BeginPropertyValidation( propertyName ) )
            {
                var wasValid = this.IsValid;

                var beforeDetectedProblems = this.ValidationService.ValidationErrors
                    .Where( ve => ve.Key == propertyName )
                    .SelectMany( ve => ve.DetectedProblems )
                    .OrderBy( dp => dp )
                    .ToArray();

                error = this.ValidationService.Validate( propertyName );

                var afterDetectedProblems = this.ValidationService.ValidationErrors
                    .Where( ve => ve.Key == propertyName )
                    .SelectMany( ve => ve.DetectedProblems )
                    .OrderBy( dp => dp )
                    .ToArray();

                var validationStatusChanged = !beforeDetectedProblems.SequenceEqual( afterDetectedProblems );
                if( validationStatusChanged && behavior == ValidationBehavior.TriggerValidationErrorsOnFailure )
                {
                    this.OnPropertyChanged( propertyName );

#if FX45
                    this.OnErrorsChanged( propertyName );
#endif
                }

                if( this.IsValid != wasValid )
                {
                    this.OnPropertyChanged( () => this.IsValid );
#if FX45
                    this.OnPropertyChanged( () => this.HasErrors );
#endif
                }
            }

            this.OnValidated();

            return error;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        [Bindable( false ), SkipPropertyValidation]
        public virtual Boolean IsValid
        {
            get { return this.ValidationService.IsValid; }
        }

        /// <summary>
        /// Gets the validation errors if any.
        /// </summary>
        /// <value>The validation errors.</value>
        [Bindable( false ), SkipPropertyValidation]
        public virtual ObservableCollection<ValidationError> ValidationErrors
        {
            get;
            private set;
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns><c>True</c> if this instance is valid; otherwise <c>false</c>.</returns>
        public Boolean Validate()
        {
            return this.Validate( null, ValidationBehavior.Default );
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        ///   <c>True</c> if this instance is valid; otherwise <c>false</c>.
        /// </returns>
        public Boolean Validate( ValidationBehavior behavior )
        {
            return this.Validate( null, behavior );
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <param name="ruleSet">The rule set.</param>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        ///   <c>True</c> if this instance is valid; otherwise <c>false</c>.
        /// </returns>
        public virtual Boolean Validate( String ruleSet, ValidationBehavior behavior )
        {
            if( this.ValidationService.IsValidationSuspended )
            {
                return this.ValidationService.IsValid;
            }

            var wasValid = this.IsValid;

            this.ValidationService.ValidateRuleSet( ruleSet );
            this.OnValidated();

            if( behavior == ValidationBehavior.TriggerValidationErrorsOnFailure && !this.ValidationService.IsValid )
            {
                this.TriggerValidation();
            }

            if( this.IsValid != wasValid )
            {
                this.OnPropertyChanged( () => this.IsValid );
#if FX45
                this.OnPropertyChanged( () => this.HasErrors );
#endif
            }

            return this.ValidationService.IsValid;
        }

        /// <summary>
        /// Occurs when the validation process terminates.
        /// </summary>
        public event EventHandler Validated;

        /// <summary>
        /// Raises the Validated event.
        /// </summary>
        protected virtual void OnValidated()
        {
            if( this.Validated != null )
            {
                this.Validated( this, EventArgs.Empty );
            }
        }

        /// <summary>
        /// Triggers the validation.
        /// </summary>
        public virtual void TriggerValidation()
        {
            if( !this.IsTriggeringValidation )
            {
                this.IsTriggeringValidation = true;

                foreach( var invalid in this.ValidationService.GetInvalidProperties() )
                {
                    this.OnPropertyChanged( invalid );
                }

                this.IsTriggeringValidation = false;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is triggering validation.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is triggering validation; otherwise, <c>false</c>.
        /// </value>
        [SkipPropertyValidation]
        protected virtual Boolean IsTriggeringValidation
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the focused element key.
        /// </summary>
        /// <value>
        /// The focused element key.
        /// </value>
        [Bindable( false ), SkipPropertyValidation]
        [MementoPropertyMetadata( TrackChanges = false )]
        public String FocusedElementKey
        {
            get { return this.GetPropertyValue( () => this.FocusedElementKey ); }
            set { this.SetPropertyValue( () => this.FocusedElementKey, value ); }
        }

        /// <summary>
        /// Moves the focus to.
        /// </summary>
        /// <param name="property">The property.</param>
        protected virtual void MoveFocusTo<T>( Expression<Func<T>> property )
        {
            this.EnsureNotDisposed();

            var propertyName = property.GetMemberName();
            this.MoveFocusTo( propertyName );
        }

        /// <summary>
        /// Moves the focus to.
        /// </summary>
        /// <param name="focusedElementKey">The focused element key.</param>
        protected virtual void MoveFocusTo( String focusedElementKey )
        {
            this.EnsureNotDisposed();

            this.FocusedElementKey = focusedElementKey;
        }

        /// <summary>
        /// Determines if each time a property changes the validation process should be run. The default value is <c>true</c>.
        /// </summary>
        [SkipPropertyValidation]
        protected Boolean RunValidationOnPropertyChanged { get; set; }

        /// <summary>
        /// <c>True</c> if the current ValidationService is resetting the validation status; Otherwise <c>false</c>.
        /// </summary>
        [SkipPropertyValidation]
        protected Boolean IsResettingValidation { get; private set; }

#if FX45

        /// <summary>
        /// Resets the validation status.
        /// </summary>
        public virtual void ResetValidation()
        {
            this.IsResettingValidation = true;
            this.ValidationService.Reset( ValidationResetBehavior.ErrorsOnly );
            this.IsResettingValidation = false;
        }

        /// <summary>
        /// Occurs when errors change.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Raises the ErrorsChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnErrorsChanged( String propertyName )
        {
            var h = this.ErrorsChanged;
            if( h != null )
            {
                h( this, new DataErrorsChangedEventArgs( propertyName ) );
            }
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public System.Collections.IEnumerable GetErrors( string propertyName )
        {
            if( String.IsNullOrEmpty( propertyName ) )
            {
                return this.ValidationErrors.ToArray();
            }

            var temp = this.ValidationErrors.Where( e => e.Key == propertyName ).ToArray();
            return temp;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        [Bindable( false )]
        [SkipPropertyValidation]
        public bool HasErrors
        {
            get
            {
                var hasErrors = !this.IsValid;
                return hasErrors;
            }
        }

#endif
    }

    //class MyColl<T> : ObservableCollection<T>
    //{
    //    class DefferedNotification : IDisposable
    //    {
    //        private MyColl<T> myColl;

    //        public DefferedNotification( MyColl<T> myColl )
    //        {
    //            this.myColl = myColl;
    //        }

    //        void IDisposable.Dispose()
    //        {
    //            myColl.OnCollectionChanged( new System.Collections.Specialized.NotifyCollectionChangedEventArgs( System.Collections.Specialized.NotifyCollectionChangedAction.Add, this.itemsQueue ) );
    //            foreach( var e in this.propertiesQueue )
    //            {
    //                myColl.OnPropertyChanged( e );
    //            }

    //            this.ClearQueues();
    //            this.IsDeferring = false;
    //        }

    //        System.Collections.Generic.List<T> itemsQueue = new System.Collections.Generic.List<T>();
    //        System.Collections.Generic.List<PropertyChangedEventArgs> propertiesQueue = new System.Collections.Generic.List<PropertyChangedEventArgs>();

    //        internal void AddToNotificationQueue( System.Collections.Generic.IEnumerable<T> range )
    //        {
    //            this.itemsQueue.AddRange( range );
    //        }

    //        internal void AddToNotificationQueue( PropertyChangedEventArgs e )
    //        {
    //            this.propertiesQueue.Add( e );
    //        }

    //        void ClearQueues()
    //        {
    //            this.itemsQueue.Clear();
    //            this.propertiesQueue.Clear();
    //        }

    //        internal Boolean IsDeferring { get; private set; }

    //        internal IDisposable StartDefer()
    //        {
    //            this.IsDeferring = true;
    //            return this;
    //        }
    //    }

    //    readonly DefferedNotification defferedNotification = null;

    //    public MyColl()
    //    {
    //        this.defferedNotification = new DefferedNotification( this );
    //    }

    //    public void AddRange( System.Collections.Generic.IEnumerable<T> range )
    //    {
    //        using( this.defferedNotification.StartDefer() )
    //        {
    //            foreach( var t in range )
    //            {
    //                this.Add( t );
    //            }

    //            this.defferedNotification.AddToNotificationQueue( range );
    //        }
    //    }

    //    protected override void OnPropertyChanged( PropertyChangedEventArgs e )
    //    {
    //        if( this.defferedNotification.IsDeferring )
    //        {
    //            this.defferedNotification.AddToNotificationQueue( e );
    //        }
    //        else
    //        {
    //            base.OnPropertyChanged( e );
    //        }
    //    }

    //    protected override void OnCollectionChanged( System.Collections.Specialized.NotifyCollectionChangedEventArgs e )
    //    {
    //        if( !this.defferedNotification.IsDeferring )
    //        {
    //            base.OnCollectionChanged( e );
    //        }
    //    }
    //}
}