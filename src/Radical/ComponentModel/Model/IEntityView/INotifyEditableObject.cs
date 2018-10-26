using System;
using System.ComponentModel;

namespace Radical.ComponentModel
{
    /// <summary>
    /// Extends the System.ComponentModel.IEditableObject interface.
    /// </summary>
    public interface INotifyEditableObject : IEditableObject
    {
        /// <summary>
        /// Notifies that an edit operation has begun.
        /// </summary>
        event EventHandler EditBegun;

        /// <summary>
        /// Notifies that an edit operation has benn canceled.
        /// </summary>
        event EventHandler EditCanceled;
        
        /// <summary>
        /// Notifies that an edit operation has ended.
        /// </summary>
        event EventHandler EditEnded;
    }
}
