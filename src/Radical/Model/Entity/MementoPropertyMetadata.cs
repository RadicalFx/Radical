using Radical.ComponentModel.ChangeTracking;
using Radical.Linq;
using Radical.Reflection;
using System;
using System.Linq.Expressions;

namespace Radical.Model
{
    public static class MementoPropertyMetadata
    {
        public static MementoPropertyMetadata<T> Create<T>(object propertyOwner, Expression<Func<T>> property)
        {
            return new MementoPropertyMetadata<T>(propertyOwner, property);
        }

        public static MementoPropertyMetadata<T> Create<T>(object propertyOwner, string propertyName)
        {
            return new MementoPropertyMetadata<T>(propertyOwner, propertyName);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public class MementoPropertyMetadata<T> : PropertyMetadata<T>,
        IMementoPropertyMetadata
    {
        public MementoPropertyMetadata(object propertyOwner, string propertyName)
            : base(propertyOwner, propertyName)
        {
            if (this.Property.IsAttributeDefined<MementoPropertyMetadataAttribute>())
            {
                var attribute = this.Property.GetAttribute<MementoPropertyMetadataAttribute>();
                this.TrackChanges = attribute.TrackChanges;
            }
            else
            {
                this.TrackChanges = true;
            }
        }

        public MementoPropertyMetadata(object propertyOwner, Expression<Func<T>> property)
            : this(propertyOwner, property.GetMemberName())
        {

        }

        public bool TrackChanges { get; set; }

        public MementoPropertyMetadata<T> DisableChangesTracking()
        {
            this.TrackChanges = false;
            return this;
        }

        public MementoPropertyMetadata<T> EnableChangesTracking()
        {
            this.TrackChanges = true;
            return this;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MementoPropertyMetadataAttribute : PropertyMetadataAttribute
    {
        public MementoPropertyMetadataAttribute()
        {
            this.TrackChanges = true;
        }

        public bool TrackChanges { get; set; }
    }
}
