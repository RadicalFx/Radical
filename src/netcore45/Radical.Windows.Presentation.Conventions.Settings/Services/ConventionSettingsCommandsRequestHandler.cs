using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Windows.Presentation.Boot;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Windows.UI.ApplicationSettings;
using Topics.Radical.Reflection;
using System.Reflection;
using Callisto.Controls;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Radical.Windows.Presentation.Conventions.Settings.Services
{
    class ConventionSettingsCommandsRequestHandler : ISettingsCommandsRequestHandler
    {
        readonly IViewResolver viewResolver;
        readonly ApplicationBootstrapper bootstrapper;
        readonly IConventionsHandler conventions;

        IEnumerable<TypeInfo> allSettingTypes = null;

        public ConventionSettingsCommandsRequestHandler( IViewResolver viewResolver, ApplicationBootstrapper bootstrapper, IConventionsHandler conventions )
        {
            this.viewResolver = viewResolver;
            this.bootstrapper = bootstrapper;
            this.conventions = conventions;
        }

        static object TryFindResource( FrameworkElement element, object resourceKey )
        {
            var currentElement = element;

            while ( currentElement != null )
            {
                var resource = currentElement.Resources[ resourceKey ];
                if ( resource != null )
                {
                    return resource;
                }

                currentElement = currentElement.Parent as FrameworkElement;
            }

            return Application.Current.Resources[ resourceKey ];
        }

        public void Handle( SettingsPaneCommandsRequestedEventArgs e )
        {
            if ( this.allSettingTypes == null )
            {
                this.allSettingTypes = this.bootstrapper.BoottimeTypesProvider()
                    .Where( t => t.GetCustomAttribute<SettingDescriptorAttribute>() != null )
                    .ToArray();
            }


            this.allSettingTypes.Aggregate( e.Request.ApplicationCommands, ( commands, type ) =>
            {
                var attribute = type.GetCustomAttribute<SettingDescriptorAttribute>();

                var command = new SettingsCommand( attribute.CommandId, attribute.CommandLabel, ( handler ) =>
                {
                    var viewModelType = type.AsType();
                    var viewType = this.conventions.ResolveViewType( viewModelType );

                    var view = this.viewResolver.GetView( viewType );
                    var viewModel = this.conventions.GetViewDataContext( view ) as ComponentModel.ISettingsViewModel;

                    var settings = new SettingsFlyout();
                    settings.Content = view;

                    if ( viewModel != null )
                    {
                        settings.HeaderBrush = viewModel.HeaderBrush;
                        settings.Background = viewModel.Background;
                        settings.HeaderText = viewModel.HeaderText;
                    }
                    else
                    {
                        settings.HeaderText = attribute.CommandLabel;

                        var fe = view as FrameworkElement;
                        if ( fe != null ) 
                        {
                            var brush = TryFindResource( fe, attribute.CommandId + "SettingHeaderBrush" ) as SolidColorBrush;
                            if ( brush == null ) 
                            {
                                brush = TryFindResource( fe, "DefaultSettingHeaderBrush" ) as SolidColorBrush;
                            }

                            settings.HeaderBrush = brush;
                        }
                    }

                    settings.IsOpen = true;
                } );

                commands.Add( command );

                return commands;
            } );
        }
    }
}
