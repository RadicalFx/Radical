using System.Windows.Markup;

#if !WINDOWS_PHONE

[assembly: XmlnsDefinition(
	"http://schemas.topics.it/wpf/radical/windows/presentation/behaviors", 
	"Topics.Radical.Windows.Presentation.Behaviors" 
)]

[assembly: XmlnsDefinition( 
	"http://schemas.topics.it/wpf/radical/windows/presentation/regions",
	"Topics.Radical.Windows.Presentation.Regions" )]

#endif

#if !WINDOWS_PHONE && !SILVERLIGHT

#endif