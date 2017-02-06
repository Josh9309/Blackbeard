using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshOutlineGizmo : MonoBehaviour
{
    #region Attributes
    private Mesh objectMesh; //The mesh of the object this script is attached to
    [Space(-10)] //Formatting
    [Header("REMEMBER TO TURN ON GIZMOS IN GAME VIEW")] //Gently remind the user to turn on Gizmos
    [Space(10)] //Formatting
    [SerializeField] private bool renderWireframe; //Should the object show a wireframe
    [SerializeField] private bool renderFill; //Should the object show a fill color
    [Space(10)] //Formatting
    [SerializeField] private Color wireColor; //The color of the wire mesh
    [SerializeField] private Color fillColor; //The color of the mesh
    #endregion

    private void Start()
    {
        objectMesh = GetComponent<MeshFilter>().mesh; //Get the mesh
    }

    private void OnDrawGizmos() //This is a magic method that can be used to draw custom gizmos
    {
        if (renderWireframe) //If wireframes should be drawn
        {
            Gizmos.color = wireColor;
            Gizmos.DrawWireMesh(objectMesh, transform.position);
        }

        if (renderFill) //If fill should be drawn
        {
            Gizmos.color = fillColor;
            Gizmos.DrawMesh(objectMesh, transform.position);
        }
    }
}