using Sirenix.OdinInspector;
using Sowtank.Collections.Trees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTreeVisualizer : MonoBehaviour
{
    public GraphVisualNode nodePrefab;

    [Header("Layout")]
    public float levelHeight = 2.5f;
    public float initialSpread = 6f;

    [Header("Datos")]
    public List<string> values = new() { "A", "B", "C", "D", "E", "F", "G" };

    [Header("Animacion")]
    public bool animate = true;
    public float stepDelay = 0.6f;

    [ReadOnly]
    [ShowInInspector]
    private string traversalResult = "";

    private BinaryTree<string> tree;
    private Dictionary<BinaryTreeNode<string>, GraphVisualNode> nodeMap = new();

    //-> construir arbol desde la lista
    [Button("Construir Arbol")]
    public void BuildTree()
    {
        StopAllCoroutines();
        ClearVisuals();

        tree = new BinaryTree<string>();
        tree.BuildFromList(values);

        RestoreColors();
        PlaceNodes(tree.Root, Vector3.zero, initialSpread);
        DrawAllConnections(tree.Root);
        traversalResult = "";
    }

    //-> recorridos
    [Button("InOrder (Izq → Raiz → Der)")]
    public void StartInOrder()      => StartCoroutine(AnimateTraversal("InOrder", tree.InOrderNodes));

    [Button("PreOrder (Raiz → Izq → Der)")]
    public void StartPreOrder()     => StartCoroutine(AnimateTraversal("PreOrder", tree.PreOrderNodes));

    [Button("PostOrder (Izq → Der → Raiz)")]
    public void StartPostOrder()    => StartCoroutine(AnimateTraversal("PostOrder", tree.PostOrderNodes));

    [Button("LevelOrder (BFS)")]
    public void StartLevelOrder()   => StartCoroutine(AnimateTraversal("LevelOrder", tree.LevelOrderNodes));

    [Button("Limpiar")]
    public void Clear()
    {
        StopAllCoroutines();
        ClearVisuals();
        tree = null;
        traversalResult = "";
    }

    //-> anima el recorrido paso a paso
    private IEnumerator AnimateTraversal(string label, System.Action<System.Action<BinaryTreeNode<string>>> traversal)
    {
        if (tree == null || tree.IsEmpty)
        {
            Debug.LogWarning("Construye el arbol primero.");
            yield break;
        }

        RestoreColors();
        traversalResult = "";

        //-> ejecutar recorrido para determinar orden
        var visited = new List<BinaryTreeNode<string>>();
        traversal(node =>
        {
            visited.Add(node);
            traversalResult += node.Value + " ";
        });

        Debug.Log($"{label}: {traversalResult.Trim()}");

        if (!animate) yield break;

        //-> resaltar cada nodo en orden
        foreach (var node in visited)
        {
            if (nodeMap.TryGetValue(node, out var visual))
                visual.NodeName.color = Color.green;
            yield return new WaitForSeconds(stepDelay);
        }

        yield return new WaitForSeconds(0.5f);
        RestoreColors();
    }

    //-> posicionar nodos visuales recursivamente
    private void PlaceNodes(BinaryTreeNode<string> node, Vector3 pos, float spread)
    {
        if (node == null) return;

        GraphVisualNode visual = Instantiate(nodePrefab, pos, Quaternion.identity, transform);
        visual.Set(node.Value);
        nodeMap[node] = visual;

        float nextSpread = spread / 2f;
        float nextY = pos.y - levelHeight;

        PlaceNodes(node.Left,  new Vector3(pos.x - spread, nextY, 0), nextSpread);
        PlaceNodes(node.Right, new Vector3(pos.x + spread, nextY, 0), nextSpread);
    }

    //-> dibujar conexiones padre-hijo
    private void DrawAllConnections(BinaryTreeNode<string> node)
    {
        if (node == null || !nodeMap.ContainsKey(node)) return;

        GraphVisualNode parentVisual = nodeMap[node];

        if (node.Left  != null && nodeMap.ContainsKey(node.Left))
            parentVisual.AddNeighbor(nodeMap[node.Left]);

        if (node.Right != null && nodeMap.ContainsKey(node.Right))
            parentVisual.AddNeighbor(nodeMap[node.Right]);

        parentVisual.DrawConections();

        DrawAllConnections(node.Left);
        DrawAllConnections(node.Right);
    }

    private void ClearVisuals()
    {
        foreach (var v in nodeMap.Values)
        {
            if (v == null) continue;
            v.ClearLines();
            Destroy(v.gameObject);
        }
        nodeMap.Clear();
    }
    private void RestoreColors()
    {
        foreach (var v in nodeMap.Values)
            if (v != null) v.NodeName.color = Color.white;
    }
}
