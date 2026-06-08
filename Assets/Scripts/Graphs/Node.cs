using System.Collections.Generic;
using UnityEngine;


namespace Sowtank.Collections.Graphs
{
    public class Node<T>//->vertices
    {
        private T value;
        private List<Node<T>> neighbors = new();

        public Node(T value)
        {
            this.value = value;
        }

        public void ConnectBidirectional(Node<T> node)//->bidireccional
        {
            if (!neighbors.Contains(node))
            {
                neighbors.Add(node);
            }
            node.neighbors.Add(this);
        }
        public void ConnectUniDirectional(Node<T> node)
        {
            if (!neighbors.Contains(node))
                neighbors.Add(node);
        }
        public void DisconnectBidirectional(Node<T> node)//> bidireccional
        {
            if (neighbors.Contains(node))
                neighbors.Remove(node);

            node.neighbors.Remove(this);
        }
        public void DisconnectUnidirectional(Node<T> node)//> bidireccional
        {
            if (neighbors.Contains(node))
                neighbors.Remove(node);

        }

        public T Value => value;
        public List<Node<T>> Neighbors => neighbors;
    }
}

