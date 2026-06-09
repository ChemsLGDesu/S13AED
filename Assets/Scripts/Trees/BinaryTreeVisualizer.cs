using Sirenix.OdinInspector;
using Sowtank.Collections.Trees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTreeVisualizer : MonoBehaviour
{
    [Header("Prefabs")]
    public GraphVisualNode nodePrefab;

    [Header("Layout")]
    public float levelHeight = 2.5f;
    public float initialSpread = 6f;

    [Header("Datos del Árbol")]
    public List<string> values = new() { "A", "B", "C", "D", "E", "F", "G" };

    [Header("Animación")]
    [Tooltip("Si está activo, resalta cada nodo paso a paso durante el recorrido.")]
    public bool animate = true;

    [Tooltip("Segundos de espera entre cada nodo durante la animación.")]
    public float stepDelay = 0.6f;

    [Header("Resultado")]
    [ReadOnly]
    [ShowInInspector]
    private string traversalResult = "";

    private BinaryTree<string> tree;
    private Dictionary<BinaryTreeNode<string>, GraphVisualNode> nodeMap = new();

    // ------------------------------------------------------------------------
    // CONSTRUCCIÓN
    // ------------------------------------------------------------------------

    /// <summary>Construye el árbol a partir de la lista de valores (inserción por niveles).</summary>
    [Button("Construir Árbol")]
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

    // ------------------------------------------------------------------------
    // RECORRIDOS
    // ------------------------------------------------------------------------

    [Button("InOrder (Izq → Raíz → Der)")]
    public void StartInOrder()
    {
        if (EnsureTree()) StartCoroutine(AnimateTraversal("InOrder", tree.InOrderNodes));
    }

    [Button("PreOrder (Raíz → Izq → Der)")]
    public void StartPreOrder()
    {
        if (EnsureTree()) StartCoroutine(AnimateTraversal("PreOrder", tree.PreOrderNodes));
    }

    [Button("PostOrder (Izq → Der → Raíz)")]
    public void StartPostOrder()
    {
        if (EnsureTree()) StartCoroutine(AnimateTraversal("PostOrder", tree.PostOrderNodes));
    }

    [Button("LevelOrder (BFS por niveles)")]
    public void StartLevelOrder()
    {
        if (EnsureTree()) StartCoroutine(AnimateTraversal("LevelOrder", tree.LevelOrderNodes));
    }

    [Button("Limpiar")]
    public void Clear()
    {
        StopAllCoroutines();
        ClearVisuals();
        tree = null;
        traversalResult = "";
    }

    // ------------------------------------------------------------------------
    // ANIMACIÓN
    // ------------------------------------------------------------------------

    /// <summary>
    /// Ejecuta el recorrido indicado, acumula los nodos visitados y luego
    /// los resalta uno por uno si la animación está activa.
    /// </summary>
    private IEnumerator AnimateTraversal(string label, System.Action<System.Action<BinaryTreeNode<string>>> traversal)
    {
        RestoreColors();
        traversalResult = "";

        // 1. Ejecutar el recorrido para determinar el orden de visita
        var visited = new List<BinaryTreeNode<string>>();
        traversal(node =>
        {
            visited.Add(node);
            traversalResult += node.Value + " ";
        });

        Debug.Log($"{label}: {traversalResult.Trim()}");

        // 2. Si no hay animación, terminamos
        if (!animate) yield break;

        // 3. Animar: resaltar cada nodo en el orden visitado
        foreach (var node in visited)
        {
            if (nodeMap.TryGetValue(node, out var visual))
                visual.NodeName.color = Color.green;
            yield return new WaitForSeconds(stepDelay);
        }

        // 4. Restaurar colores tras un momento
        yield return new WaitForSeconds(0.5f);
        RestoreColors();
    }

    // ------------------------------------------------------------------------
    // LAYOUT DEL ÁRBOL
    // ------------------------------------------------------------------------

    /// <summary>Ubica los nodos visuales recursivamente (layout de árbol binario clásico).</summary>
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

    /// <summary>Dibuja las conexiones (aristas) entre padres e hijos.</summary>
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

    private bool EnsureTree()
    {
        if (tree == null || tree.IsEmpty)
        {
            Debug.LogWarning("Primero construye el árbol con 'Build Tree'.");
            return false;
        }
        return true;
    }
}
