namespace System.DataStructures
{
	/// <summary>
	/// Interface for the nodes that are used in a <see cref="BinaryTreeBase{TNode,TValue}"/>.
	/// </summary>
	/// <typeparam name="TNode">Type of the node.</typeparam>
	/// <typeparam name="TValue">Type of the value.</typeparam>
	/// <remarks></remarks>
	internal interface IBinaryTreeNode<TNode, TValue>
	{
		/// <summary>
		/// Gets or sets the left node reference.
		/// </summary>
		/// <value>The left.</value>
		/// <remarks></remarks>
		TNode Left { get; set; }

		/// <summary>
		/// Gets or sets the right node reference.
		/// </summary>
		/// <value>The right.</value>
		/// <remarks></remarks>
		TNode Right { get; set; }

		/// <summary>
		/// Gets or sets the value of the <see cref="IBinaryTreeNode{TNode,TValue}"/>.
		/// </summary>
		/// <value>The value.</value>
		/// <remarks></remarks>
		TValue Value { get; set; }
	}

	/// <summary>
	/// Node used by a binary tree.
	/// </summary>
	/// <typeparam name="T">Type of the node.</typeparam>
	/// <remarks></remarks>
	internal class BinaryTreeNode<T> : IBinaryTreeNode<BinaryTreeNode<T>, T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryTreeNode{T}"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <remarks></remarks>
		public BinaryTreeNode(T value) {
			Value = value;
		}

		/// <summary>
		/// Gets or sets the left node reference.
		/// </summary>
		/// <value>The left.</value>
		/// <remarks></remarks>
		public BinaryTreeNode<T> Left { get; set; }

		/// <summary>
		/// Gets or sets the right node reference.
		/// </summary>
		/// <value>The right.</value>
		/// <remarks></remarks>
		public BinaryTreeNode<T> Right { get; set; }

		/// <summary>
		/// Gets or sets the value of the node.
		/// </summary>
		/// <value>The value.</value>
		/// <remarks></remarks>
		public T Value { get; set; }
	}
}