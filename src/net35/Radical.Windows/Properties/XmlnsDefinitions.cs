using System.Windows.Markup;

#if !WINDOWS_PHONE

[assembly: XmlnsDefinition( "http://schemas.topics.it/wpf/radical/windows/behaviors", "Topics.Radical.Windows.Behaviors" )]
[assembly: XmlnsDefinition( "http://schemas.topics.it/wpf/radical/windows/input", "Topics.Radical.Windows.Input" )]
[assembly: XmlnsDefinition( "http://schemas.topics.it/wpf/radical/windows", "Topics.Radical.Windows" )]
[assembly: XmlnsDefinition( "http://schemas.topics.it/wpf/radical/windows/converters", "Topics.Radical.Windows.Converters" )]
[assembly: XmlnsDefinition( "http://schemas.topics.it/wpf/radical/windows/controls", "Topics.Radical.Windows.Controls" )]

#endif

#if !WINDOWS_PHONE && !SILVERLIGHT

[assembly: XmlnsDefinition( "http://schemas.topics.it/wpf/radical/windows/effects", "Topics.Radical.Windows.Effects" )]
[assembly: XmlnsDefinition( "http://schemas.topics.it/wpf/radical/windows/markup", "Topics.Radical.Windows.Markup" )]

#endif