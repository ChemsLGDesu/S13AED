using Sirenix.OdinInspector;
using Sowtank.Collections.Trees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSTVisualizer : MonoBehaviour
{
    [Header("Prefabs")]
    public GraphVisualNode nodePrefab;

    [Header("Layout")]
    public float levelHeight = 2.5f;
    public float initialSpread = 6f;

    [Header("Data")]
    public List<int> values = new() { 5, 3, 7, 1, 4, 6, 9 };
    [SerializeField] private int singleValue;

    private BinarySearchTree<int> bst;
    private Dictionary<BinaryTreeNode<int>, GraphVisualNode> visuals = new();

    // Construye el arbol insertando los valores de la lista uno por uno con animacion
    [Button("Build Tree")]
    public void BuildTree()
    {
        StopAllCoroutines();
        ClearVisuals();
        bst = new BinarySearchTree<int>();
        StartCoroutine(InsertSequentially());
    }

    // Inserta un valor individual en caliente (util para demos en clase)
    [Button("Insert Single")]
    public void InsertSingle()
    {
        if (bst == null) bst = new BinarySearchTree<int>();
        bst.Insert(singleValue);
        ClearVisuals();
        PlaceNodes(bst.Root, Vector3.zero, initialSpread);
        DrawAllConnections(bst.Root);
    }

    [Button("Clear")]
    public void Clear()
    {
        StopAllCoroutines();
        ClearVisuals();
        bst = null;
    }

    private IEnumerator InsertSequentially()
    {
        foreach (int v in values)
        {
            bst.Insert(v);

            // Rebuild posiciones cada vez para reflejar el arbol actualizado
            ClearVisuals();
            PlaceNodes(bst.Root, Vector3.zero, initialSpread);
            DrawAllConnections(bst.Root);

            yield return new WaitForSeconds(1f);
        }
    }

    // Posiciona recursivamente: raiz en pos, hijos separados por spread a cada lado
    // El spread se divide a la mitad en cada nivel -> layout de arbol binario clasico
    private void PlaceNodes(BinaryTreeNode<int> node, Vector3 pos, float spread)
    {
        if (node == null) return;

        GraphVisualNode visual = Instantiate(nodePrefab, pos, Quaternion.identity, transform);
        visual.Set(node.Value.ToString());
        visuals[node] = visual;

        float nextSpread = spread / 2f;
        float nextY      = pos.y - levelHeight;

        PlaceNodes(node.Left,  new Vector3(pos.x - spread, nextY, 0), nextSpread);
        PlaceNodes(node.Right, new Vector3(pos.x + spread, nextY, 0), nextSpread);
    }

    // Reutiliza GraphVisualNode.AddNeighbor + DrawConections para dibujar las aristas
    private void DrawAllConnections(BinaryTreeNode<int> node)
    {
        if (node == null || !visuals.ContainsKey(node)) return;

        GraphVisualNode parentVisual = visuals[node];

        if (node.Left  != null && visuals.ContainsKey(node.Left))
            parentVisual.AddNeighbor(visuals[node.Left]);

        if (node.Right != null && visuals.ContainsKey(node.Right))
            parentVisual.AddNeighbor(visuals[node.Right]);

        parentVisual.DrawConections();

        DrawAllConnections(node.Left);
        DrawAllConnections(node.Right);
    }

    private void ClearVisuals()
    {
        foreach (var v in visuals.Values)
        {
            if (v == null) continue;
            v.ClearLines();
            Destroy(v.gameObject);
        }
        visuals.Clear();
    }
}
