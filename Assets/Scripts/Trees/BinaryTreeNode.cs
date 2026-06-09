using System;

namespace Sowtank.Collections.Trees
{
    /// <summary>
    /// Representa un nodo de un árbol binario.
    /// Cada nodo almacena un valor y referencias a sus hijos izquierdo y derecho.
    /// </summary>
    /// <typeparam name="T">Tipo del valor almacenado en el nodo.</typeparam>
    public class BinaryTreeNode<T>
    {
        /// <summary>Valor contenido en el nodo.</summary>
        public T Value { get; set; }

        /// <summary>Subárbol izquierdo.</summary>
        public BinaryTreeNode<T> Left { get; set; }

        /// <summary>Subárbol derecho.</summary>
        public BinaryTreeNode<T> Right { get; set; }

        /// <summary>Indica si el nodo es hoja (no tiene hijos).</summary>
        public bool IsLeaf => Left == null && Right == null;

        /// <summary>Crea un nodo con el valor especificado.</summary>
        /// <param name="value">Valor a almacenar.</param>
        public BinaryTreeNode(T value)
        {
            Value = value;
        }

        public override string ToString() => Value?.ToString() ?? "null";
    }
}
