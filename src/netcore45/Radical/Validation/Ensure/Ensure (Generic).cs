using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Topics.Radical.Validation
{
    /// <summary>
    /// Enusre is a simple, fluent based, engine usefull to validate
    /// methods and constructors parameters.
    /// </summary>
    /// <typeparam name="T">The type of the parameter to validate.</typeparam>
    public sealed class Ensure<T> : IConfigurableEnsure<T>, IEnsure<T>
    {
        private readonly T inspectedObject;
        readonly Ensure.SourceInfo si;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ensure&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="obj">The value of the parameter to validate.</param>
        /// <param name="si">The source info.</param>
        internal Ensure(T obj, Ensure.SourceInfo si)
        {
            this.inspectedObject = obj;
            this.si = si;
        }

        /// <summary>
        /// Gets the currently inspected object value.
        /// </summary>
        /// <returns>The currently inspected object value.</returns>
        public T GetValue()
        {
            return this.inspectedObject;
        }

        /// <summary>
        /// Gets the currently inspected object value castaed to specified type.
        /// </summary>
        /// <typeparam name="K">The type to cast the inspected object to, K must inherith from T.</typeparam>
        /// <returns>The currently inspected object value.</returns>
        public K GetValue<K>() where K : T
        {
            return (K)this.inspectedObject;
        }

        // *** prestazioni inaccettabili
        //
        //internal Ensure( Expression<Func<T>> obj )
        //{
        //    if( obj == null )
        //    {
        //        throw new ArgumentNullException( "obj", "Cannot use a null Expression<Func<T>> as Ensure ctor parameter." );
        //    }

        //    Func<T> func = obj.Compile();
        //    this.inspectedObject = func();

        //    var expression = obj.Body as MemberExpression;
        //    var member = expression.Member as FieldInfo;
        //    this.Name = member.Name;
        //}

        /// <summary>
        /// Identifies the name of the parameter that will be validated.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>The Ensure instance for fluent interface usage.</returns>
        public IEnsure<T> Named(String parameterName)
        {
            this._name = parameterName;
            return this;
        }

        /// <summary>
        /// Identifies the name of the parameter that will be validated.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>The Ensure instance for fluent interface usage.</returns>
        public IEnsure<T> Named(Expression<Func<T>> parameterName)
        {
            var expression = parameterName.Body as MemberExpression;
            if (expression == null)
            {
                throw new NotSupportedException("Only MemberExpression(s) are supported.");
            }

            this.nameExpression = parameterName;
            this._name = null;

            return this;
        }

        Expression<Func<T>> nameExpression;
        String _name;
        /// <summary>
        /// Gets or sets the name of the parameter to validate.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public String Name
        {
            get
            {
                if (String.IsNullOrEmpty(this._name) && this.nameExpression != null)
                {
                    var expression = (MemberExpression)this.nameExpression.Body;
                    this._name = expression.Member.Name;
                }

                return this._name;
            }
        }

        /// <summary>
        /// Specifies the custom user message to be used when raising exceptions.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>This ensure instance for fluent interface usage.</returns>
        public IEnsure<T> WithMessage(String errorMessage)
        {
            this.UserErrorMessage = errorMessage;
            return this;
        }

        /// <summary>
        /// Specifies the custom user message to be used when raising exceptions.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="formatArgs">The format arguments.</param>
        /// <returns>
        /// This ensure instance for fluent interface usage.
        /// </returns>
        public IEnsure<T> WithMessage(String errorMessage, params Object[] formatArgs)
        {
            this.UserErrorMessage = String.Format(errorMessage, formatArgs);
            return this;
        }

        /// <summary>
        /// Gets the user custom error message.
        /// </summary>
        /// <value>The error message.</value>
        public String UserErrorMessage
        {
            get;
            private set;
        }

        /*
         * {0} --> NewLine
         * {1} --> class name
         * {2} --> source type
         * {3} --> method name
         * {4} --> user error message (if any)
         * {5} --> <validatorSpecificMessage>
         * {6} --> parameter name (if any)
         */
        const String fullErrorMessageFormat = "Ensure validation failure.{0}{0}" +
                                              "   {1}{0}" +
                                              "   Name: '{2}'{0}" +
                                              "   Caller supplied informations: {3}{0}";

        /// <summary>
        /// Gets the full error message.
        /// </summary>
        /// <param name="validatorSpecificMessage">The validator specific message.</param>
        /// <returns>The error message.</returns>
        public String GetFullErrorMessage(String validatorSpecificMessage)
		{
			var fullErrorMessage = String.Format
			(
				fullErrorMessageFormat,
				Environment.NewLine,
                validatorSpecificMessage,
                this.Name ?? "<no-name-supplied>",
            	this.UserErrorMessage ?? "--"	
			);

			return fullErrorMessage;
		}

        /// <summary>
        /// Gets the full error message.
        /// </summary>
        /// <returns>The error message.</returns>
        public String GetFullErrorMessage()
        {
            return this.GetFullErrorMessage(String.Empty);
        }

        /// <summary>
        /// Gets the value of the validated parameter.
        /// </summary>
        /// <value>The value of the parameter.</value>
        public T Value
        {
            get { return this.inspectedObject; }
        }

        Boolean state = false;

        /// <summary>
        /// Execute the given predicate and saves the result for later usage.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate in order to establish if the operation resault is <c>true</c> or <c>false</c>.</param>
        /// <returns>The Ensure instance for fluent interface usage.</returns>
        public IEnsure<T> If(Predicate<T> predicate)
        {
            state = predicate(this.inspectedObject);
            return this;
        }

        /// <summary>
        /// Executes the specified action only if the <c>If</c> operation has been evaluated to <c>true</c>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>The Ensure instance for fluent interface usage.</returns>
        public IEnsure<T> Then(Action<T> action)
        {
            if (state)
            {
                action(this.inspectedObject);
            }

            return this;
        }

        /// <summary>
        /// Executes the specified action only if the <c>If</c> operation has been evaluated to <c>false</c>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        public IEnsure<T> Else(Action<T> action)
        {
            if (!state)
            {
                action(this.inspectedObject);
            }

            return this;
        }

        /// <summary>
        /// Executes the specified action only if the <c>If</c> operation has been evaluated to <c>true</c>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        public IEnsure<T> Then(Action<T, String> action)
        {
            if (state)
            {
                action(this.inspectedObject, this.Name);
            }

            return this;
        }

        /// <summary>
        /// Executes the specified action only if the <c>If</c> operation has been evaluated to <c>false</c>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        public IEnsure<T> Else(Action<T, String> action)
        {
            if (!state)
            {
                action(this.inspectedObject, this.Name);
            }

            return this;
        }

        /// <summary>
        /// Ensure that the supplied predicate returns true.
        /// </summary>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="ArgumentException">An ArgumentException is raised if the predicate result is false.</exception>
        public IEnsure<T> IsTrue(Predicate<T> func)
        {
            return this.If(func).Else(obj =>
            {
                this.Throw(new ArgumentException(this.Name, this.GetFullErrorMessage("The supplied condition is not met, condition was expected to be true.")));
            });
        }

        /// <summary>
        /// Ensure that the supplied predicate returns false.
        /// </summary>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="ArgumentException">An ArgumentException is raised if the predicate result is true.</exception>
        public IEnsure<T> IsFalse(Predicate<T> func)
        {
            return this.If(func).ThenThrow(v =>
            {
                return new ArgumentException(v.GetFullErrorMessage("The supplied condition is not met, condition was expected to be false."), v.Name);
            });
        }

        /// <summary>
        /// Ensure that the supplied object is equal to the currently inspected object.
        /// </summary>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="ArgumentException">An ArgumentException is raised if the object equality fails.</exception>
        public IEnsure<T> Is(T value)
        {
            if (!Object.Equals(this.inspectedObject, value))
            {
                this.Throw(new ArgumentException(this.GetFullErrorMessage("The currently inspected value is not equal to the supplied value."), this.Name));
            }

            return this;
        }

        /// <summary>
        /// Ensure that the supplied object is not equal to the currently inspected object.
        /// </summary>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="ArgumentException">An ArgumentException is raised if the object equality does not fail.</exception>
        public IEnsure<T> IsNot(T value)
        {
            if (Object.Equals(this.inspectedObject, value))
            {
                this.Throw(new ArgumentException(this.GetFullErrorMessage("The currently inspected value should be different from to the supplied value."), this.Name));
            }

            return this;
        }

        /// <summary>
        /// Throws the specified error if the previous If check has returned true.
        /// </summary>
        /// <param name="builder">The exception builder.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        public IEnsure<T> ThenThrow(Func<IEnsure<T>, Exception> builder)
        {
            if (state)
            {
                var exception = builder(this);
                this.Throw(exception);
            }

            return this;
        }

        Action<IEnsure<T>, Exception> validationFailurePreview;

        /// <summary>
        /// Allows the user to interceptthe ensure failure before the exception is raised.
        /// </summary>
        /// <param name="validationFailurePreview">The validation failure preview handler.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        public IEnsure<T> WithPreview(Action<IEnsure<T>, Exception> validationFailurePreview)
        {
            this.validationFailurePreview = validationFailurePreview;

            return this;
        }

        /// <summary>
        /// Throws the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        public void Throw(Exception error)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }

            if (this.validationFailurePreview != null)
            {
                this.validationFailurePreview(this, error);
            }

            throw error;
        }

        //Func<IEnsure<T>, Exception> builder;

        //public IEnsure<T> WithException( Func<IEnsure<T>, Exception> builder ) 
        //{
        //    this.builder = builder;

        //    return this;
        //}
    }
}
