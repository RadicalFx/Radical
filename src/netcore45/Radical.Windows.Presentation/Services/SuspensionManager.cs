using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Windows.Storage;

namespace Topics.Radical.Windows.Presentation.Services
{
    class SuspensionManager : ISuspensionManager
    {
        Dictionary<String, StorageItem> storage = new Dictionary<String, StorageItem>();
        HashSet<Type> knownTypes;

        readonly String localFileName = "___local.state";
        readonly String roamingFileName = "___roaming.state";

        readonly IMessageBroker broker;
        readonly INavigationService navigation;
        readonly IAnalyticsServices analyticsServices;

        public SuspensionManager( IMessageBroker broker, INavigationService navigation, IKnownTypesProvider[] typeProviders, IAnalyticsServices analyticsServices )
        {
            this.broker = broker;
            this.navigation = navigation;
            this.analyticsServices = analyticsServices;

            //var kt = new Type[] 
            //{ 
            //	this.storage.GetType(),
            //	typeof( StorageLocation ), 
            //	typeof( StorageItem ) 
            //};

            //this.knownTypes.Add .AddRange( kt );


            this.knownTypes = new HashSet<Type>( typeProviders.SelectMany( p => p.GetKnownTypes() ) );
        }

        public object GetValue( string key )
        {
            return this.storage[ key ].Data;
        }

        public T GetValue<T>( string key )
        {
            return ( T )this.storage[ key ].Data;
        }

        public void SetValue( string key, object data, StorageLocation location )
        {
            this.storage[ key ] = new StorageItem()
            {
                Location = location,
                Data = data
            };
        }

        public void Remove( string key )
        {
            if ( this.storage.ContainsKey( key ) )
            {
                this.storage.Remove( key );
            }
        }

        public bool Contains( string key )
        {
            return this.storage.ContainsKey( key );
        }

        public async Task SuspendAsync()
        {
            await this.broker.DispatchAsync( this, new Messaging.ApplicationSuspend( this ) );

            this.navigation.Suspend( this );

            var localFile = await ApplicationData.Current.LocalFolder.CreateFileAsync( this.localFileName, CreationCollisionOption.ReplaceExisting );
            var localStorage = this.storage.Where( kvp => kvp.Value.Location == StorageLocation.Local )
                .ToDictionary( kvp => kvp.Key, kvp => kvp.Value );

            await Save( localStorage, localFile, this.knownTypes );

            var roamingFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync( this.roamingFileName, CreationCollisionOption.ReplaceExisting );
            var roamingStorage = this.storage.Where( kvp => kvp.Value.Location == StorageLocation.Roaming )
                .ToDictionary( kvp => kvp.Key, kvp => kvp.Value );

            await Save( roamingStorage, roamingFile, this.knownTypes );
        }

        static async Task Save( Dictionary<String, StorageItem> state, StorageFile file, IEnumerable<Type> knownTypes )
        {
            var data = new MemoryStream();
            var serializer = new DataContractSerializer( typeof( Dictionary<String, StorageItem> ), knownTypes );
            serializer.WriteObject( data, state );

            using ( Stream fileStream = await file.OpenStreamForWriteAsync() )
            {
                data.Seek( 0, SeekOrigin.Begin );
                await data.CopyToAsync( fileStream );
                await fileStream.FlushAsync();
            }
        }

        static async Task<Dictionary<string, StorageItem>> Restore( StorageFile file, IEnumerable<Type> knownTypes )
        {
            using ( var inStream = await file.OpenSequentialReadAsync() )
            {
                var serializer = new DataContractSerializer( typeof( Dictionary<String, StorageItem> ), knownTypes );
                var state = ( Dictionary<string, StorageItem> )serializer.ReadObject( inStream.AsStreamForRead() );

                return state;
            }
        }

        public async Task ResumeAsync()
        {
            var localFile = await ApplicationData.Current.LocalFolder.GetFileAsync( this.localFileName );
            var roamingFile = await ApplicationData.Current.RoamingFolder.GetFileAsync( this.roamingFileName );

            var localState = await Restore( localFile, this.knownTypes );
            var romaingState = await Restore( roamingFile, this.knownTypes );

            foreach ( var kvp in localState )
            {
                this.storage.Add( kvp.Key, kvp.Value );
            }

            foreach ( var kvp in romaingState )
            {
                this.storage.Add( kvp.Key, kvp.Value );
            }

            this.navigation.Resume( this );

            this.broker.Broadcast( this, new Messaging.ApplicationResumed( this ) );
        }
    }
}
