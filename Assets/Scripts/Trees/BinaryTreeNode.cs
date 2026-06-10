namespace Sowtank.Collections.Trees
{
    public class BinaryTreeNode<T>//->nodo de arbol binario
    {
        public T Value;
        public BinaryTreeNode<T> Left;
        public BinaryTreeNode<T> Right;

        public BinaryTreeNode(T value)
        {
            Value = value;
        }

        public bool IsLeaf => Left == null && Right == null;//->no tiene hijos

    }
}
