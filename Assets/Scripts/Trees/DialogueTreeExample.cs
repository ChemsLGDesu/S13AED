using Sirenix.OdinInspector;
using Sowtank.Collections.Trees;
using UnityEngine;

public class DialogueTreeExample : MonoBehaviour
{
    [Header("Dialogo Actual")]
    [ReadOnly]
    [ShowInInspector]
    private string currentDialogue = "Presiona 'Iniciar Dialogo'";

    [ReadOnly]
    [ShowInInspector]
    private string leftOption = "-";

    [ReadOnly]
    [ShowInInspector]
    private string rightOption = "-";

    [ReadOnly]
    [ShowInInspector]
    private bool isFinished;

    private BinaryTree<string> tree;
    private BinaryTreeNode<string> currentNode;

    //-> construir arbol e iniciar dialogo
    [Button("Iniciar Dialogo")]
    public void StartDialogue()
    {
        BuildSampleTree();
        currentNode = tree.Root;
        UpdateUI();
        isFinished = false;
        Debug.Log("--- Dialogo iniciado ---");
    }

    //-> elegir opcion izquierda (A)
    [Button("Opcion Izquierda")]
    public void ChooseLeft()
    {
        if (tree == null || currentNode == null || isFinished)
        {
            Debug.Log("Primero inicia el dialogo.");
            return;
        }
        if (currentNode.Left == null)
        {
            Debug.Log("No hay opcion izquierda.");
            return;
        }
        currentNode = currentNode.Left;
        UpdateUI();
    }

    //-> elegir opcion derecha (B)
    [Button("Opcion Derecha")]
    public void ChooseRight()
    {
        if (tree == null || currentNode == null || isFinished)
        {
            Debug.Log("Primero inicia el dialogo.");
            return;
        }
        if (currentNode.Right == null)
        {
            Debug.Log("No hay opcion derecha.");
            return;
        }
        currentNode = currentNode.Right;
        UpdateUI();
    }

    //-> reiniciar desde la raiz
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
        Debug.Log("--- Dialogo reiniciado ---");
    }

    private void UpdateUI()
    {
        if (currentNode == null)
        {
            currentDialogue = "(fin del dialogo)";
            leftOption = "-";
            rightOption = "-";
            isFinished = true;
            Debug.Log("--- Fin del dialogo ---");
            return;
        }

        currentDialogue = currentNode.Value;
        leftOption  = currentNode.Left  != null ? currentNode.Left.Value  : "(fin)";
        rightOption = currentNode.Right != null ? currentNode.Right.Value : "(fin)";

        Debug.Log($"Dialogo: {currentDialogue}");
    }

    //-> arbol de dialogo de ejemplo (7 nodos)
    //-> izquierda = opcion A, derecha = opcion B
    private void BuildSampleTree()
    {
        tree = new BinaryTree<string>();

        //            [Hola aventurero]
        //           /                  \
        //  [Como estas?]          [Quien eres?]
        //      /       \              /       \
        // [Bien!]  [Necesito ayuda] [Mago]  [Adios]

        var n00 = new BinaryTreeNode<string>("Hola aventurero!");
        var n10 = new BinaryTreeNode<string>("Como estas?");
        var n11 = new BinaryTreeNode<string>("Quien eres?");
        var n20 = new BinaryTreeNode<string>("Me alegra oirlo!");
        var n21 = new BinaryTreeNode<string>("Claro, dime que necesitas");
        var n22 = new BinaryTreeNode<string>("Soy el mago Merlin");
        var n23 = new BinaryTreeNode<string>("Hasta pronto, viajero");
        var n24 = new BinaryTreeNode<string>("Necesito una Arma Magica");
        var n25 = new BinaryTreeNode<string>("Que haces aqui, EN ESTE LUGAR");
        var n26 = new BinaryTreeNode<string>("");

        n00.Left  = n10;
        n00.Right = n11;
        n10.Left  = n20;
        n10.Right = n21;
        n11.Left  = n22;
        n11.Right = n23;
        n24.Left = n24;
        n24.Right = n24;
        tree.SetRoot( n00);
    }
}
