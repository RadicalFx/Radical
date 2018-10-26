namespace Radical.ChangeTracking
{
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using Radical.ComponentModel.ChangeTracking;
    using Radical.Linq;

    public class ChangeSetDistinctVisitor : IChangeSetDistinctVisitor
    {
        public IDictionary<object, IChange> Visit( IChangeSet changeSet )
        {
            var distinct = new Dictionary<object, IChange>();

            changeSet.ForEach( change =>
            {
                /*
                 * recuperiamo un riferimento alle entities 
                 * che sono oggetto della modifica
                 */
                change.GetChangedEntities().ForEach( entity =>
                {
                    if( !distinct.ContainsKey( entity ) )
                    {
                        /*
                         * se l'entity non è tra quelle che abbiamo
                         * già incontrato la aggiungiamo.
                         */
                        distinct.Add( entity, change );
                    }
                    else
                    {
                        /*
                         * Se l'entity è già tra quelle visitate
                         * sostituiamo la IChange associata perchè
                         * la IChange più importante è l'ultima, che è
                         * quella che determina la ProposedActions che
                         * verrà proposta.
                         */
                        distinct[ entity ] = change;
                    }
                } );
            } );

            return distinct;
        }
    }
}
