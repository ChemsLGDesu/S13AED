using Sirenix.OdinInspector;
using Sowtank.Collections.Graphs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphVisualizer : MonoBehaviour
{
    public GraphVisualNode nodePrefab;

    public float spacing = 4f;

    public List<GraphVisualNode> visualNode;

    void Start()
    {
        
    }
   /* public void BuildVisual<T>(OrientedGraph<T> graph)
    {

   
    }*/
    public void DrawVisual<T>(NonOrientedGraph<T> graph)
    {
        StartCoroutine(DrawStepByStepBFS(graph));
    }
    public IEnumerator DrawStepByStepBFS<T>(NonOrientedGraph<T> graph)
    {
        List<Node<T>> visitedNodes = new();

        graph.BFS(graph.Nodes.FirstOrDefault(), current =>
        {
            visitedNodes.Add(current);  
        });

        for (int i = 0; i < visitedNodes.Count; i++)
        {
            Debug.Log(visitedNodes[i].Value);

            Vector3 position = new Vector3
            (
                i * spacing,
                0,
                UnityEngine.Random.Range(-spacing, spacing)
            );
            GraphVisualNode visual = Instantiate(nodePrefab, position, Quaternion.identity);
            visual.Set(visitedNodes[i].Value.ToString());
            visualNode.Add(visual);

            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < visitedNodes.Count; i++)
        {
            Node<T> currentNode = visitedNodes[i];

            GraphVisualNode currentVisual = visualNode[i];

            foreach (var neighbor in currentNode.Neighbors)
            {
                int neighborIndex = visitedNodes.IndexOf(neighbor);

                if (neighborIndex == -1)
                    continue;

                GraphVisualNode neighborVisual =
                    visualNode[neighborIndex];

                currentVisual.AddNeighbor(neighborVisual);
            }
        }
        yield return new WaitForSeconds(1f);

        foreach (var Node in visualNode)
        {
            Node.DrawConections();

            yield return new WaitForSeconds(1f);
        }


        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
