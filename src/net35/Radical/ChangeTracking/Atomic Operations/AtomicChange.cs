using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel.ChangeTracking;

namespace Topics.Radical.ChangeTracking
{
    sealed class AtomicChange : IChange
    {
        Dictionary<Object, Boolean> transientEntities = new Dictionary<Object, Boolean>();
#if FX40
        List<Tuple<IChange, AddChangeBehavior>> changes = new List<Tuple<IChange, AddChangeBehavior>>();
#else
        List<Values<IChange, AddChangeBehavior>> changes = new List<Values<IChange, AddChangeBehavior>>();
#endif

        /// <summary>
        /// Adds the specified change.
        /// </summary>
        /// <param name="change">The change.</param>
        /// <param name="behavior">The behavior.</param>
        public void Add( IChange change, AddChangeBehavior behavior )
        {
#if FX40
            this.changes.Add( new Tuple<IChange, AddChangeBehavior>( change, behavior ) );
#else
            this.changes.Add( new Values<IChange, AddChangeBehavior>( change, behavior ) );
#endif

        }

        /// <summary>
        /// Registers the transient.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="autoRemove">if set to <c>true</c> [auto remove].</param>
        public void RegisterTransient( Object entity, Boolean autoRemove )
        {
            this.transientEntities.Add( entity, autoRemove );
        }

        /// <summary>
        /// Gets the owner of this change, tipically the changed object.
        /// </summary>
        /// <value>The owner.</value>
        public object Owner
        {
            get { return this; }
        }

        /// <summary>
        /// Gets the state of the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// A set of values from the <see cref="EntityTrackingStates"/> enumeration.
        /// </returns>
        public EntityTrackingStates GetEntityState( Object entity )
        {
            var state = EntityTrackingStates.None;

            if( this.transientEntities != null && this.transientEntities.ContainsKey( entity ) )
            {
                state |= EntityTrackingStates.IsTransient;

                if( this.transientEntities[ entity ] )
                {
                    state |= EntityTrackingStates.AutoRemove;
                }
            }

#if FX40
            var isChanged = this.changes.Any( c => Object.Equals( c.Item1.Owner, entity ) );
#else
            var isChanged = this.changes.Any( c => Object.Equals( c.Value1.Owner, entity ) );
#endif
            if( isChanged )
            {
                var changed = EntityTrackingStates.HasBackwardChanges | EntityTrackingStates.HasForwardChanges;
                state |= changed;
            }

            return state;
        }

        /// <summary>
        /// Gets the changed entities holded by this IChange instance.
        /// </summary>
        /// <returns>A list of changed entities.</returns>
        public IEnumerable<object> GetChangedEntities()
        {

#if FX40
            var tmp = this.changes.SelectMany( c => c.Item1.GetChangedEntities() );
#else
            var tmp = this.changes.SelectMany( c => c.Value1.GetChangedEntities() );
#endif

            return tmp;
        }

        /// <summary>
        /// Commits this change.
        /// </summary>
        /// <param name="reason">The reason of the commit.</param>
        public void Commit( CommitReason reason )
        {
            foreach( var c in this.changes )
            {
#if FX40
                c.Item1.Commit( reason );
#else
                c.Value1.Commit( reason );
#endif
            }

            this.OnCommitted( new CommittedEventArgs( reason ) );
        }

        /// <summary>
        /// Occurs when this IChange has been committed.
        /// </summary>
        public event EventHandler<CommittedEventArgs> Committed;

        void OnCommitted( CommittedEventArgs e )
        {
            var h = this.Committed;
            if( h != null )
            {
                h( this, e );
            }
        }

        /// <summary>
        /// Rejects this change.
        /// </summary>
        /// <param name="reason">The reason of the reject.</param>
        public void Reject( RejectReason reason )
        {
            /*
             * Quando viene fatta una "reject" ogni singola
             * entity altro non fa che creare una nuova "change"
             * e piazzarla nel ForwardStack in questo modo siamo
             * in grado di fare "Redo". Alla luce di una AtomicChange
             * è evidente che questo meccanismo ci sbomba un po' 
             * tutto... dimostrando che dovrebbeessere il sistema di 
             * change tracking a fare la cosa. In realtà non è possibile
             * perchè il sistema non sa quale sia il valore attuale 
             * che dovrebbe essere cachato nel ForwardStack.
             * 
             * Quello che dovremmo fare quindi è creare qui una
             * nuova AtomicOperation in cui fare la reject e poi
             * infilare quella nel ForwardStack. L'inghippo è che non
             * abbiamo una reference al motore di Change Tracking...
             * 
             * in questo momento abbiamo uno special case sul servizio
             * in fase di reject che fa esattamente questo.
             */
            var reversed = this.changes.ToArray().Reverse();
            foreach( var c in reversed )
            {
#if FX40
                c.Item1.Reject( reason );
#else
                c.Value1.Reject( reason );
#endif
            }

            this.OnRejected( new RejectedEventArgs( reason ) );
        }

        /// <summary>
        /// Occurs when this IChange has been rejected.
        /// </summary>
        public event EventHandler<RejectedEventArgs> Rejected;

        void OnRejected( RejectedEventArgs e )
        {
            var h = this.Rejected;
            if( h != null )
            {
                h( this, e );
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance supports commit.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance supports commit; otherwise, <c>false</c>.
        /// </value>
        public bool IsCommitSupported
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return String.Empty; }
        }

        /// <summary>
        /// Gets the advised action for this IChange.
        /// </summary>
        /// <param name="changedItem"></param>
        /// <returns></returns>
        /// <value>The advised action.</value>
        public ProposedActions GetAdvisedAction( object changedItem )
        {
            /*
             * Abbiamo la lista di modifiche
             * seguiamo la stessa logica del
             * ChangeSetDistinctVisitor e ci 
             * andiamo a prendere l'ultima
             * modifica che è stata fatta
             * considerando quella come la più
             * importante per il changedItem
             * che stiamo considerando.
             */
#if FX40
            var actions = this.changes
                .Where( v => v.Item1.Owner == changedItem )
                .Select( v => v.Item1.GetAdvisedAction( changedItem ) );
#else
            var actions = this.changes
                .Where( v => v.Value1.Owner == changedItem )
                .Select( v => v.Value1.GetAdvisedAction( changedItem ) );
#endif


            return actions.Last();
        }

        /// <summary>
        /// Clones this IChange instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public IChange Clone()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Merges the transient entities.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public void MergeTransientEntities( IDictionary<object, bool> dictionary )
        {
            foreach( var kvp in this.transientEntities )
            {
                dictionary.Add( kvp );
            }

            this.transientEntities.Clear();
        }
    }
}
