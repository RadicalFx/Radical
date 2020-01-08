namespace Radical.Model
{
    using Radical.ComponentModel;
    using Radical.Linq;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// The Indexer class is primarily an Index manager for the View,
    /// its primary target is to handle the relationship between an
    /// EntityItemView instance and the Index of the encapsulated T 
    /// element in the DataSource.
    /// </summary>
    /// <typeparam name="T">The type of the element encapsulated by the EntityView.</typeparam>
    public sealed class Indexer<T> :
        IEnumerable,
        ICollection<IEntityItemView<T>>
    {
        readonly EntityView<T> view;
        readonly List<IEntityItemView<T>> storage = new List<IEntityItemView<T>>();

        /*
         * L'indice di default, gestisce la relazione tra l'EntityItemView e l'indice 
         * dell'elemento T nella DataSource, serve anche per sapere se un elemento è
         * in stato di "Pending Add", nel qual caso ha un Value pari a -1.
         * Un elemento è in stato di Pending Add nel caso in cui sia stato chiamato AddNew
         * sulla View e non sia ancora stato chiamato EndNew, o CancelNew. In questo caso 
         * l'elemento esiste solo nella View e non ancora nella DataSource sottostante, 
         * attualmente siamo in grado di gestire un solo elemento alla volta in stato di 
         * "Pending Add", ma questa è una limitazione della View e non dell'indice che 
         * sarebbe già ora in grado di gestirne 'n'.
         */
        readonly Dictionary<IEntityItemView<T>, int> defaultIndex = new Dictionary<IEntityItemView<T>, int>();
        readonly Dictionary<string, Dictionary<object, IEntityItemView<T>>> propertiesIndexes = new Dictionary<string, Dictionary<object, IEntityItemView<T>>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Indexer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        internal Indexer(EntityView<T> view)
        {
            this.view = view;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<IEntityItemView<T>> IEnumerable<IEntityItemView<T>>.GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        /// <summary>
        /// Gets a item indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <item></item>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(IEntityItemView<T> item)
        {
            int index = storage.IndexOf(item);
            if (index > -1)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            /*
             * Svuotiamo lo storage interno
             */
            storage.Clear();

            /*
             * Svuotiamo l'indice di default
             */
            defaultIndex.Clear();

            if (propertiesIndexes.Count > 0)
            {
                /*
                 * Se ci sono proprietà indicizzate
                 * svuotiamo anche questi indici, non li
                 * eliminamo perchè altrimenti non sapremmo
                 * come reindicizzarli.
                 * 
                 * Questo significa che una colonna indicizzata
                 * è da rimuovere esplictamente
                 */
                foreach (var index in propertiesIndexes)
                {
                    index.Value.Clear();
                }
            }
        }

        /// <summary>
        /// Removes the occurrence of the EntityItemView at the specified index
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            IEntityItemView<T> item = storage[index];
            if (defaultIndex.ContainsKey(item))
            {
                defaultIndex.Remove(item);
            }

            storage.RemoveAt(index);
        }

        /// <summary>
        /// Copies the elements of the ICollection to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from ICollection. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public void CopyTo(IEntityItemView<T>[] array, int arrayIndex)
        {
            storage.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <item></item>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
        public int Count
        {
            get { return storage.Count; }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific item.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains(IEntityItemView<T> item)
        {
            return storage.Contains(item);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific item.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        /// <summary>
        /// Gets the <see cref="Radical.Model.EntityItemView&lt;T&gt;"/> at the specified index.
        /// </summary>
        /// <item></item>
        public IEntityItemView<T> this[int index]
        {
            get { return storage[index]; }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(IEntityItemView<T> item)
        {
            storage.Add(item);

            /*
             * In fase di insert di un nuovo elemento
             * aggiungiamo l'elemento anche al nostro
             * indice, l'item lo recuperiamo dalla
             * List che fa da storage per la View.
             * 
             * Se l'elemento che stiamo inserendo è un nuovo elemento
             * questo non sarà ancora presente nella List quindi giustamente
             * viene inserito con Value a -1
             */
            defaultIndex.Add(item, view.DataSource.IndexOf(item.EntityItem));
        }

        /// <summary>
        /// Given a T object this method return the index
        /// of the EntityItemView, that encapsulates the T
        /// object, in the View.
        /// </summary>
        /// <param name="item">The item to find the index for.</param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            /*
             * Le performance di questo potrebbero essere
             * drasticamente migliorate se gestissimo anche
             * un'indicizzazione tra T e la posizione dello
             * EntityItemView che lo incapsula utilizzando
             * un altro Dictionary<T, int> dove T è il DataItem
             * mentre l'int è l'indice dell'EntityItemView che
             * lo incapsula
             */
            var x = (from el in storage
                     where el.EntityItem.Equals(item)
                     select el).FirstOrDefault<IEntityItemView<T>>();

            if (x != null)
            {
                return IndexOf(x);
            }

            return -1;
        }

        /// <summary>
        /// Return the index of an EntityItemView in the View.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public int IndexOf(IEntityItemView<T> item)
        {
            return storage.IndexOf(item);
        }


        /// <summary>
        /// Given the Index of an EntityItemView, in the View, returns the Index
        /// of the encapsulated T element in the DataSource, otherwise -1.
        /// </summary>
        /// <param name="objectItemViewIndexInView">The EntityItemView index.</param>
        /// <returns>The index of the T element, otherwise -1</returns>
        public int FindObjectIndexInDataSource(int objectItemViewIndexInView)
        {
            if (objectItemViewIndexInView > -1 && objectItemViewIndexInView < storage.Count)
            {
                IEntityItemView<T> el = storage[objectItemViewIndexInView];
                if (el != null)
                {
                    return defaultIndex[el];
                }
            }

            return -1;
        }

        /// <summary>
        /// Given the Index of a T element, in the DataSource, returns the
        /// Index of the EntityItemView, in the View, that encapsulates the
        /// T element, otherwise -1.
        /// </summary>
        /// <param name="entityIndexInDataSource">The index of the T element in DataSource.</param>
        /// <returns>The Index of the EntityItemView, otherwise -1</returns>
        public int FindEntityItemViewIndexInView(int entityIndexInDataSource)
        {
            if (entityIndexInDataSource > -1 && entityIndexInDataSource < view.DataSource.Count)
            {
                T obj = (T)view.DataSource[entityIndexInDataSource];
                return IndexOf(obj);
            }

            return -1;
        }

        /// <summary>
        /// Adds the given property descriptor to the index.
        /// </summary>
        /// <param name="property">The property to index.</param>
        public void AddIndex(PropertyDescriptor property)
        {
            if (!propertiesIndexes.ContainsKey(property.Name))
            {
                /*
                 * Se non abbiamo già indicizzato quella proprietà
                 * lo facciamo adesso, creiamo un nuovo Dictionary
                 */
                Dictionary<object, IEntityItemView<T>> newIndex = new Dictionary<object, IEntityItemView<T>>(Count);
                IndexProperty(property, newIndex);
                propertiesIndexes.Add(property.Name, newIndex);
            }
        }

        void IndexProperty(PropertyDescriptor property, Dictionary<object, IEntityItemView<T>> index)
        {
            foreach (EntityItemView<T> oiv in this)
            {
                /*
                 * Scorriamo tutti gli elementi presenti e per ogni elememnto
                 * facciamo un'associazione tra il valore della proprietà che
                 * vogliamo indicizzare e l'elemento
                 * 
                 * Il problema è che questa cosa funziona solo se la proprietà
                 * è una chiave univoca all'interno della lista altrimenti ciccia
                 * e direi che mi sembra un comportamento corretto
                 * 
                 * Non ha inoltre senso supportare l'eventuale cambiamento
                 * di valore delle chiavi perchè se così fosse non sarebbe
                 * una chiave...
                 */
                index.Add(property.GetValue(oiv.EntityItem), oiv);
            }
        }

        /// <summary>
        /// Finds the given key searching on all items for the specified property.
        /// If the property is an indexed property the index is used for faster searching.
        /// </summary>
        /// <param name="property">The property to look at.</param>
        /// <param name="key">The key to search.</param>
        /// <returns>The index of the first item the match the given key.</returns>
        public int Find(PropertyDescriptor property, object key)
        {
            int index = -1;

            if (propertiesIndexes.ContainsKey(property.Name))
            {
                /*
                 * Se la proprietà è indicizzata facciamo fare all'indice
                 */
                Dictionary<object, IEntityItemView<T>> propertyIndex = propertiesIndexes[property.Name];
                if (propertyIndex.ContainsKey(key))
                {
                    IEntityItemView<T> item = propertyIndex[key];
                    index = IndexOf(item);
                }
            }
            else
            {
                /*
                 * Altrimenti ci tocca scorrere tutta la lista,
                 * recuperare via reflection il valore e confrontarlo
                 * 
                 * Utilizziamo Object.Equals() e non == perchè se i tipi sono
                 * ValueType vengono boxati e == confronterebe le reference
                 */
                var item = this.Where(element => Equals(property.GetValue(element.EntityItem), key)).FirstOrDefault();
                if (item != null)
                {
                    index = IndexOf(item);
                }
            }

            return index;
        }

        /// <summary>
        /// Removes the given PropertyDescriptor from the indexed properties.
        /// </summary>
        /// <param name="property">The property to remove.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public void RemoveIndex(PropertyDescriptor property)
        {
            if (propertiesIndexes.ContainsKey(property.Name))
            {
                propertiesIndexes.Remove(property.Name);
            }
        }

        /// <summary>
        /// Clears the built indexes.
        /// </summary>
        public void ClearIndexes()
        {
            if (propertiesIndexes.Count > 0)
            {
                propertiesIndexes.ForEach(kvp => { kvp.Value.Clear(); });
                propertiesIndexes.Clear();
            }
        }



        /// <summary>
        /// Rebuilds all the Indexes.
        /// </summary>
        public void Rebuild()
        {
            IEnumerable<KeyValuePair<IEntityItemView<T>, int>> pendingAddElements = null;

            if (Count > 0)
            {
                /*
                 * Dato che verranno ricreati da zero tutti gli elementi
                 * presenti nella EntityView, sganciamo i gestori degli
                 * eventi degli elementi attualmente presenti.
                 */
                storage.ForEach(element => { view.OnUnwireEntityItemView(element); });

                /*
                 * Dato che è possibile che ci siano elementi nuovi
                 * non ancora inseriti nella DataSource sottostante
                 * dobbiamo tenerne traccia prima di rimuovere tutti
                 * gli elementi presenti, quindi aggiungiamo gli elementi
                 * non presenti nella DataSource sottostante ad una cache 
                 * temporanea. Gli elementi non presenti sono quelli che
                 * nel defaultIndex hanno un Value pari a -1
                 * Il "ToList()" è necessario per scatenare subito il deferred
                 * loading altrimenti ci perdiamo i pezzi per strada con la clear.
                 */
                pendingAddElements = defaultIndex.Where(element => element.Value == -1).AsReadOnly();

                /*
                 * Svuotiamo questa istanza
                 */
                Clear();
            }

            /*
             * Creiamo una lista, di tuple (Anonymous Type), che tiene traccia
             * di tutti gli elementi nella DataSource che devono essere reinsiriti e del
             * loro indice relativo alla DataSource, se un elemento è in stato
             * PendingAdd non sarà ancora presente nella DataSource quindi correttamente
             * verrà aggiunto con Value a -1
             */
            var sourceElements = ((IEnumerable<T>)view.DataSource)
                .Where(element => view.Filter.ShouldInclude(element))
                .Select(element => new { Element = (T)element, Index = view.DataSource.IndexOf(element) });

            /*
             * Scorriamo tutti gli elementi trovati, creiamo un nuovo
             * EntityItemView agganciamo gli eventi che ci interessano
             * e lo reinseriamo nell'indice
             */
            sourceElements.ForEach(sourceElement =>
            {
                IEntityItemView<T> item = view.CreateEntityItemView(sourceElement.Element);
                view.OnWireEntityItemView(item);

                Add(item);
            });

            sourceElements = null;

            if (pendingAddElements != null && pendingAddElements.Any())
            {
                /*
                 * Riaggiungiamo gli elementi che erano in pending add
                 */
                pendingAddElements.ForEach(element =>
                {
                    IEntityItemView<T> item = element.Key;

                    view.OnWireEntityItemView(item);
                    Add(item);
                });
            }

            pendingAddElements = null;

            ApplySort();

            if (propertiesIndexes.Count > 0)
            {
                /*
                 * Ci sono proprietà indicizzate
                 * dobbiamo ricostruire anche questi
                 * indici
                 */
                propertiesIndexes.ForEach(index =>
                {
                    /*
                     * Ricostruiamo il PropertyDescriptor
                     * e reindicizziamo la proprietà
                     */
                    PropertyDescriptor property = TypeDescriptor.GetProperties(typeof(T)).Find(index.Key, false);
                    IndexProperty(property, index.Value);
                });
            }
        }

        /// <summary>
        /// Applies the current sort.
        /// </summary>
        public void ApplySort()
        {
            /*
             * Se abbiamo impostato un sort, riapplichiamo il Sort
             */
            if (view.IsSorted)
            {
                storage.Sort(view.SortComparer);
            }
        }

        /// <summary>
        /// Removes the sort applied to the Index.
        /// </summary>
        public void RemoveSort()
        {
            /*
             * Dobbiamo rimuovere il sort e riportarlo allo stato 
             * orginale che è quello della DataSource, l'unica nota
             * è che l'Indice potrebbe essere "filtrato"
             */
            var comparer = new DefaultEntityItemViewSortComparer<T>(view.DataSource);
            storage.Sort(comparer);
        }
    }
}