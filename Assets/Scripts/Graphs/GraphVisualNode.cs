using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GraphVisualNode : MonoBehaviour
{
    public TextMeshPro NodeName;
    public LineRenderer linePrefab;

    public List<GraphVisualNode> Neighbors;

    public List<LineRenderer> lines;

    public float step;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Set(string name)
    {
        NodeName.text = name;
    }
    public void AddNeighbor(GraphVisualNode obj)
    {
        Neighbors.Add(obj);
    }
    public void DrawConections()
    {
        ClearLines();
        StartCoroutine(DrawConectionsCorr());
    }
    public IEnumerator DrawConectionsCorr()
    {
        foreach (var neighbor in Neighbors)
        {
            LineRenderer line =
            Instantiate(linePrefab);

            line.positionCount = 2;

            line.SetPosition(0, transform.position);

            line.SetPosition(1, neighbor.transform.position);

            lines.Add(line);
            yield return new WaitForSeconds(0.4f);
        }

        yield return null;
    }

    public void ClearLines()
    {
        foreach (var line in lines)
        {
            Destroy(line);
        }
        lines.Clear();
    }
}
