using System;
using System.Collections.Generic;
using NHibernate.Collection;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Collection;

namespace NHibernate.Type
{
	/// <summary>
	/// An <see cref="IType"/> that maps an <see cref="IList{T}"/> collection
	/// using bag semantics with an identifier to the database.
	/// </summary>
	[Serializable]
	public class GenericIdentifierBagType<T> : CollectionType
	{
		/// <summary>
		/// Initializes a new instance of a <see cref="CollectionType"/> class for
		/// a specific role.
		/// </summary>
		/// <param name="role">The role the persistent collection is in.</param>
		/// <param name="propertyRef">The name of the property in the
		/// owner object containing the collection ID, or <see langword="null" /> if it is
		/// the primary key.</param>
		public GenericIdentifierBagType(string role, string propertyRef)
			: base(role, propertyRef, false)
		{
		}

		/// <summary>
		/// Instantiates a new <see cref="IPersistentCollection"/> for the identifier bag.
		/// </summary>
		/// <param name="session">The current <see cref="ISessionImplementor"/> for the identifier bag.</param>
		/// <param name="persister"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public override IPersistentCollection Instantiate(ISessionImplementor session, ICollectionPersister persister, object key)
		{
			return new PersistentIdentifierBag<T>(session);
		}

		/// <summary>
		/// Wraps an <see cref="IList{T}"/> in a <see cref="PersistentIdentifierBag{T}"/>.
		/// </summary>
		/// <param name="session">The <see cref="ISessionImplementor"/> for the collection to be a part of.</param>
		/// <param name="collection">The unwrapped <see cref="IList{T}"/>.</param>
		/// <returns>
		/// An <see cref="PersistentIdentifierBag{T}"/> that wraps the non NHibernate <see cref="IList{T}"/>.
		/// </returns>
		public override IPersistentCollection Wrap(ISessionImplementor session, object collection)
		{
			return new PersistentIdentifierBag<T>(session, (ICollection<T>) collection);
		}

		/// <summary></summary>
		public override System.Type ReturnedClass
		{
			get { return typeof(IList<T>); }
		}

		/// <summary> 
		/// Instantiate an empty instance of the "underlying" collection (not a wrapper),
		/// but with the given anticipated size (i.e. accounting for initial capacity
		/// and perhaps load factor).
		/// </summary>
		/// <param name="anticipatedSize">
		/// The anticipated size of the instantiated collection after we are done populating it.
		/// </param>
		/// <returns> A newly instantiated collection to be wrapped. </returns>
		public override object Instantiate(int anticipatedSize)
		{
			return anticipatedSize <= 0 ? new List<T>() : new List<T>(anticipatedSize + 1);
		}


		protected override void Clear(object collection)
		{
			((IList<T>)collection).Clear();
		}

		protected override void Add(object collection, object element)
		{
			((IList<T>)collection).Add((T)element);
		}
	}
}
