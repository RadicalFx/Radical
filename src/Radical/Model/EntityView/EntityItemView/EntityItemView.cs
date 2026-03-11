using Radical.ComponentModel;
using System;
using System.Collections;
using System.ComponentModel;

namespace Radical.Model
{
    /// <summary>
    /// Represents a view item that wraps an entity of type <typeparamref name="T"/> for use within an <see cref="IEntityView{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the underlying entity item.</typeparam>
    public class EntityItemView<T> :
        IEntityItemView<T>,
        IDataErrorInfo,
        INotifyEditableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemView&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="entityItem">The entity item.</param>
        public EntityItemView(IEntityView<T> view, T entityItem)
        {
            _view = view;
            EntityItem = entityItem;

            OnInit();
        }

        /// <summary>
        /// Called during initialization, after the view and entity item have been set. Override to perform additional setup.
        /// </summary>
        protected virtual void OnInit()
        {

        }

        private readonly IEntityView<T> _view;
        /// <summary>
        /// Gets a reference the view that owns this instance.
        /// </summary>
        /// <item>The owner view.</item>
        public IEntityView<T> View
        {
            get { return _view; }
        }

        /// <summary>
        /// Deletes this IEntityItemView and removes if from the view and from the underlying collection.
        /// </summary>
        public void Delete()
        {
            int myIndex = View.IndexOf(this);
            View.RemoveAt(myIndex);
        }

        private T _entityItem;
        /// <summary>
        /// Gets the underlying entity item.
        /// </summary>
        /// <item>The underlying entity item.</item>
        public T EntityItem
        {
            get { return _entityItem; }
            private set
            {
                if (EntityItem != null /* && this.tIsINotifyPropertyChanged */ && EntityItem is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)EntityItem).PropertyChanged -= new PropertyChangedEventHandler(OnItemPropertyChanged);
                }

                if (value == null)
                {
                    throw new ArgumentNullException("item");
                }

                _entityItem = value;
                if (EntityItem is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)EntityItem).PropertyChanged += new PropertyChangedEventHandler(OnItemPropertyChanged);
                }
            }
        }

        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }

        /// <summary>
        /// Occurs when a property item changes.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <item></item>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        public virtual string Error
        {
            get
            {
                if (EntityItem is IDataErrorInfo)
                {
                    return ((IDataErrorInfo)EntityItem).Error;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <value>The error message for the property. The default is an empty string ("").</value>
        public virtual string this[string columnName]
        {
            get
            {
                //TODO: si potrebbe trovare un sistema per rimbalzare su chi espone la proprietà
                //if( this.View.IsCustomPropertyDefined( columnName ) ) 
                //{

                //}

                if (EntityItem is IDataErrorInfo)
                {
                    return ((IDataErrorInfo)EntityItem)[columnName];
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Notifies that an edit operation has begun.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EditBegun;

        /// <summary>
        /// Raises the <see cref="EditBegun"/> event.
        /// </summary>
        protected virtual void OnEditBegun()
        {
            EditBegun?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Notifies that an edit operation has been canceled.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EditCanceled;
        /// <summary>
        /// Raises the <see cref="EditCanceled"/> event.
        /// </summary>
        protected virtual void OnEditCanceled()
        {
            EditCanceled?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Notifies that an edit operation has ended.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EditEnded;
        /// <summary>
        /// Raises the <see cref="EditEnded"/> event.
        /// </summary>
        protected virtual void OnEditEnded()
        {
            EditEnded?.Invoke(this, EventArgs.Empty);
        }

        private bool isEditing;

        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public void BeginEdit()
        {
            if (!isEditing)
            {
                isEditing = true;

                if (EntityItem is IEditableObject)
                {
                    ((IEditableObject)EntityItem).BeginEdit();
                }

                OnEditBegun();
            }
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public void CancelEdit()
        {
            if (isEditing)
            {
                if (EntityItem is IEditableObject)
                {
                    ((IEditableObject)EntityItem).CancelEdit();
                }

                OnEditCanceled();
                isEditing = false;
            }
        }

        /// <summary>
        /// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
        /// </summary>
        public void EndEdit()
        {
            if (isEditing)
            {
                if (EntityItem is IEditableObject)
                {
                    ((IEditableObject)EntityItem).EndEdit();
                }

                OnEditEnded();
                isEditing = false;
            }
        }

        /// <summary>
        /// Gets the underlying entity item.
        /// </summary>
        /// <item>The underlying entity item.</item>
        object IEntityItemView.EntityItem
        {
            get
            {
                return EntityItem;
            }
        }

        /// <summary>
        /// Gets a reference the view that owns this instance.
        /// </summary>
        /// <item>The owner view.</item>
        IEntityView IEntityItemView.View
        {
            get { return View; }
        }

        /// <summary>
        /// Sets the value of a custom property on this item view.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="customPropertyName">The name of the custom property.</param>
        /// <param name="value">The value to set.</param>
        public void SetCustomValue<TValue>(string customPropertyName, TValue value)
        {
            var beforeSet = GetCustomValue<TValue>(customPropertyName);
            View.SetCustomPropertyValue(customPropertyName, this, value);

            if (!Equals(beforeSet, value))
            {
                OnPropertyChanged(new PropertyChangedEventArgs(customPropertyName));
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        public void NotifyPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets the value of a custom property on this item view.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="customPropertyName">The name of the custom property.</param>
        /// <returns>The current value of the custom property.</returns>
        public TValue GetCustomValue<TValue>(string customPropertyName)
        {
            return View.GetCustomPropertyValue<TValue>(customPropertyName, this);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            return _view.GetItemProperties(null);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return AttributeCollection.Empty;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return EventDescriptorCollection.Empty;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return EventDescriptorCollection.Empty;
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return null;
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return null;
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return null;
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return null;
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return null;
        }

        /// <summary>
        /// Returns a hash code for this instance based on the underlying entity item.
        /// </summary>
        /// <returns>A hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return EntityItem.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><c>true</c> if the specified object represents the same entity item; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is IEntityItemView<T> other)
            {
                return Equals(EntityItem, other.EntityItem);
            }

            return ReferenceEquals(this, obj);
        }
    }
}
