using Radical.ComponentModel.ChangeTracking;
using Radical.Linq;
using System.Collections.Generic;

namespace Radical.ChangeTracking
{
    /// <summary>
    /// Visits a change set and produces a dictionary of distinct entities mapped to their most recent change.
    /// </summary>
    public class ChangeSetDistinctVisitor : IChangeSetDistinctVisitor
    {
        /// <summary>
        /// Visits the specified change set and returns a dictionary where each key is a distinct entity
        /// and the value is the last <see cref="IChange"/> recorded for that entity.
        /// </summary>
        /// <param name="changeSet">The change set to visit.</param>
        /// <returns>A dictionary mapping each distinct entity to its most recent change.</returns>
        public IDictionary<object, IChange> Visit(IChangeSet changeSet)
        {
            var distinct = new Dictionary<object, IChange>();

            changeSet.ForEach(change =>
            {
                /*
                 * recuperiamo un riferimento alle entities 
                 * che sono oggetto della modifica
                 */
                change.GetChangedEntities().ForEach(entity =>
                {
                    if (!distinct.ContainsKey(entity))
                    {
                        /*
                         * se l'entity non è tra quelle che abbiamo
                         * già incontrato la aggiungiamo.
                         */
                        distinct.Add(entity, change);
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
                        distinct[entity] = change;
                    }
                });
            });

            return distinct;
        }
    }
}
