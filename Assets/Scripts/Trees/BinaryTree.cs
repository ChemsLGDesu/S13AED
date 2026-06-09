using System;
using System.Collections.Generic;

namespace Sowtank.Collections.Trees
{
    /// <summary>
    /// Árbol binario genérico de construcción manual.
    /// A diferencia de un BST, este árbol no impone una propiedad de orden.
    /// Los hijos se asignan directamente sobre los nodos (node.Left / node.Right),
    /// lo que permite modelar árboles de expresión, árboles de decisión, diálogos, etc.
    /// 
    /// Incluye los recorridos clásicos: InOrder, PreOrder, PostOrder y LevelOrder,
    /// además de métodos para calcular altura, tamaño e imprimir la estructura.
    /// </summary>
    /// <typeparam name="T">Tipo del valor almacenado en los nodos.</typeparam>
    public class BinaryTree<T>
    {
        private BinaryTreeNode<T> root;

        /// <summary>Raíz del árbol (null si está vacío).</summary>
        public BinaryTreeNode<T> Root
        {
            get => root;
            set => root = value;
        }

        /// <summary>Crea un árbol vacío.</summary>
        public BinaryTree() { }

        /// <summary>Crea un árbol con la raíz especificada.</summary>
        /// <param name="root">Nodo raíz inicial.</param>
        public BinaryTree(BinaryTreeNode<T> root) => this.root = root;

        /// <summary>Asigna la raíz desde un nodo existente.</summary>
        public void SetRoot(BinaryTreeNode<T> node) => root = node;

        /// <summary>Crea un nuevo nodo raíz con el valor dado.</summary>
        public void SetRoot(T value) => root = new BinaryTreeNode<T>(value);

        /// <summary>Elimina todos los nodos del árbol.</summary>
        public void Clear() => root = null;

        /// <summary>Indica si el árbol está vacío (sin nodos).</summary>
        public bool IsEmpty => root == null;

        /// <summary>
        /// Construye un árbol binario completo (o casi completo) insertando valores
        /// por niveles, de izquierda a derecha (level-order insertion).
        /// Útil para crear árboles de prueba rápidamente desde una lista.
        /// </summary>
        /// <param name="values">Lista de valores a insertar.</param>
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

        /// <summary>Retorna la altura del árbol (-1 si está vacío).</summary>
        public int Height() => HeightRecursive(root);

        private int HeightRecursive(BinaryTreeNode<T> node)
        {
            if (node == null) return -1;
            return 1 + Math.Max(HeightRecursive(node.Left), HeightRecursive(node.Right));
        }

        /// <summary>Retorna la cantidad total de nodos en el árbol.</summary>
        public int Size() => SizeRecursive(root);

        private int SizeRecursive(BinaryTreeNode<T> node)
        {
            if (node == null) return 0;
            return 1 + SizeRecursive(node.Left) + SizeRecursive(node.Right);
        }

        // ========================================================================
        // RECORRIDOS (Traversals)
        // ========================================================================

        /// <summary>
        /// Recorrido InOrder: subárbol izquierdo → raíz → subárbol derecho.
        /// En un BST produce los valores ordenados ascendentemente.
        /// </summary>
        /// <param name="action">Acción a ejecutar con cada valor visitado.</param>
        public void InOrder(Action<T> action) => InOrderRecursive(root, action);

        private void InOrderRecursive(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node == null) return;
            InOrderRecursive(node.Left, action);
            action?.Invoke(node.Value);
            InOrderRecursive(node.Right, action);
        }

        /// <summary>
        /// InOrder que expone los nodos completos en lugar de solo los valores.
        /// Útil para visualización o modificación de nodos.
        /// </summary>
        public void InOrderNodes(Action<BinaryTreeNode<T>> action) => InOrderNodesRecursive(root, action);

        private void InOrderNodesRecursive(BinaryTreeNode<T> node, Action<BinaryTreeNode<T>> action)
        {
            if (node == null) return;
            InOrderNodesRecursive(node.Left, action);
            action?.Invoke(node);
            InOrderNodesRecursive(node.Right, action);
        }

        /// <summary>
        /// Recorrido PreOrder: raíz → subárbol izquierdo → subárbol derecho.
        /// Útil para copiar o serializar un árbol.
        /// </summary>
        /// <param name="action">Acción a ejecutar con cada valor visitado.</param>
        public void PreOrder(Action<T> action) => PreOrderRecursive(root, action);

        private void PreOrderRecursive(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node == null) return;
            action?.Invoke(node.Value);
            PreOrderRecursive(node.Left, action);
            PreOrderRecursive(node.Right, action);
        }

        /// <summary>PreOrder exponiendo los nodos completos.</summary>
        public void PreOrderNodes(Action<BinaryTreeNode<T>> action) => PreOrderNodesRecursive(root, action);

        private void PreOrderNodesRecursive(BinaryTreeNode<T> node, Action<BinaryTreeNode<T>> action)
        {
            if (node == null) return;
            action?.Invoke(node);
            PreOrderNodesRecursive(node.Left, action);
            PreOrderNodesRecursive(node.Right, action);
        }

        /// <summary>
        /// Recorrido PostOrder: subárbol izquierdo → subárbol derecho → raíz.
        /// Útil para eliminar el árbol (primero se eliminan los hijos).
        /// </summary>
        /// <param name="action">Acción a ejecutar con cada valor visitado.</param>
        public void PostOrder(Action<T> action) => PostOrderRecursive(root, action);

        private void PostOrderRecursive(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node == null) return;
            PostOrderRecursive(node.Left, action);
            PostOrderRecursive(node.Right, action);
            action?.Invoke(node.Value);
        }

        /// <summary>PostOrder exponiendo los nodos completos.</summary>
        public void PostOrderNodes(Action<BinaryTreeNode<T>> action) => PostOrderNodesRecursive(root, action);

        private void PostOrderNodesRecursive(BinaryTreeNode<T> node, Action<BinaryTreeNode<T>> action)
        {
            if (node == null) return;
            PostOrderNodesRecursive(node.Left, action);
            PostOrderNodesRecursive(node.Right, action);
            action?.Invoke(node);
        }

        /// <summary>
        /// Recorrido por niveles (Level Order / BFS):
        /// visita los nodos nivel por nivel, de izquierda a derecha usando una cola.
        /// </summary>
        /// <param name="action">Acción a ejecutar con cada valor visitado.</param>
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

        /// <summary>LevelOrder exponiendo los nodos completos.</summary>
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

        // ========================================================================
        // MÉTODOS DE IMPRESIÓN (Debug)
        // ========================================================================

        /// <summary>Retorna el recorrido InOrder como string.</summary>
        public string GetInOrder()
        {
            var result = "";
            InOrder(v => result += v + " ");
            return result.Trim();
        }

        /// <summary>Retorna el recorrido PreOrder como string.</summary>
        public string GetPreOrder()
        {
            var result = "";
            PreOrder(v => result += v + " ");
            return result.Trim();
        }

        /// <summary>Retorna el recorrido PostOrder como string.</summary>
        public string GetPostOrder()
        {
            var result = "";
            PostOrder(v => result += v + " ");
            return result.Trim();
        }

        /// <summary>Retorna el recorrido por niveles como string.</summary>
        public string GetLevelOrder()
        {
            var result = "";
            LevelOrder(v => result += v + " ");
            return result.Trim();
        }

        /// <summary>
        /// Imprime el árbol en la consola de Unity con formato jerárquico.
        /// La raíz aparece a la izquierda y las ramas se extienden hacia la derecha.
        /// </summary>
        public void PrintTree()
        {
            if (root == null)
            {
                UnityEngine.Debug.Log("(árbol vacío)");
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
