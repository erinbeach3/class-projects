using System;
using System.Text;

namespace DataStructures
{
	public class Node<T>
	{
		public static void WalkInOrder(Node<T> node, Action<Node<T>, int> action)
		{
			Action<Node<T>, int> walk = null;
			walk = (n, d) =>
			{
				if (n.HasLeft) walk(n.Left, d + 1);
				action(n, d);
				if (n.HasRight) walk(n.Right, d + 1);
			};
			if (node != null) walk(node, 0);
		}

		public static void WalkPreOrder(Node<T> node, Action<Node<T>, int> action)
		{
			Action<Node<T>, int> walk = null;
			walk = (n, d) =>
			{
				action(n, d);
				if (n.HasLeft) walk(n.Left, d + 1);
				if (n.HasRight) walk(n.Right, d + 1);
			};
			if (node != null) walk(node, 0);
		}

		public static Node<T> DeepClone(Node<T> root)
		{
			if (root == null) throw new ArgumentNullException(nameof(root));
			if (!root.IsRoot) throw new ArgumentException($"{nameof(root)} must be a root node.");
			return root.Clone();
		}

		public Node(T value)
		{
			Value = value;
		}

		public Node(Node<T> parent, T value)
		{
			Parent = parent;
			Value = value;
		}

		public Node(Node<T> parent, T value, Node<T> left, Node<T> right)
		{
			Parent = parent;
			Value = value;
			Left = left;
			if (HasLeft) Left.Parent = this;
			Right = right;
			if (HasRight) Right.Parent = this;
		}

		public Node<T> Parent { get; internal set; }
		public T Value { get; internal set; }
		public Node<T> Left { get; internal set; }
		public Node<T> Right { get; internal set; }
		public bool HasParent => Parent != null;
		public bool HasLeft => Left != null;
		public bool HasRight => Right != null;
		public bool HasChild => HasLeft || HasRight;
		public bool IsRoot => !HasParent;
		public bool IsLeft => Parent != null && Object.ReferenceEquals(this, Parent.Left);
		public bool IsRight => Parent != null && Object.ReferenceEquals(this, Parent.Right);

		/// <summary>
		/// Get this node's root node.
		/// </summary>
		public Node<T> RootNode
		{
			get
			{
				Node<T> n = this;
				while (n.HasParent) n = n.Parent;
				return n;
			}
		}

		/// <summary>
		/// Use this in any way that is helpful.
		/// </summary>
		public object Tag { get; set; }

		/// <summary>
		/// Get this node's depth in the tree.  Depth is zero-based (the root node has depth of 0).
		/// </summary>
		public int Depth
		{
			get
			{
				if (Parent == null) return 0;
				return 1 + Parent.Depth;
			}
		}

		/// <summary>
		/// Get the # of levels below this node
		/// </summary>
		public int Height
		{
			get
			{
				int l = HasLeft ? Left.Height : 0;
				int r = HasRight ? Right.Height : 0;
				return 1 + Math.Max(l, r);
			}
		}

		public int Imbalance
		{
			get
			{
				int l = HasLeft ? Left.Height : 0,
					r = HasRight ? Right.Height : 0;
				return Math.Abs(l - r);
			}
		}

		public string PathToNode
		{
			get
			{
				StringBuilder s = new StringBuilder();
				Node<T> n = this;
				while (n.Parent != null)
				{
					if (n.IsLeft) s.Append("L"); else s.Append("R");
					n = n.Parent;
				}
				return s.ToString();
			}
		}

		public ulong PositionFromLeft
		{
			get
			{
				ulong p = 0;
				int d = Depth, dmax = d;
				Node<T> n = this;
				while (n.HasParent)
				{
					if (n.IsRight)
					{
						p += (ulong)Math.Pow(2, dmax - d);
					}
					d--;
					n = n.Parent;
				}
				return p;
			}
		}

		public int SideFromRoot
		{
			get
			{
				if (!HasParent) return 0;
				Node<T> n = this;
				while (n.Parent.Parent != null) n = n.Parent;
				return n.IsLeft ? 1 : -1;
			}
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public override bool Equals(object obj)
		{
			Node<T> n = obj as Node<T>;
			if (n == null) return false;
			if (!Value.Equals(n.Value)) return false;
			if (HasLeft != n.HasLeft) return false;
			if (HasLeft && !Left.Equals(n.Left)) return false;
			if (HasRight != n.HasRight) return false;
			if (HasRight && !Right.Equals(n.Right)) return false;
			return true;

		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		internal void EnsureParents()
		{
			if (HasLeft)
			{
				Left.Parent = this;
				Left.EnsureParents();
			}
			if (HasRight)
			{
				Right.Parent = this;
				Right.EnsureParents();
			}
		}

		private Node<T> FindRoot()
		{
			Node<T> n = this;
			while (n.Parent != null) n = n.Parent;
			return n;
		}

		private Node<T> Clone()
		{
			Node<T> r = new Node<T>(Value);
			if (HasLeft) r.Left = Left.Clone();
			if (HasRight) r.Right = Right.Clone();
			return r;
		}
	}
}
