namespace Sowtank.Collections.Trees
{
    public class BinaryTreeNode<T>
    {
        public T Value;
        public BinaryTreeNode<T> Left;
        public BinaryTreeNode<T> Right;

        public BinaryTreeNode(T value)
        {
            Value = value;
        }
    }
}
