using Radical.ComponentModel;
using System;
using System.Collections;
using System.ComponentModel;

namespace Radical.Model
{
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
            ((IList)View).RemoveAt(myIndex);
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

        protected virtual void OnEditBegun()
        {
            EditBegun?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Notifies that an edit operation has been canceled.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EditCanceled;
        protected virtual void OnEditCanceled()
        {
            EditCanceled?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Notifies that an edit operation has ended.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EditEnded;
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

        public void SetCustomValue<TValue>(string customPropertyName, TValue value)
        {
            var beforeSet = GetCustomValue<TValue>(customPropertyName);
            View.SetCustomPropertyValue(customPropertyName, this, value);

            if (!Equals(beforeSet, value))
            {
                OnPropertyChanged(new PropertyChangedEventArgs(customPropertyName));
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

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

        public override int GetHashCode()
        {
            return EntityItem.GetHashCode();// base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is IEntityItemView<T> other)
            {
                return Equals(EntityItem, other.EntityItem);
            }

            return ReferenceEquals(this, obj); // this.EntityItem.Equals( obj );
        }
    }
}
