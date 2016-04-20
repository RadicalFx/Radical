namespace Topics.Radical.Transactions
{
    using System;
    using System.Transactions;
    using Topics.Radical;

    /// <summary>
    /// A simple helper used to enslist a transaction 
    /// mamager in an existing transaction.
    /// </summary>
    public sealed class TransactionEnlistmentHelper : IDisposable
    {
        private static readonly Object syncRoot = new Object();

        #region IDisposable Members

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="TransactionEnlistmentHelper"/> is reclaimed by garbage collection.
        /// </summary>
        ~TransactionEnlistmentHelper()
        {
            this.Dispose( false );
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><collection>true</collection> to release both managed and unmanaged resources; <collection>false</collection> to release only unmanaged resources.</param>
        void Dispose( Boolean disposing )
        {
            if( disposing )
            {
                /*
                 * Se disposing è 'true' significa che dispose
                 * è stato invocato direttamentente dall'utente
                 * è quindi lecito accedere ai 'field' e ad 
                 * eventuali reference perchè sicuramente Finalize
                 * non è ancora stato chiamato su questi oggetti
                 */
                lock( this )
                {

                }
            }

            lock( syncRoot )
            {
                this.TransactionCompleted = null;
                this.TransactionEnlisted = null;
                this.enlistedTransaction = null;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose( true );
            GC.SuppressFinalize( this );
        }

        #endregion

        /// <summary>
        /// Occurs when transaction is completed.
        /// </summary>
        public event EventHandler TransactionCompleted;

        void OnTransactionCompleted()
        {
            if( this.TransactionCompleted != null )
            {
                this.TransactionCompleted( this, EventArgs.Empty );
            }
        }

        /// <summary>
        /// Occurs when the transaction has been successfully enlisted.
        /// </summary>
        public event EventHandler TransactionEnlisted;

        void OnTransactionEnlisted()
        {
            if( this.TransactionEnlisted != null )
            {
                this.TransactionEnlisted( this, EventArgs.Empty );
            }
        }

        /// <summary>
        /// Gets a value indicating whether the transaction manager is enlisted in a transaction.
        /// </summary>
        /// <value>
        ///     <c>true</c> if enlisted; otherwise, <c>false</c>.
        /// </value>
        public Boolean EnlistedInTransaction
        {
            get { return this.enlistedTransaction != null; }
        }

        private Transaction enlistedTransaction;

        /// <summary>
        /// Enlists the supplied transaction manager in the current transaction.
        /// </summary>
        /// <param name="ensureTransaction">if set to <c>true</c> forces an exception if there are no available transactions.</param>
        /// <param name="enlistmentNotification">The <c>IEnlistmentNotification</c> instance to enlist in the transaction.</param>
        /// <param name="options">The transaction enlistment options.</param>
        public void EnlistInTransaction( Boolean ensureTransaction, IEnlistmentNotification enlistmentNotification, EnlistmentOptions options  )
        {
            options.EnsureIsDefined();
            if( enlistmentNotification == null )
            {
                throw new ArgumentNullException( "enlistmentNotification" );
            }

            if( ensureTransaction && Transaction.Current == null )
            {
                throw new InvalidOperationException( "Transaction is mandatory." );
            }

            if( this.EnlistedInTransaction && this.enlistedTransaction != Transaction.Current )
            {
                //Errore siamo in una transazione diversa da quella in cui ci siamo enlisted.
                throw new NotSupportedException( "Multiple Transation detected." );
            }

            if( Transaction.Current != null )
            {
                /*
                 * Dobbiamo inserirci in una Transazione:
                 *    -    ci teniamo un riferimento alla transazione in cui andiamo ad inserirci
                 *    -    chiediamo alla transazione di inserirci all'interno del suo processo
                 */
                this.enlistedTransaction = Transaction.Current;
                this.enlistedTransaction.EnlistVolatile( enlistmentNotification, options );

                /*
                 * Ci interessa tenere traccia di quando la transazione finisce
                 * per liberare un po' di risorse
                 */
                this.enlistedTransaction.TransactionCompleted += new TransactionCompletedEventHandler( OnEnlistedTransactionTransactionCompleted );

                this.OnTransactionEnlisted();
            }
        }

        void OnEnlistedTransactionTransactionCompleted( object sender, TransactionEventArgs e )
        {
            try
            {
                this.OnTransactionCompleted();
            }
            finally
            {
                if( this.enlistedTransaction != null )
                {
                    lock( syncRoot )
                    {
                        if( this.enlistedTransaction != null )
                        {
                            //WARN: potenziale race condition da gestire  con un lock
                            this.enlistedTransaction.TransactionCompleted -= new TransactionCompletedEventHandler( OnEnlistedTransactionTransactionCompleted );
                            this.enlistedTransaction = null;
                        }
                    }
                }
            }
        }
    }
}
