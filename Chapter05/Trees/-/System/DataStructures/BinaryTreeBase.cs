using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace System.DataStructures
{
	/// <summary>
	/// Base type for common binary tree's.
	/// </summary>
	/// <typeparam name="TNode">Type of the tree node.</typeparam>
	/// <typeparam name="TValue">Type of the <see cref="BinaryTreeBase{TNode,TValue}"/>.</typeparam>
	/// <remarks><para>
	/// Examples of common binary trees: BinarySearchTree, AvlTree.
	///   </para>
	///   <para>
	/// The nodes that make up what is deemed a <see cref="BinaryTreeBase{TNode,TValue}"/> must adhere by the contract
	/// defined in <see cref="IBinaryTreeNode{TNode,TValue}"/>.
	///   </para></remarks>
	[DebuggerDisplay("Count = {Count}")]
	internal abstract class BinaryTreeBase<TNode, TValue> : ICollection<TValue>, ICollection
		where TNode : class, IBinaryTreeNode<TNode, TValue>
		where TValue : IComparable<TValue>
	{
		[NonSerialized]
		private readonly IComparer<TValue> _comparer;

		[NonSerialized]
		private TNode _root;

		[NonSerialized]
		private object _syncRoot;

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryTreeBase&lt;TNode, TValue&gt;"/> class.
		/// </summary>
		/// <remarks></remarks>
		protected BinaryTreeBase()
		{
			_comparer = Comparer<TValue>.Default;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryTreeBase&lt;TNode, TValue&gt;"/> class.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		/// <remarks></remarks>
		protected BinaryTreeBase(IComparer<TValue> comparer)
		{
			_comparer = comparer;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryTreeBase&lt;TNode, TValue&gt;"/> class.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		/// <remarks></remarks>
		protected BinaryTreeBase(Func<TValue, TValue, int> comparer)
		{
			_comparer = new FuncComparer<TValue>(comparer);
		}

		/// <summary>
		/// Gets the <see cref="IComparer{T}"/> to use for comparisons.
		/// </summary>
		/// <remarks></remarks>
		public IComparer<TValue> Comparer
		{
			get { return _comparer; }
		}

		/// <summary>
		/// Gets the number of items contained in the <see cref="ICollection{T}"/>.
		/// </summary>
		/// <value>The count.</value>
		/// <returns>
		/// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		///   </returns>
		/// <remarks></remarks>
		public int Count { get; protected set; }

		/// <summary>
		/// Gets the root of the <see cref="BinaryTreeBase{TNode,TValue}"/>.
		/// </summary>
		/// <value>The root.</value>
		/// <remarks></remarks>
		public TNode Root
		{
			get { return _root; }
			protected set { _root = value; }
		}

		/// <summary>
		/// Adds an item to the <see cref="ICollection{T}"/>.
		/// </summary>
		/// <param name="item">Item to add to collection.</param>
		/// <exception cref="T:System.NotSupportedException">
		/// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		///   </exception>
		/// <remarks></remarks>
		public abstract void Add(TValue item);

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <remarks></remarks>
		public void AddRange(IEnumerable<TValue> values)
		{
			Guard.NotNull(values, "values");
			foreach (TValue value in values)
			{
				Add(value);
			}
		}

		/// <summary>
		/// Clears all items from the <see cref="BinaryTreeBase{TNode,TValue}"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">
		/// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		///   </exception>
		/// <remarks>This method is an O(1) operation.</remarks>
		public void Clear()
		{
			_root = null;
			Count = 0;
		}

		/// <summary>
		/// Determines whether an item is contained within the <see cref="BinaryTreeBase{TNode,TValue}"/>.
		/// </summary>
		/// <param name="item">Item to search the <see cref="BinaryTreeBase{TNode,TValue}"/> for.</param>
		/// <returns>True if the item is contained within the <see cref="BinaryTreeBase{TNode,TValue}"/>; otherwise false.</returns>
		/// <remarks></remarks>
		public bool Contains(TValue item)
		{
			return Contains(_root, item);
		}

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <remarks></remarks>
		public void CopyTo(TValue[] array)
		{
			CopyTo(array, 0);
		}

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="index">The index.</param>
		/// <remarks></remarks>
		public void CopyTo(TValue[] array, int index)
		{
			Guard.NotNull(array, "array");
			Guard.GreaterThanOrEqualTo(0, index, "index");
			Guard.GreaterThanOrEqualTo(Count + index, array.Length, "array.Length");
			foreach (TValue item in GetBreadthFirstEnumerator())
			{
				array[index++] = item;
			}
		}

		/// <summary>
		/// Finds the largest value in the <see cref="BinaryTreeBase{TNode,TValue}"/>.
		/// </summary>
		/// <returns>Largest value in the <see cref="BinaryTreeBase{TNode,TValue}"/>.</returns>
		/// <exception cref="InvalidOperationException">
		/// The <see cref="BinaryTreeBase{TNode,TValue}"/> contains <strong>0</strong> items.
		///   </exception>
		/// <remarks></remarks>
		public TValue FindMax()
		{
			Guard.Against(_root == null, "Tree is empty.");
			return FindMax(_root);
		}

		/// <summary>
		/// Finds smallest value in the <see cref="BinaryTreeBase{TNode,TValue}"/>.
		/// </summary>
		/// <returns>Smallest value in the <see cref="BinaryTreeBase{TNode,TValue}"/>.</returns>
		/// <exception cref="InvalidOperationException">
		/// The <see cref="BinaryTreeBase{TNode,TValue}"/> contains <strong>0</strong> items.
		///   </exception>
		/// <remarks></remarks>
		public TValue FindMin()
		{
			Guard.Against(_root == null, "Tree is empty.");
			return FindMin(_root);
		}

		/// <summary>
		/// Finds the two values nearest to the search value, one above and one below.
		/// If an exact match is found, that value is returned in both tuple positions.
		/// </summary>
		/// <param name="searchFor">The search for.</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public Tuple<TValue, TValue> FindNearestValues(TValue searchFor)
		{
			TValue left = default(TValue);
			TValue right = default(TValue);
			IBinaryTreeNode<TNode, TValue> currentNode = _root;
			while (true)
			{
				int compareResult = searchFor.CompareTo(currentNode.Value);
				if (compareResult == 0)
				{
					return new Tuple<TValue, TValue>(currentNode.Value, currentNode.Value);
				}
				if (compareResult < 0)
				{
					right = currentNode.Value;
					if (currentNode.Left == null)
					{
						return new Tuple<TValue, TValue>(left, right);
					}
					currentNode = currentNode.Left;
				}
				else
				{
					left = currentNode.Value;
					if (currentNode.Right == null)
					{
						return new Tuple<TValue, TValue>(left, right);
					}
					currentNode = currentNode.Right;
				}
			}
		}

		/// <summary>
		/// Finds a node in the <see cref="BinaryTreeBase{TNode,TValue}"/> with the specified value.
		/// </summary>
		/// <param name="value">Value to find.</param>
		/// <returns>An instance of the correct node used for the respective tree if the node was found with the value provided;
		/// otherwise null.</returns>
		/// <remarks></remarks>
		public TNode FindNode(TValue value)
		{
			return FindNode(value, _root);
		}

		/// <summary>
		/// Finds the parent node of a node with the specified value.
		/// </summary>
		/// <param name="value">Value of node to find parent of.</param>
		/// <returns>An instance of the correct node used for the respective tree if the node was found with the value provided;
		/// otherwise null.</returns>
		/// <remarks></remarks>
		public TNode FindParent(TValue value)
		{
			if (_root == null)
			{
				return null;
			}
			return value.IsEqual(_root.Value, Comparer) ? null : FindParent(value, _root);
		}

		/// <summary>
		/// Traverses the items in the <see cref="BinaryTreeBase{TNode,TValue}"/> in breadth first order.
		/// </summary>
		/// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the
		/// <see cref="BinaryTreeBase{TNode,TValue}"/>.</returns>
		/// <remarks>This method is an O(n) operation where n is the number of nodes in the
		/// <see cref="BinaryTreeBase{TNode,TValue}"/> .</remarks>
		public IEnumerable<TValue> GetBreadthFirstEnumerator()
		{
			return BreadthFirstTraversal(_root);
		}

		/// <summary>
		/// 	<para>
		/// An <see cref="IEnumerator{T}"/> that iterates through the <see cref="BinaryTreeBase{TNode,TValue}"/>.
		///   </para>
		///   <para>
		/// The default is preorder traversal of the items in the <see cref="BinaryTreeBase{TNode,TValue}"/>.
		///   </para>
		/// </summary>
		/// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the
		/// <see cref="BinaryTreeBase{TNode,TValue}"/>.</returns>
		/// <remarks>This method is an O(n) operation.</remarks>
		public IEnumerator<TValue> GetEnumerator()
		{
			List<TValue> tempStorage = new List<TValue>();
			return PreorderTraveral(_root, tempStorage).GetEnumerator();
		}

		/// <summary>
		/// Traverses the <see cref="BinaryTreeBase{TNode,TValue}"/> in an in order traversal.
		/// </summary>
		/// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the
		/// <see cref="BinaryTreeBase{TNode,TValue}"/>.</returns>
		/// <remarks>This method is an O(n) operation where n is the number of nodes in the
		/// <see cref="BinaryTreeBase{TNode,TValue}"/>.</remarks>
		public IEnumerable<TValue> GetInorderEnumerator()
		{
			List<TValue> tempStorage = new List<TValue>();
			return InorderTraversal(_root, tempStorage);
		}

		/// <summary>
		/// Traverses the <see cref="BinaryTreeBase{TNode,TValue}"/> in a post order fashion.
		/// </summary>
		/// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the
		/// <see cref="BinaryTreeBase{TNode,TValue}"/>.</returns>
		/// <remarks>This method is an O(n) operation where n is the number of nodes in the
		/// <see cref="BinaryTreeBase{TNode,TValue}"/> .</remarks>
		public IEnumerable<TValue> GetPostorderEnumerator()
		{
			List<TValue> tempStorage = new List<TValue>();
			return PostorderTraversal(_root, tempStorage);
		}

		/// <summary>
		/// Removes the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		/// <exception cref="T:System.NotSupportedException">
		/// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		///   </exception>
		/// <remarks></remarks>
		public abstract bool Remove(TValue item);

		/// <summary>
		/// Returns the items in the <see cref="BinaryTreeBase{TNode,TValue}"/> as an <see cref="Array"/>
		/// using <see cref="BinaryTreeBase{TNode,TValue}.GetBreadthFirstEnumerator"/> traversal.
		/// </summary>
		/// <returns>A one-dimensional <see cref="Array"/> containing the items of the
		/// <see cref="BinaryTreeBase{TNode,TValue}"/>.</returns>
		/// <remarks>This method is an O(n) operation where n is the number of nodes in the
		/// <see cref="BinaryTreeBase{TNode,TValue}"/>.</remarks>
		public TValue[] ToArray()
		{
			TValue[] local = new TValue[Count];
			int i = 0;
			foreach (TValue item in GetBreadthFirstEnumerator())
			{
				local[i++] = item;
			}
			return local;
		}

		/// <summary>
		/// Determines whether an item is contained within the <see cref="BinaryTreeBase{TNode,TValue}"/>.
		/// </summary>
		/// <param name="root">The root node of the <see cref="BinaryTreeBase{TNode,TValue}"/>.</param>
		/// <param name="item">The item to be located in the <see cref="BinaryTreeBase{TNode,TValue}"/>.</param>
		/// <returns>True if the item is contained within the <see cref="BinaryTreeBase{TNode,TValue}"/>; otherwise false.</returns>
		/// <remarks></remarks>
		private bool Contains(TNode root, TValue item)
		{
			if (root == null)
			{
				// if the root is null then we have exhausted all the nodes in the tree, thus the item isn't in the bst
				return false;
			}

			if (root.Value.IsEqual(item, _comparer))
			{
				return true;
			}

			return item.IsLessThan(root.Value, _comparer) ? Contains(root.Left, item) : Contains(root.Right, item);
		}

		/// <summary>
		/// Finds a node in the <see cref="BinaryTreeBase{TNode,TValue}"/> with the specified value.
		/// </summary>
		/// <param name="value">Value to find.</param>
		/// <param name="root">Node to start the search from.</param>
		/// <returns>An instance of the correct node used for the respective tree if the node was found with the value provided;
		/// otherwise null.</returns>
		/// <remarks></remarks>
		private TNode FindNode(TValue value, TNode root)
		{
			if (root == null)
			{
				return null;
			}

			return value.IsLessThan(root.Value, Comparer)
			       	? FindNode(value, root.Left)
			       	: (value.IsGreaterThan(root.Value, Comparer) ? FindNode(value, root.Right) : root);
		}

		/// <summary>
		/// Finds the parent of a node with the specified value, starting the search from a specified node in the
		/// <see cref="BinaryTreeBase{TNode,TValue}"/>.
		/// </summary>
		/// <param name="value">Value of node to find parent of.</param>
		/// <param name="root">Node to start the search at.</param>
		/// <returns>An instance of the correct node used for the respective tree if the node was found with the value provided;
		/// otherwise null.</returns>
		/// <remarks></remarks>
		private TNode FindParent(TValue value, TNode root)
		{
			if (value.IsLessThan(root.Value, Comparer))
			{
				// check to see if the left child of root is null, if it is then the value is not in the bst
				if (root.Left == null)
				{
					return null;
				}

				return value.IsEqual(root.Left.Value, Comparer) ? root : FindParent(value, root.Left);
			}

			if (root.Right == null)
			{
				return null;
			}

			return value.IsEqual(root.Right.Value, Comparer) ? root : FindParent(value, root.Right);
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array"/> is null. </exception>
		///   
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index"/> is less than zero. </exception>
		///   
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array"/> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
		///   
		/// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
		/// <remarks></remarks>
		void ICollection.CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
		/// </summary>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
		/// <remarks></remarks>
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
		/// </summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
		/// <remarks></remarks>
		object ICollection.SyncRoot
		{
			get
			{
				if (_syncRoot == null)
				{
					Interlocked.CompareExchange(ref _syncRoot, new object(), null);
				}

				return _syncRoot;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
		///   </returns>
		/// <remarks></remarks>
		bool ICollection<TValue>.IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
		/// <remarks></remarks>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Traverse the <see cref="BinaryTreeBase{TNode,TValue}"/> in breadth first order, i.e. each node is visited on
		/// the same depth to depth n, where n is the depth of the tree.
		/// </summary>
		/// <param name="root">The root node of the <see cref="BinaryTreeBase{TNode,TValue}"/>.</param>
		/// <returns><see cref="List{T}"/> populated with the items from the traversal.</returns>
		/// <remarks></remarks>
		private static IEnumerable<TValue> BreadthFirstTraversal(TNode root)
		{
			Queue<TNode> unvisited = new Queue<TNode>();
			List<TValue> visited = new List<TValue>();

			while (root != null)
			{
				visited.Add(root.Value);
				if (root.Left != null)
				{
					unvisited.Enqueue(root.Left);
				}

				if (root.Right != null)
				{
					unvisited.Enqueue(root.Right);
				}

				root = unvisited.Count > 0 ? unvisited.Dequeue() : null;
			}

			return visited;
		}

		/// <summary>
		/// Finds the largest value in the <see cref="BinaryTreeBase{TNode,TValue}"/>.
		/// </summary>
		/// <param name="root">Root node of the <see cref="BinaryTreeBase{TNode,TValue}"/>.</param>
		/// <returns>Largest value in the <see cref="BinaryTreeBase{TNode,TValue}"/>.</returns>
		/// <remarks></remarks>
		private static TValue FindMax(TNode root)
		{
			return root.Right == null ? root.Value : FindMax(root.Right);
		}

		/// <summary>
		/// Finds the smallest value in the <see cref="BinaryTreeBase{TNode,TValue}"/>.
		/// </summary>
		/// <param name="root">Root node of the <see cref="BinaryTreeBase{TNode,TValue}"/>.</param>
		/// <returns>Smallest value in the <see cref="BinaryTreeBase{TNode,TValue}"/>.</returns>
		/// <remarks></remarks>
		private static TValue FindMin(TNode root)
		{
			return root.Left == null ? root.Value : FindMin(root.Left);
		}

		/// <summary>
		/// Traverses the <see cref="BinaryTreeBase{TNode,TValue}"/> in an in order fashion, i.e. returning the
		/// values of the nodes when a node is passed underneath.
		/// </summary>
		/// <param name="root">The root node of the <see cref="BinaryTreeBase{TNode,TValue}"/>.</param>
		/// <param name="arrayList"><see cref="List{T}"/> to store the traversed node values.</param>
		/// <returns><see cref="List{T}"/> populated with the items from the traversal.</returns>
		/// <remarks></remarks>
		private static IList<TValue> InorderTraversal(TNode root, IList<TValue> arrayList)
		{
			if (root != null)
			{
				InorderTraversal(root.Left, arrayList);
				arrayList.Add(root.Value);
				InorderTraversal(root.Right, arrayList);
			}

			return arrayList;
		}

		/// <summary>
		/// Traverses the tree in postorder, i.e. returning the values of the nodes passed on the right.
		/// </summary>
		/// <param name="root">The root node of the <see cref="BinaryTreeBase{TNode,TValue}"/>.</param>
		/// <param name="arrayList"><see cref="List{T}"/> to store the traversed node values.</param>
		/// <returns><see cref="List{T}"/> populated with the items from the traversal.</returns>
		/// <remarks></remarks>
		private static IList<TValue> PostorderTraversal(TNode root, IList<TValue> arrayList)
		{
			if (root != null)
			{
				PostorderTraversal(root.Left, arrayList);
				PostorderTraversal(root.Right, arrayList);
				arrayList.Add(root.Value);
			}

			return arrayList;
		}

		/// <summary>
		/// Traverses the tree in preorder, i.e. returning the values of the nodes passed on the left.
		/// </summary>
		/// <param name="root">The root node of the <see cref="BinaryTreeBase{TNode,TValue}"/>.</param>
		/// <param name="arrayList"><see cref="List{T}"/> to store the traversed node values.</param>
		/// <returns><see cref="List{T}"/> populated with the items from the traversal.</returns>
		/// <remarks></remarks>
		private static IList<TValue> PreorderTraveral(TNode root, IList<TValue> arrayList)
		{
			if (root != null)
			{
				arrayList.Add(root.Value);
				PreorderTraveral(root.Left, arrayList);
				PreorderTraveral(root.Right, arrayList);
			}

			return arrayList;
		}
	}
}