using System;
using UnityEngine;

namespace Sowtank.Collections.Trees
{
    public class BinarySearchTree<T> where T : IComparable<T>
    {
        private BinaryTreeNode<T> root;

        // Insert mantiene la propiedad: izquierda < nodo <= derecha
        public void Insert(T value)
        {
            root = InsertRecursive(root, value);
        }

        private BinaryTreeNode<T> InsertRecursive(BinaryTreeNode<T> current, T value)
        {
            if (current == null)
                return new BinaryTreeNode<T>(value);

            int cmp = value.CompareTo(current.Value);

            if (cmp < 0)
                current.Left = InsertRecursive(current.Left, value);
            else
                current.Right = InsertRecursive(current.Right, value);

            return current;
        }

        public bool Contains(T value)
        {
            return SearchRecursive(root, value) != null;
        }

        public BinaryTreeNode<T> Search(T value)
        {
            return SearchRecursive(root, value);
        }

        private BinaryTreeNode<T> SearchRecursive(BinaryTreeNode<T> current, T value)
        {
            if (current == null) return null;

            int cmp = value.CompareTo(current.Value);

            if (cmp == 0) return current;
            if (cmp < 0)  return SearchRecursive(current.Left, value);
            return             SearchRecursive(current.Right, value);
        }

        // InOrder: izquierda -> raíz -> derecha  (produce valores en orden ascendente)
        public void InOrder(Action<T> action)   => InOrderRecursive(root, action);

        private void InOrderRecursive(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node == null) return;
            InOrderRecursive(node.Left, action);
            action?.Invoke(node.Value);
            InOrderRecursive(node.Right, action);
        }

        // PreOrder: raíz -> izquierda -> derecha  (útil para copiar/serializar el árbol)
        public void PreOrder(Action<T> action)  => PreOrderRecursive(root, action);

        private void PreOrderRecursive(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node == null) return;
            action?.Invoke(node.Value);
            PreOrderRecursive(node.Left, action);
            PreOrderRecursive(node.Right, action);
        }

        // PostOrder: izquierda -> derecha -> raíz  (útil para eliminar el árbol)
        public void PostOrder(Action<T> action) => PostOrderRecursive(root, action);

        private void PostOrderRecursive(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node == null) return;
            PostOrderRecursive(node.Left, action);
            PostOrderRecursive(node.Right, action);
            action?.Invoke(node.Value);
        }

        public void PrintInOrder()
        {
            string result = "InOrder: ";
            InOrder(v => result += v + " ");
            Debug.Log(result);
        }

        public BinaryTreeNode<T> Root => root;
    }
}
