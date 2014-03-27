using System;
using System.Windows;

namespace Topics.Radical.Windows.Presentation.Regions
{
    /// <summary>
    /// A static class used to manage the attached properties for a region headered element.
    /// Use this class to easily add a custom Header to any <see cref="DependencyObject"/> that will be injected in a <see cref="TabControlRegion"/>.
    /// </summary>
    public static class RegionHeaderedElement
    {
        /// <summary>
        /// Identifies the Topics.Radical.Windows.Presentation.Regions.Specialized.RegionHeaderedElement.Header dependency property.
        /// </summary>
        /// <returns>The identifier for the Topics.Radical.Windows.Presentation.Regions.Specialized.RegionHeaderedElement.Header dependency property.</returns>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.RegisterAttached(
            "Header",
            typeof(object),
            typeof(RegionHeaderedElement),
            new PropertyMetadata(null));

        /// <summary>
        /// Sets the value of the Topics.Radical.Windows.Presentation.Regions.Specialized.RegionHeaderedElement.Header attached property for a specified dependency object.
        /// </summary>
        /// <param name="element">The dependency object for which to set the value of the Topics.Radical.Windows.Presentation.Regions.Specialized.RegionHeaderedElement.Header property.</param>
        /// <param name="value">The new value to set the property to.</param>
        public static void SetHeader(DependencyObject element, object value)
        {
            element.SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Returns the value of the Topics.Radical.Windows.Presentation.Regions.Specialized.RegionHeaderedElement.Header attached property for a specified dependency object.
        /// </summary>
        /// <param name="element">The dependency object for which to retrieve the value of the Topics.Radical.Windows.Presentation.Regions.Specialized.RegionHeaderedElement.Header property.</param>
        /// <returns>The current value of the Topics.Radical.Windows.Presentation.Regions.Specialized.RegionHeaderedElement.Header attached property on the specified dependency object.</returns>
        /// <exception cref="ArgumentNullException">d is null.</exception>
        public static object GetHeader(DependencyObject element)
        {
            return (object)element.GetValue(HeaderProperty);
        }
    }
}