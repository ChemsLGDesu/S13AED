using Sirenix.OdinInspector;
using Sowtank.Collections.Trees;
using UnityEngine;

/// <summary>
/// Ejemplo didáctico de un árbol de diálogos implementado con BinaryTree.
/// 
/// Cada nodo contiene un texto de diálogo. Las ramas izquierda y derecha
/// representan dos opciones de respuesta que el jugador puede elegir.
/// 
/// Esto permite visualizar cómo un árbol binario puede modelar decisiones
/// ramificadas en un videojuego (sistema de diálogos).
/// 
/// Uso en el Inspector:
///   1. Presiona "Iniciar Diálogo"
///   2. Usa "Opción Izquierda" o "Opción Derecha" para navegar
///   3. "Reiniciar" vuelve a la raíz
/// </summary>
public class DialogueTreeExample : MonoBehaviour
{
    [Header("Diálogo Actual")]
    [ReadOnly]
    [ShowInInspector]
    private string currentDialogue = "Presiona 'Iniciar Diálogo'";

    [ReadOnly]
    [ShowInInspector]
    private string leftOption = "-";

    [ReadOnly]
    [ShowInInspector]
    private string rightOption = "-";

    [ReadOnly]
    [ShowInInspector]
    private bool isFinished;

    [ReadOnly]
    [ShowInInspector]
    private int nodeCount;

    private BinaryTree<string> tree;
    private BinaryTreeNode<string> currentNode;

    // ------------------------------------------------------------------------
    // CONSTRUCCIÓN DEL ÁRBOL DE DIÁLOGOS
    // ------------------------------------------------------------------------

    /// <summary>Construye el árbol de diálogos de ejemplo.</summary>
    [Button("Iniciar Diálogo")]
    public void StartDialogue()
    {
        BuildSampleTree();
        currentNode = tree.Root;
        UpdateUI();
        isFinished = false;
        Debug.Log("--- Diálogo iniciado ---");
    }

    /// <summary>Toma la rama izquierda (Opción A).</summary>
    [Button("Opción Izquierda")]
    public void ChooseLeft()
    {
        if (CanChoose())
        {
            currentNode = currentNode.Left;
            UpdateUI();
        }
    }

    /// <summary>Toma la rama derecha (Opción B).</summary>
    [Button("Opción Derecha")]
    public void ChooseRight()
    {
        if (CanChoose())
        {
            currentNode = currentNode.Right;
            UpdateUI();
        }
    }

    /// <summary>Reinicia el diálogo desde la raíz.</summary>
    [Button("Reiniciar")]
    public void ResetDialogue()
    {
        if (tree == null || tree.IsEmpty)
        {
            BuildSampleTree();
        }
        currentNode = tree.Root;
        UpdateUI();
        isFinished = false;
        Debug.Log("--- Diálogo reiniciado ---");
    }

    // ------------------------------------------------------------------------
    // INTERNA
    // ------------------------------------------------------------------------

    private bool CanChoose()
    {
        if (tree == null || currentNode == null)
        {
            Debug.LogWarning("Primero inicia el diálogo con 'Start Dialogue'.");
            return false;
        }
        if (isFinished)
        {
            Debug.Log("El diálogo ha terminado. Presiona 'Reiniciar'.");
            return false;
        }
        return true;
    }

    /// <summary>Actualiza los campos del Inspector según el nodo actual.</summary>
    private void UpdateUI()
    {
        if (currentNode == null)
        {
            currentDialogue = "(fin del diálogo)";
            leftOption = "-";
            rightOption = "-";
            isFinished = true;
            Debug.Log("--- Fin del diálogo ---");
            return;
        }

        currentDialogue = currentNode.Value;
        leftOption  = currentNode.Left  != null ? currentNode.Left.Value  : "(fin)";
        rightOption = currentNode.Right != null ? currentNode.Right.Value : "(fin)";
        nodeCount   = tree.Size();

        Debug.Log($"Diálogo: {currentDialogue}");
    }

    /// <summary>Construye un árbol de diálogo de ejemplo con 7 nodos.</summary>
    private void BuildSampleTree()
    {
        tree = new BinaryTree<string>();

        //            [¡Hola aventurero!]
        //           /                    \
        //  [¿Cómo estás?]          [¿Quién eres?]
        //      /       \              /       \
        // [¡Bien!]  [Necesito ayuda] [Mago]  [Adiós]
        //
        // Izquierda = opción A, Derecha = opción B

        var n00 = new BinaryTreeNode<string>("¡Hola aventurero!");
        var n10 = new BinaryTreeNode<string>("¿Cómo estás?");
        var n11 = new BinaryTreeNode<string>("¿Quién eres?");
        var n20 = new BinaryTreeNode<string>("¡Me alegra oírlo!");
        var n21 = new BinaryTreeNode<string>("Claro, dime qué necesitas");
        var n22 = new BinaryTreeNode<string>("Soy el mago Merlín");
        var n23 = new BinaryTreeNode<string>("Hasta pronto, viajero");

        n00.Left  = n10;
        n00.Right = n11;
        n10.Left  = n20;
        n10.Right = n21;
        n11.Left  = n22;
        n11.Right = n23;

        tree.SetRoot(n00);
    }
}
