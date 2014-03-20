using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Topics.Radical.Linq;
using Topics.Radical.Model;
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
		System.Windows.DependencyObject IViewModel.View { get; set; }

		IValidationService _validationService;

		/// <summary>
		/// Gets the validation service.
		/// </summary>
		/// <value>The validation service.</value>
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
					};

					this._validationService.Resetted += ( s, e ) =>
					{
						this.ValidationErrors.Clear();
						this.GetType()
							.GetProperties()
							.Select( p => p.Name )
							.ForEach( p => this.OnPropertyChanged( p ) );
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
		}

		//protected virtual void OnLoading()
		//{

		//}

		//protected virtual void OnLoaded()
		//{

		//}

		///// <summary>
		///// Gets or sets a value indicating whether this instance is loaded.
		///// </summary>
		///// <value><c>true</c> if this instance is loaded; otherwise, <c>false</c>.</value>
		//protected Boolean IsLoaded
		//{
		//    get;
		//    private set;
		//}

		/// <summary>
		/// Signals the object that initialization is starting.
		/// </summary>
		void ISupportInitialize.BeginInit()
		{
			//this.OnLoading();
		}

		/// <summary>
		/// Signals the object that initialization is complete.
		/// </summary>
		void ISupportInitialize.EndInit()
		{
			//this.OnLoaded();

			//this.IsLoaded = true;
		}

		/// <summary>
		/// Gets the error.
		/// </summary>
		/// <value>The error.</value>
		/// <remarks>Used only in order to satisfy IDataErrorInfo interface implementation, the default implementation always returns null.</remarks>
		public virtual String Error
		{
			get { return null; }
		}

		/// <summary>
		/// Gets the error message, if any, for the property with the given name.
		/// </summary>
		public virtual String this[ String propertyName ]
		{
			get
			{
				var wasValid = this.IsValid;

				var error = this.ValidationService.Validate( propertyName );

				if( this.IsValid != wasValid )
				{
					this.OnPropertyChanged( () => this.IsValid );
				}

				this.OnValidated();

				return error;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is valid.
		/// </summary>
		/// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
		public virtual Boolean IsValid
		{
			get { return this.ValidationService.IsValid; }
		}

		/// <summary>
		/// Gets the validation errors if any.
		/// </summary>
		/// <value>The validation errors.</value>
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
			return this.Validate(null, ValidationBehavior.Default );
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
			this.ValidationService.ValidateRuleSet( ruleSet );
			this.OnValidated();

			if( behavior == ValidationBehavior.TriggerValidationErrorsOnFailure && !this.ValidationService.IsValid )
			{
				this.TriggerValidation();
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