using Radical.ComponentModel.ChangeTracking;
using Radical.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Radical.ChangeTracking
{
    /// <summary>
    /// Offers a default implementation of the <see cref="IAdvisoryBuilder"/> interface.
    /// </summary>
    public class AdvisoryBuilder : IAdvisoryBuilder
    {
        readonly IChangeSetDistinctVisitor visitor = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvisoryBuilder"/> class.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public AdvisoryBuilder(IChangeSetDistinctVisitor visitor)
        {
            Ensure.That(visitor).Named("visitor").IsNotNull();

            this.visitor = visitor;
        }

        /// <summary>
        /// Generates the advisory.
        /// </summary>
        /// <param name="svc">The service that holds the data to generate the advisory for.</param>
        /// <param name="changeSet">The subset of changes to generate the advisory for.</param>
        /// <returns>The generated advisory.</returns>
        public IAdvisory GenerateAdvisory(IChangeTrackingService svc, IChangeSet changeSet)
        {
            var result = new List<IAdvisedAction>();

            var distinct = this.visitor.Visit(changeSet);
            foreach (var kvp in distinct)
            {
                ProposedActions proposedAction = kvp.Value.GetAdvisedAction(kvp.Key);
                EntityTrackingStates state = svc.GetEntityState(kvp.Key);
                bool isTransient = (state & EntityTrackingStates.IsTransient) == EntityTrackingStates.IsTransient;

                switch (proposedAction)
                {
                    case ProposedActions.Create | ProposedActions.Update:
                        proposedAction = isTransient ? ProposedActions.Create : ProposedActions.Update;
                        break;

                    case ProposedActions.Delete | ProposedActions.Dispose:
                        proposedAction = isTransient ? ProposedActions.Dispose : ProposedActions.Delete;
                        break;

                    default:
                        throw new NotSupportedException();
                }

                var advisedAction = this.OnCreateAdvisedAction(kvp.Key, proposedAction);
                result.Add(advisedAction);
            }

            IEnumerable transientEntities = svc.GetEntities(EntityTrackingStates.IsTransient, true);
            foreach (object te in transientEntities)
            {
                if (result.Any(a => a.Target == te))
                {
                    /*
                     * An entity is created as Transient+Persistable, then is added to
                     * a MementoEntityCollection that tracks a change as "item-added" thus
                     * the advisory adds that entity (that is not AutoRemove) as something that
                     * must be created. so if we arrive here and we already have the transient
                     * entity in the advisory we skip it.
                     */
                    continue;
                }

                var advisedAction = this.OnCreateAdvisedAction(te, ProposedActions.Create);
                result.Add(advisedAction);
            }

            return new Advisory(result);
        }

        /// <summary>
        /// Called in order to create the advised action for the give target.
        /// </summary>
        /// <param name="target">The target entity.</param>
        /// <param name="proposedAction">The proposed action.</param>
        /// <returns>The advised action.</returns>
        protected virtual IAdvisedAction OnCreateAdvisedAction(object target, ProposedActions proposedAction)
        {
            var advisedAction = new AdvisedAction(target, proposedAction);
            return advisedAction;
        }
    }
}
