using System;
using System.Collections.Generic;
using UnityEngine;
using Sowtank.Collections;

namespace Sowtank.Collections.Graphs
{
    public class OrientedGraph<T>
    {
        private List<Node<T>> nodes = new();

        //-> añadir un nodo
        public Node<T> AddNode(T value)
        {
            Node<T> newNode = new Node<T>(value);
            nodes.Add(newNode);

            return newNode;
        }
        //-> remover un nodo
        public void RemoveNode(Node<T> node)
        {
            nodes.Remove(node);

            foreach (var other in nodes)
            {
                other.DisconnectBidirectional(node);
            }
        }
        public void AddEdges(Node<T> a, Node<T> b)
        {
            a.ConnectUniDirectional(b);
        }
        public void AddEdges(int posA, int posB)
        {
            nodes[posA].ConnectUniDirectional(nodes[posB]);
        }
        public void DeleteEdges(Node<T> a, Node<T> b)
        {
            a.DisconnectUnidirectional(b);
        }

        public void PrintAdjancencyList()//-> cuadratica
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                string nodeList = "Node Nro : " + nodes[i].Value.ToString() + " =>";

                //-> i => posoicion
                for (var j = 0; j < nodes[i].Neighbors.Count; j++)
                {
                    //-> j cada vecino de i

                    nodeList += nodes[i].Neighbors[j].Value.ToString() + ", ";
                }

                Debug.Log(nodeList);

            }
        }

        public void PrintAdjacencyMatrix()
        {
            Debug.Log("Matriz de adyacencia");
            string context = "    ";

            for (var i = 0; i < nodes.Count; i++)
            {
                context += i + "     ";
            }
            context += "\n";
            for (var i = 0; i < nodes.Count; i++)
            {
                context += i + "    ";//\n
                for (var j = 0; j < nodes.Count; j++)
                {
                    context += nodes[i].Neighbors.Contains(nodes[j]) ? "  1  " : "  0  ";
                }
                context += "\n";
            }
            Debug.Log(context);
        }

        public void BFS(Node<T> startNode , Action<Node<T>> action)//->colas
        {
            List<Node<T>> visited = new List<Node<T>>(); //-> los que ya revise
            MyQueue<Node<T>> queue = new MyQueue<Node<T>>();    //-> los que estoy revisando

            queue.Enqueue(startNode);
            visited.Add(startNode);

            while(queue.Count > 0)
            {
                Node<T> current = queue.Dequeue();
                action?.Invoke(current);

                foreach (var neighbor in current.Neighbors)
                {
                    if(!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }
        public void DFS(Node<T> startNode, Action<Node<T>> action)//-> similar pila
        {
            List<Node<T>> visited = new List<Node<T>>(); //-> los que ya revise
            DFSRecursive(startNode, visited, action);
        }
        public void DFSRecursive(Node<T> current, List<Node<T>> visited, Action<Node<T>> action)
        {
            if (current == null)
                return;

            visited.Add(current);
            action?.Invoke(current);

            foreach (var neighbor in current.Neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    DFSRecursive(neighbor, visited, action);
                }
            }
        }




    }
}
