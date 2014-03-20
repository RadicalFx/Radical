//using System;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using Topics.Radical.Messaging;
//using System.Windows.Navigation;

//namespace Topics.Radical.Windows.Phone.Shell.Services
//{
//    public class NavigatingMessage : Message
//    {
//        public NavigatingMessage( Object sender, Boolean isTombstoning, NavigatingCancelEventArgs e )
//            : base( sender )
//        {
//            this.IsTombstoning = isTombstoning;
			
//            //i messaggi vanno in broadcast non è possibile "aspettare"
//            //se non con un waithandle ma non ha senso per ora
//            //this.Cancel = e.Cancel;
//            this.NavigationMode = e.NavigationMode;
//            this.Destination = e.Uri;
//        }

//        public Boolean IsTombstoning { get; private set; }

//        //public Boolean Cancel { get; set; }

//        public NavigationMode NavigationMode { get; private set; }

//        public Uri Destination { get; private set; }
//    }
//}
