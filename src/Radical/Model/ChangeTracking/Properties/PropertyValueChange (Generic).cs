namespace Radical.ChangeTracking.Specialized
{
    using Radical.ChangeTracking;
    using Radical.ComponentModel.ChangeTracking;
    using Radical.Validation;
    using System;

    /// <summary>
    /// Identifies a change specific for a property change.
    /// </summary>
    public class PropertyValueChange<T> : Change<T>, IChange, IPropertyValueChange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValueChange&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="restoreCallback">The restore callback.</param>
        public PropertyValueChange(Object owner, string propertyName, T value, RejectCallback<T> restoreCallback)
            : this(owner, propertyName, value, restoreCallback, null, string.Empty)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValueChange&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="restoreCallback">The restore callback.</param>
        /// <param name="description">The description.</param>
        public PropertyValueChange(Object owner, string propertyName, T value, RejectCallback<T> restoreCallback, string description)
            : this(owner, propertyName, value, restoreCallback, null, description)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValueChange&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="restoreCallback">The restore callback.</param>
        /// <param name="commitCallback">The commit callback.</param>
        /// <param name="description">The description.</param>
        public PropertyValueChange(Object owner, string propertyName, T value, RejectCallback<T> restoreCallback, CommitCallback<T> commitCallback, string description)
            : base(owner, value, restoreCallback, commitCallback, description)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Gets the advised action for this IChange.
        /// </summary>
        /// <param name="changedItem"></param>
        /// <returns></returns>
        /// <value>The advised action.</value>
        public override ProposedActions GetAdvisedAction(object changedItem)
        {
            Ensure.That(changedItem)
                .Named("changedItem")
                .IsNotNull()
                .If(v => v != this.Owner)
                .Then((v, n) =>
               {
                   throw new ArgumentOutOfRangeException(n);
               });

            return ProposedActions.Update | ProposedActions.Create;
        }

        /// <summary>
        /// Clones this IChange instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public override IChange Clone()
        {
            return new PropertyValueChange<T>(this.Owner, this.PropertyName, this.CachedValue, this.RejectCallback, this.CommitCallback, this.Description);
        }
    }
}