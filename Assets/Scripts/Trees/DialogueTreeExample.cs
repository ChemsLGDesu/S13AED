using Sirenix.OdinInspector;
using Sowtank.Collections.Trees;
using UnityEngine;
public struct Dialog
{
    public string Dialogo;
    public string Opcion1;
    public string Opcion2;
    public Dialog(string value , string op1, string op2)
    {
        Dialogo = value;
        Opcion1 = op1;
        Opcion2 = op2;
    }
}
public class DialogueTreeExample : MonoBehaviour
{
    [Header("Dialogo Actual")]
    [ReadOnly]
    [FoldoutGroup("Respuesta") , ShowInInspector]
    private string currentDialogue = "Presiona 'Iniciar Dialogo'";


    [ReadOnly]
    [FoldoutGroup("Right"), ShowInInspector]
    private string rightOption = "-";
    [ReadOnly]
    [FoldoutGroup("Left"), ShowInInspector]
    private string leftOption = "-";

    [ReadOnly]
    [ShowInInspector]
    private bool isFinished;

    private BinaryTree<Dialog> tree;
    private BinaryTreeNode<Dialog> currentNode;

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


    [FoldoutGroup("Left"),Button("Opcion Izquierda")]
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
    [FoldoutGroup("Right") ,Button("Opcion Derecha")]
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

        currentDialogue = currentNode.Value.Dialogo;
        leftOption  =  currentNode.Value.Opcion1  ;
        rightOption = currentNode.Value.Opcion2;

        Debug.Log($"Dialogo: {currentDialogue}");
    }

    //-> arbol de dialogo de ejemplo (7 nodos)
    //-> izquierda = opcion A, derecha = opcion B
    private void BuildSampleTree()
    {
        tree = new BinaryTree<Dialog>();

        //            [Hola aventurero]
        //           /                  \
        //  [Como estas?]          [Quien eres?]
        //      /       \              /       \
        // [Bien!]  [Necesito ayuda] [Mago]  [Adios]
        var t00 = new BinaryTreeNode<Dialog>(new("Hola aventurero!", "Como estas?" , "Quien eres?"));

        var t10 = new BinaryTreeNode<Dialog>(new("Bien gracias por preguntar :D ", " Tienes una cara muy rara >:l ", "Eres una persona muy elegante"));

        var t21 = new BinaryTreeNode<Dialog>(new("Insolente Preparate Para Morir", " [PELEAR] ", "[ESCAPAR]"));
        var t22 = new BinaryTreeNode<Dialog>(new("Muchas gracias , quieres venir a mi casa?", " [SEGUIRLO] ", "[RECHAZAR LA INVITACIÓN]"));



        var t11 = new BinaryTreeNode<Dialog>(new("Soy el gran herrero TripleT", "Triple T?", "Que me contas!"));

        var t23 = new BinaryTreeNode<Dialog>(new("TUNG TUNG TUNG SAHUR", " [DARLE TODO TU DINERO] ", "[REZARLE]"));
        var t24 = new BinaryTreeNode<Dialog>(new("Insolente preparate para morir ", " [PELEAR] ", "[ESCAPAR]"));



        /*var n00 = new BinaryTreeNode<string>("Hola aventurero!");
        var n10 = new BinaryTreeNode<string>("Como estas?");
        var n11 = new BinaryTreeNode<string>("Quien eres?");
        var n20 = new BinaryTreeNode<string>("Me alegra oirlo!");
        var n21 = new BinaryTreeNode<string>("Claro, dime que necesitas");
        var n22 = new BinaryTreeNode<string>("Soy el mago Merlin");
        var n23 = new BinaryTreeNode<string>("Hasta pronto, viajero");

        n00.Left  = n10;
        n00.Right = n11;
        n10.Left  = n20;
        n10.Right = n21;
        n11.Left  = n22;
        n11.Right = n23;*/
        t00.Left = t10;
        t00.Right = t11;

        t10.Left = t21;
        t10.Right = t22;

        t11.Left = t23;
        t11.Right = t24;

        tree.SetRoot(t00);
    }
}
