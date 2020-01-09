using System;
using System.Reflection;
using System.Resources;

namespace Radical
{
    /// <summary>
    /// Extend the <see cref="EnumItemDescriptionAttribute"/> providing localization
    /// functionalities.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes"), AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class LocalizableEnumItemDescriptionAttribute : EnumItemDescriptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableEnumItemDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="captionKey">The description key.</param>
        public LocalizableEnumItemDescriptionAttribute(string captionKey)
            : base(captionKey)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableEnumItemDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="captionKey">The caption key.</param>
        /// <param name="index">The index.</param>
        public LocalizableEnumItemDescriptionAttribute(string captionKey, int index)
            : base(captionKey, index)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableEnumItemDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="captionKey">The caption key.</param>
        /// <param name="descriptionKey">The description key.</param>
        /// <param name="index">The index.</param>
        public LocalizableEnumItemDescriptionAttribute(string captionKey, string descriptionKey, int index)
            : base(captionKey, descriptionKey, index)
        {

        }

        private ResourceManager _resourceManager;

        /// <summary>
        /// Gets the resource manager.
        /// </summary>
        /// <value>The resource manager.</value>
        protected ResourceManager ResourceManager
        {
            get
            {
                if (_resourceManager == null)
                {
                    Assembly assembly = null;

                    switch (AssemblyLocationBehavior)
                    {
                        case ResourceAssemblyLocationBehavior.UseExecutingAssembly:
                            assembly = Assembly.GetExecutingAssembly();
                            break;

                        case ResourceAssemblyLocationBehavior.UseCallingAssembly:
                            assembly = Assembly.GetCallingAssembly();
                            break;

                        case ResourceAssemblyLocationBehavior.UseEntryAssembly:
                            assembly = Assembly.GetEntryAssembly();
                            break;

                        case ResourceAssemblyLocationBehavior.ByAssemblyName:
                            assembly = Assembly.Load(AssemblyName);
                            break;

                        default:
                            throw new NotSupportedException($"ResourceAssemblyLocationBehavior value ('{AssemblyLocationBehavior}') not supported.");
                    }

                    _resourceManager = new ResourceManager(ResourceName, assembly);
                }

                return _resourceManager;
            }
        }

        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>The name of the resource.</value>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        /// <value>The name of the assembly.</value>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the caption fall-back value.
        /// </summary>
        /// <value>
        /// The caption fall-back value.
        /// </value>
        public string CaptionFallbackValue { get; set; }

        /// <summary>
        /// Gets or sets the description fall-back value.
        /// </summary>
        /// <value>
        /// The description fall-back value.
        /// </value>
        public string DescriptionFallbackValue { get; set; }

        /// <summary>
        /// Gets or sets the assembly location behavior.
        /// </summary>
        /// <value>
        /// The assembly location behavior.
        /// </value>
        public ResourceAssemblyLocationBehavior AssemblyLocationBehavior { get; set; }

        /// <summary>
        /// Called by Caption getter, override this
        /// method to customize Caption return value
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <returns>
        /// The Caption value.
        /// </returns>
        protected override string OnGetCaption(string caption)
        {
            var value = ResourceManager.GetString(caption);

            return value ?? CaptionFallbackValue;
        }

        /// <summary>
        /// Called by Description getter, override this
        /// method to customize Description return value
        /// </summary>
        /// <param name="description"></param>
        /// <returns>
        /// The Description value.
        /// </returns>
        protected override string OnGetDescription(string description)
        {
            var value = ResourceManager.GetString(description);

            return value ?? DescriptionFallbackValue;
        }
    }
}
