using System;
using System.Reflection;
using System.Resources;

namespace Radical
{
    /// <summary>
    /// Extend the <see cref="EnumItemDescriptionAttribute"/> providing localization
    /// functionalities.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1813:AvoidUnsealedAttributes" ), AttributeUsage( AttributeTargets.Field, AllowMultiple = false, Inherited = false )]
    public class LocalizableEnumItemDescriptionAttribute : EnumItemDescriptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableEnumItemDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="captionKey">The description key.</param>
        public LocalizableEnumItemDescriptionAttribute( String captionKey )
            : base( captionKey )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableEnumItemDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="captionKey">The caption key.</param>
        /// <param name="index">The index.</param>
        public LocalizableEnumItemDescriptionAttribute( String captionKey, Int32 index )
            : base( captionKey, index )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableEnumItemDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="captionKey">The caption key.</param>
        /// <param name="descriptionKey">The description key.</param>
        /// <param name="index">The index.</param>
        public LocalizableEnumItemDescriptionAttribute( String captionKey, String descriptionKey, Int32 index )
            : base( captionKey, descriptionKey, index )
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
                if ( this._resourceManager == null )
                {
                    Assembly assembly = null;

                    switch ( this.AssemblyLocationBehavior )
                    {
                        case ResourceAssemblyLocationBehavior.UseExecutingAssembly:
                            assembly = Assembly.GetExecutingAssembly();
                            break;

                        case ResourceAssemblyLocationBehavior.UseCallingAssembly:
                            assembly = Assembly.GetCallingAssembly();
                            break;

#if !SILVERLIGHT
                        case ResourceAssemblyLocationBehavior.UseEntryAssembly:
                            assembly = Assembly.GetEntryAssembly();
                            break;
#endif

                        case ResourceAssemblyLocationBehavior.ByAssemblyName:
                        default:
                            assembly = Assembly.Load( this.AssemblyName );
                            break;
                    }

                    this._resourceManager = new ResourceManager( this.ResourceName, assembly );
                }

                return this._resourceManager;
            }
        }

        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>The name of the resource.</value>
        public String ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        /// <value>The name of the assembly.</value>
        public String AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the caption fallback value.
        /// </summary>
        /// <value>
        /// The caption fallback value.
        /// </value>
        public String CaptionFallbackValue { get; set; }

        /// <summary>
        /// Gets or sets the description fallback value.
        /// </summary>
        /// <value>
        /// The description fallback value.
        /// </value>
        public String DescriptionFallbackValue { get; set; }

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
        protected override String OnGetCaption( String caption )
        {
            var value = this.ResourceManager.GetString( caption );

            return value ?? this.CaptionFallbackValue;
        }

        /// <summary>
        /// Called by Description getter, override this
        /// method to customize Description return value
        /// </summary>
        /// <param name="description"></param>
        /// <returns>
        /// The Description value.
        /// </returns>
        protected override string OnGetDescription( String description )
        {
            var value = this.ResourceManager.GetString( description );

            return value ?? this.DescriptionFallbackValue;
        }
    }
}
