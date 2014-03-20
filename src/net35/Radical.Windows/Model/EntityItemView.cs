//namespace Topics.Radical.Windows.Model
//{
//    using System.ComponentModel;
//    using System.Threading;
//    using Topics.Radical.ComponentModel;

//    class EntityItemView<T> : Topics.Radical.Model.EntityItemView<T> where T : class
//    {
//        public EntityItemView( IEntityView<T> view, T item )
//            : this( null, view, item )
//        {

//        }

//        public EntityItemView( AsyncOperation asyncOperation, IEntityView<T> view, T item )
//            : base( view, item )
//        {
//            this.asyncOperation = asyncOperation;
//        }

//        private AsyncOperation asyncOperation;
//        private SendOrPostCallback propertyChangedCallback;
//        private SendOrPostCallback editBegunCallback;
//        private SendOrPostCallback editEndedCallback;
//        private SendOrPostCallback editCanceledCallback;

//        protected override void OnInit()
//        {
//            base.OnInit();

//            if( this.asyncOperation == null )
//            {
//                this.asyncOperation = AsyncOperationManager.CreateOperation( null );
//            }

//            this.propertyChangedCallback = new SendOrPostCallback( obj =>
//            {
//                var args = ( PropertyChangedEventArgs )obj;
//                base.OnPropertyChanged( args );
//            } );

//            this.editBegunCallback = new SendOrPostCallback( obj => base.OnEditBegun() );
//            this.editEndedCallback = new SendOrPostCallback( obj => base.OnEditEnded() );
//            this.editCanceledCallback = new SendOrPostCallback( obj => base.OnEditCanceled() );
//        }

//        protected override void OnEditBegun()
//        {
//            this.asyncOperation.Post( this.editBegunCallback, null );
//        }

//        protected override void OnEditCanceled()
//        {
//            this.asyncOperation.Post( this.editCanceledCallback, null );
//        }

//        protected override void OnEditEnded()
//        {
//            this.asyncOperation.Post( this.editEndedCallback, null );
//        }

//        protected override void OnPropertyChanged( PropertyChangedEventArgs args )
//        {
//            this.asyncOperation.Post( this.propertyChangedCallback, args );
//        }
//    }
//}
