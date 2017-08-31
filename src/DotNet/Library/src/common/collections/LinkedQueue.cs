// 
// General:
//      This file is part of .NET Bridge
//
// Copyright:
//      2010 Jonathan Shore
//      2017 Jonathan Shore and Contributors
//
// License:
//      Licensed under the Apache License, Version 2.0 (the "License");
//      you may not use this file except in compliance with the License.
//      You may obtain a copy of the License at:
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//      Unless required by applicable law or agreed to in writing, software
//      distributed under the License is distributed on an "AS IS" BASIS,
//      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//      See the License for the specific language governing permissions and
//      limitations under the License.
//

using System;
using System.Collections.Generic;
using bridge.common.system;
using System.Text;
using System.Threading;

namespace bridge.common.collections
{
	/// <summary>
	/// Queue that uses exposed nodes, allowing for lower-level manipulation
	/// </summary>
	public class LinkedQueue<Node> : IEnumerable<Node> where Node : LinkedNode<Node>, new()
	{		
		// Properties
		
		public Node Front
			{ get { return _front; } }
		
		public Node Back
			{ get { return _back; } }
		
		public int Count
			{ get { return _count; } }
		
		
		// Operations
		
		
		/// <summary>
		/// Append value node to list
		/// </summary>
		/// <param name='node'>
		/// Node.
		/// </param>
		public virtual Node Append (Node node)
		{
			if (_back != null)
			{
				node.Prior = _back;
				_back.Next = node;
				_back = node;
			}
			else
			{
				_back = node;
				_front = node;
			}
			
			_count++;
			return node;
		}
		
		
		/// <summary>
		/// Append new node to list and return ref
		/// </summary>
		public virtual Node Append ()
		{
			return Append (NodePool<Node>.Alloc());
		}
		
		
		/// <summary>
		/// Prepend the specified node.
		/// </summary>
		/// <param name='node'>
		/// Node.
		/// </param>
		public virtual Node Prepend (Node node)
		{
			if (_front != null)
			{
				node.Next = _front;
				_front.Prior = node;
				_front = node;
			}
			else
			{
				_back = node;
				_front = node;
			}
			
			_count++;
			return node;
		}


		/// <summary>
		/// Move given node to front of queue
		/// </summary>
		/// <param name='node'>
		/// Node.
		/// </param>
		public virtual void MoveToFront (Node node)
		{
			if (_front == node)
				return;
			if (_front == null)
				throw new ArgumentException ("cannot move a node in the queue when does not belong to the queue");

			if (node == _back)
				_back = node.Prior;
			if (node.Prior != null)
				node.Prior.Next = node.Next;
			if (node.Next != null)
				node.Next.Prior = node.Prior;

			node.Next = _front;
			_front.Prior = node;
			_front = node;
		}


		/// <summary>
		/// Move given node to back of queue
		/// </summary>
		/// <param name='node'>
		/// Node.
		/// </param>
		public virtual void MoveToBack (Node node)
		{
			if (_back == node)
				return;
			if (_back == null)
				throw new ArgumentException ("cannot move a node in the queue when does not belong to the queue");

			if (node == _front)
				_front = node.Next;
			if (node.Prior != null)
				node.Prior.Next = node.Next;
			if (node.Next != null)
				node.Next.Prior = node.Prior;

			node.Next = _front;
			_front.Prior = node;
			_front = node;
		}

		
		/// <summary>
		/// Prepend the a new node and return ref
		/// </summary>
		public virtual Node Prepend ()
		{
			return Prepend (NodePool<Node>.Alloc());
		}	
		
		
		/// <summary>
		/// insert node after given
		/// </summary>
		/// <param name='prior'>
		/// Node to insert after
		/// </param>
		/// <param name='newnode'>
		/// New node to insert
		/// </param>
		public virtual Node Insert (Node prior, Node newnode)
		{
			var next = prior.Next;
			
			newnode.Next = next;
			newnode.Prior = prior;
			prior.Next = newnode;
	
			if (_back == prior)
				_back = newnode;
			
			if (next != null)
				next.Prior = newnode;
			
			_count++;
			return newnode;
		}
		
		
		/// <summary>
		/// insert new node after given prior node, returning ref to new node
		/// </summary>
		/// <param name='prior'>
		/// Node to insert after
		/// </param>
		public virtual Node Insert (Node prior)
		{
			return Insert (prior, NodePool<Node>.Alloc());
		}
		
		
		/// <summary>
		/// Remove the specified node 
		/// <p/>
		/// Note that the node is invalidated after this call, in fact may be allocated to a new use
		/// </summary>
		/// <param name='node'>
		/// node to be removed
		/// </param>
		public virtual bool Remove (Node node)
		{
			bool removed = false;
			if (node == _front)
				{ _front = node.Next; removed = true; }
			if (node == _back)
				{ _back = node.Prior; removed = true; }
			
			if (node.Prior != null)
				{ node.Prior.Next = node.Next; removed = true; }
			if (node.Next != null)
				{ node.Next.Prior = node.Prior; removed = true; }
			
			node.Prior = null;
			node.Next = null;
			
			if (removed)
			{
				_count--;
				NodePool<Node>.Free (node);
			}
			
			return removed;
		}
				
		
		/// <summary>
		/// Clear list
		/// <p/>
		/// Note that all nodes are invalidated after this call, in fact may be allocated to a new use
		/// </summary>
		public virtual void Clear ()
		{
			for (Node node = _front ; node != null ; )
			{
				var next = node.Next;
				NodePool<Node>.Free (node);					
				node = next;
			}
			
			_front = null;
			_back = null;
			_count = 0;
		}
	
		
		// Meta
		
		
		public System.Collections.IEnumerator GetEnumerator ()
		{
			for (Node  node = _front ; node != null ; node = node.Next)
				yield return node;
		}
		
		IEnumerator<Node> IEnumerable<Node>.GetEnumerator ()
		{
			for (Node node = _front ; node != null ; node = node.Next)
				yield return node;
		}

		public override string ToString ()
		{
			StringBuilder s = new StringBuilder(1024);
			s.Append("[");
			
			int i = 0;
			for (Node node = _front ; node != null ; node = node.Next)
			{
				if (i++ > 0)
					s.Append(", ");
				
				s.Append(node.ToString());
			}
			
			return s.ToString();			 
		}
		
		// Variables
		
		private Node 		_front;
		private Node 		_back;
		private int			_count;
	}
	
	
	
	/// <summary>
	/// OrderInfo pool allocator
	/// </summary>
	public static class NodePool<Node> where Node : LinkedNode<Node>, new()
	{
		/// <summary>
		/// Create an node
		/// </summary>
		public static Node Alloc ()
		{
			var allocator = _global.Value;
			var node = allocator.Alloc ();
			return node;
		}
		
		
		/// <summary>
		/// Release a node
		/// </summary>
		/// <param name='node'>
		/// node to be released.
		/// </param>
		public static void Free (Node node)
		{
			node.Prior = null;
			node.Next = null;	
			
			var allocator = _global.Value;
			allocator.Free (node);
		}

				
		// Variables
		
		static ThreadLocal<STObjectPool<Node>>
			_global = new ThreadLocal<STObjectPool<Node>> (
				() => new STObjectPool<Node>(1024, () => new Node()));
	}
	
}

