using Radical.ComponentModel;
using System;
using System.Collections;
using System.ComponentModel;

namespace Radical.Model
{
    public class EntityItemView<T> :
        IEntityItemView,
        IEntityItemView<T>,
        IDataErrorInfo,
        INotifyPropertyChanged,
        INotifyEditableObject
    //where T : class
    {
        //readonly IDictionary<string, Object> customValues = new Dictionary<string, Object>();

        #region .ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemView&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="entityItem">The entity item.</param>
        public EntityItemView(IEntityView<T> view, T entityItem)
        {
            this._view = view;
            this.EntityItem = entityItem;

            this.OnInit();
        }

        #endregion

        protected virtual void OnInit()
        {

        }

        private IEntityView<T> _view;
        /// <summary>
        /// Gets a reference the view that owns this instance.
        /// </summary>
        /// <item>The owner view.</item>
        public IEntityView<T> View
        {
            get { return this._view; }
        }

        /// <summary>
        /// Deletes this IEntityItemView and removes if from the view and from the underlying collection.
        /// </summary>
        public void Delete()
        {
            int myIndex = this.View.IndexOf(this);
            ((IList)this.View).RemoveAt(myIndex);
        }

        private T _entityItem;
        /// <summary>
        /// Gets the underlying entity item.
        /// </summary>
        /// <item>The underlying entity item.</item>
        public T EntityItem
        {
            get { return this._entityItem; }
            private set
            {
                if (this.EntityItem != null /* && this.tIsINotifyPropertyChanged */ && this.EntityItem is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)this.EntityItem).PropertyChanged -= new PropertyChangedEventHandler(OnItemPropertyChanged);
                }

                if (value == null)
                {
                    throw new ArgumentNullException("item");
                }

                this._entityItem = value;
                if (this.EntityItem is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)this.EntityItem).PropertyChanged += new PropertyChangedEventHandler(OnItemPropertyChanged);
                }
            }
        }

        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e);
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
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, args);
            }
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
                if (this.EntityItem is IDataErrorInfo)
                {
                    return ((IDataErrorInfo)this.EntityItem).Error;
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
                //si potrebbe trovare un sistema per rimbalzare su chi espone la proprietà
                //if( this.View.IsCustomPropertyDefined( columnName ) ) 
                //{

                //}

                if (this.EntityItem is IDataErrorInfo)
                {
                    return ((IDataErrorInfo)this.EntityItem)[columnName];
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
            if (this.EditBegun != null)
            {
                this.EditBegun(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Notifies that an edit operation has benn canceled.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EditCanceled;
        protected virtual void OnEditCanceled()
        {
            if (this.EditCanceled != null)
            {
                this.EditCanceled(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Notifies that an edit operation has ended.
        /// </summary>
        [field: NonSerialized]
        public event EventHandler EditEnded;
        protected virtual void OnEditEnded()
        {
            if (this.EditEnded != null)
            {
                this.EditEnded(this, EventArgs.Empty);
            }
        }

        private bool isEditing;

        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public void BeginEdit()
        {
            if (!this.isEditing)
            {
                this.isEditing = true;

                if (this.EntityItem is IEditableObject)
                {
                    ((IEditableObject)this.EntityItem).BeginEdit();
                }

                this.OnEditBegun();
            }
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public void CancelEdit()
        {
            if (this.isEditing)
            {
                if (this.EntityItem is IEditableObject)
                {
                    ((IEditableObject)this.EntityItem).CancelEdit();
                }

                this.OnEditCanceled();
                this.isEditing = false;
            }
        }

        /// <summary>
        /// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
        /// </summary>
        public void EndEdit()
        {
            if (this.isEditing)
            {
                if (this.EntityItem is IEditableObject)
                {
                    ((IEditableObject)this.EntityItem).EndEdit();
                }

                this.OnEditEnded();
                this.isEditing = false;
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
                return this.EntityItem;
            }
        }

        /// <summary>
        /// Gets a reference the view that owns this instance.
        /// </summary>
        /// <item>The owner view.</item>
        IEntityView IEntityItemView.View
        {
            get { return this.View; }
        }

        public void SetCustomValue<TValue>(string customPropertyName, TValue value)
        {
            var beforeSet = this.GetCustomValue<TValue>(customPropertyName);
            this.View.SetCustomPropertyValue(customPropertyName, this, value);

            if (!Object.Equals(beforeSet, value))
            {
                this.OnPropertyChanged(new PropertyChangedEventArgs(customPropertyName));
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public TValue GetCustomValue<TValue>(string customPropertyName)
        {
            return this.View.GetCustomPropertyValue<TValue>(customPropertyName, this);
        }

        Object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            //return TypeDescriptor.GetProperties( this.EntityItem );

            return this._view.GetItemProperties(null);
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

        Object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return null;
        }

        public override int GetHashCode()
        {
            return this.EntityItem.GetHashCode();// base.GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            var other = obj as IEntityItemView<T>;
            if (other != null)
            {
                return Object.Equals(this.EntityItem, other.EntityItem);
            }

            return Object.ReferenceEquals(this, obj); // this.EntityItem.Equals( obj );
        }
    }
}
