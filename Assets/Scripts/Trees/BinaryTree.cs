using System;
using System.Collections.Generic;

namespace Sowtank.Collections.Trees
{
    public class BinaryTree<T>//->arbol binario generico (no ordenado como el BST)
    {
        private BinaryTreeNode<T> root;

        public BinaryTreeNode<T> Root => root;
        public bool IsEmpty => root == null;

        //-> construir desde lista (insercion por niveles)

        public void SetRoot(BinaryTreeNode<T> node)
        {
            root = node;
        }
        public void BuildFromList(List<T> values)
        {
            if (values == null || values.Count == 0)
            {
                root = null;
                return;
            }

            root = new BinaryTreeNode<T>(values[0]);
            var queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(root);
            int i = 1;

            while (i < values.Count)
            {
                var current = queue.Dequeue();

                if (i < values.Count)
                {
                    current.Left = new BinaryTreeNode<T>(values[i]);
                    queue.Enqueue(current.Left);
                    i++;
                }

                if (i < values.Count)
                {
                    current.Right = new BinaryTreeNode<T>(values[i]);
                    queue.Enqueue(current.Right);
                    i++;
                }
            }
        }

        //-> altura del arbol (-1 si vacio)
        public int Height() => HeightRecursive(root);

        private int HeightRecursive(BinaryTreeNode<T> node)
        {
            if (node == null) return -1;
            return 1 + Math.Max(HeightRecursive(node.Left), HeightRecursive(node.Right));
        }

        //-> cantidad de nodos
        public int Size() => SizeRecursive(root);

        private int SizeRecursive(BinaryTreeNode<T> node)
        {
            if (node == null) return 0;
            return 1 + SizeRecursive(node.Left) + SizeRecursive(node.Right);
        }

        //==================================================================
        // RECORRIDOS
        //==================================================================

        //-> InOrder: izquierda → raiz → derecha
        public void InOrder(Action<T> action) => InOrderRecursive(root, action);
        public void InOrderNodes(Action<BinaryTreeNode<T>> action) => InOrderNodesRecursive(root, action);

        private void InOrderRecursive(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node == null) return;
            InOrderRecursive(node.Left, action);
            action?.Invoke(node.Value);
            InOrderRecursive(node.Right, action);
        }
        private void InOrderNodesRecursive(BinaryTreeNode<T> node, Action<BinaryTreeNode<T>> action)
        {
            if (node == null) return;
            InOrderNodesRecursive(node.Left, action);
            action?.Invoke(node);
            InOrderNodesRecursive(node.Right, action);
        }

        //-> PreOrder: raiz → izquierda → derecha
        public void PreOrder(Action<T> action) => PreOrderRecursive(root, action);
        public void PreOrderNodes(Action<BinaryTreeNode<T>> action) => PreOrderNodesRecursive(root, action);

        private void PreOrderRecursive(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node == null) return;
            action?.Invoke(node.Value);
            PreOrderRecursive(node.Left, action);
            PreOrderRecursive(node.Right, action);
        }
        private void PreOrderNodesRecursive(BinaryTreeNode<T> node, Action<BinaryTreeNode<T>> action)
        {
            if (node == null) return;
            action?.Invoke(node);
            PreOrderNodesRecursive(node.Left, action);
            PreOrderNodesRecursive(node.Right, action);
        }

        //-> PostOrder: izquierda → derecha → raiz
        public void PostOrder(Action<T> action) => PostOrderRecursive(root, action);
        public void PostOrderNodes(Action<BinaryTreeNode<T>> action) => PostOrderNodesRecursive(root, action);

        private void PostOrderRecursive(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node == null) return;
            PostOrderRecursive(node.Left, action);
            PostOrderRecursive(node.Right, action);
            action?.Invoke(node.Value);
        }
        private void PostOrderNodesRecursive(BinaryTreeNode<T> node, Action<BinaryTreeNode<T>> action)
        {
            if (node == null) return;
            PostOrderNodesRecursive(node.Left, action);
            PostOrderNodesRecursive(node.Right, action);
            action?.Invoke(node);
        }

        //-> LevelOrder / BFS: nivel por nivel usando cola
        public void LevelOrder(Action<T> action)
        {
            if (root == null) return;
            var queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                action?.Invoke(current.Value);
                if (current.Left  != null) queue.Enqueue(current.Left);
                if (current.Right != null) queue.Enqueue(current.Right);
            }
        }
        public void LevelOrderNodes(Action<BinaryTreeNode<T>> action)
        {
            if (root == null) return;
            var queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                action?.Invoke(current);
                if (current.Left  != null) queue.Enqueue(current.Left);
                if (current.Right != null) queue.Enqueue(current.Right);
            }
        }

        //==================================================================
        // METODOS DE IMPRESION
        //==================================================================

        public string GetInOrder()
        {
            string result = "";
            InOrder(v => result += v + " ");
            return result.Trim();
        }
        public string GetPreOrder()
        {
            string result = "";
            PreOrder(v => result += v + " ");
            return result.Trim();
        }
        public string GetPostOrder()
        {
            string result = "";
            PostOrder(v => result += v + " ");
            return result.Trim();
        }
        public string GetLevelOrder()
        {
            string result = "";
            LevelOrder(v => result += v + " ");
            return result.Trim();
        }

        public void PrintTree()//-> imprime jerarquia del arbol
        {
            if (root == null)
            {
                UnityEngine.Debug.Log("(arbol vacio)");
                return;
            }
            PrintTreeRecursive(root, "", true);
        }
        private void PrintTreeRecursive(BinaryTreeNode<T> node, string indent, bool isLast)
        {
            if (node == null) return;
            UnityEngine.Debug.Log(indent + (isLast ? "└── " : "├── ") + node.Value);
            indent += isLast ? "    " : "│   ";
            PrintTreeRecursive(node.Left, indent, node.Right == null);
            PrintTreeRecursive(node.Right, indent, true);
        }
    }
}
