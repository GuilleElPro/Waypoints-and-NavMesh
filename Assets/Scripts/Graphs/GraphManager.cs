using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creamos una estructura para facilitar la entrada de datos en nuestro grafo
[System.Serializable]
public struct Link //Edge entre nodos
{
    public enum direction { UNI, BI }; //Edge de una direccion o bidireccional
    public GameObject node1;
    public GameObject node2;
    public direction dir;
}

public class GraphManager : MonoBehaviour
{
    public GameObject[] waypoints;    //Array de waypoints
    public Link[] links;              //Array de links
    public Graph graph = new Graph(); //Codigo para el grafo esta ya creado en la carpeta "Graphs"
 

    // Start is called before the first frame update
    void Start()
    {
        //Si wayPoints tiene nodos definidos (prevenir estructura vacia)
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("GraphManager: no hay Waypoints asignados.", this);
            return;
        }
        //if (...) {
        //Iteramos para cada WP y lo guardamos en la estructura graph
        foreach (GameObject wp in waypoints)
        {
           
            
            if (wp != null)
            {
                graph.AddNode(wp);
            }
           
        }


        //Iteramos para cada link y lo guardamos en la estructura graph
        if (links != null)
        {
            foreach (Link link in links)
            {
                if (link.node1 == null || link.node2 == null)
                {
                    Debug.LogWarning("GraphManager: hay una conexión incompleta.", this);
                    continue;
                }

                // Dirección de node1 hacia node2.
                graph.AddEdge(link.node1, link.node2);

                // Si es bidireccional, añadir también la vuelta.
                if (link.dir == Link.direction.BI)
                {
                    graph.AddEdge(link.node2, link.node1);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
       
        graph.debugDraw();
    }
}
