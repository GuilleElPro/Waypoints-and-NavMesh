using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    Transform goal; //Goal from A* algorithm
    float speed = 5.0f;          //Velocidad del tanque
    float accuracy = 1.0f;		 //Distancia por debajo de la cual considero que he alcanzado el nodo
    float rotSpeed = 2.0f;       //Velocidad de rotacion del tanque
    public GameObject wpManager; //All the graph points and links
    GameObject[] wps;            //Nodos del grafo
    GameObject currentNode;		 //El nodo mas cercano en cada momento
    int currentWP = 0;           //Indice a lo largo del camino que seguimos, no tiene que ver con el numero/posicion de cada WP
    Graph g;                     //Grafo

    public GameObject heliWaypoint;
    public GameObject ruinWaypoint;
    private bool pathActive = false;

    // Start is called before the first frame update
    void Start()
    {
        //Inicializar wps, g y currentNode
        if (wpManager == null)
        {
            Debug.LogError("FollowPath: asigna el GraphManager.", this);
            enabled = false;
            return;
        }

        GraphManager manager = wpManager.GetComponent<GraphManager>();

        if (manager == null)
        {
            Debug.LogError("FollowPath: el objeto asignado no tiene GraphManager.", this);
            enabled = false;
            return;
        }

        g = manager.graph;
        wps = manager.waypoints;

        currentNode = FindClosestWaypoint();

        if (currentNode == null)
        {
            Debug.LogError("FollowPath: no se encuentran Waypoints validos.", this);
            enabled = false;
        }
    }

    GameObject FindClosestWaypoint()
    {
        if (wps == null || wps.Length == 0)
            return null;

        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject wp in wps)
        {
            if (wp == null)
                continue;

            float distance = Vector3.Distance(
                transform.position, wp.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = wp;
            }
        }

        return closest;
    }

    public void GoToHeli()
    {
        
        GoToDestination(heliWaypoint);
    }

    public void GoToRuin()
    {
        GoToDestination(ruinWaypoint);
    }

    void GoToDestination(GameObject destination)
    {
        if (destination == null)
        {
            Debug.LogError(
                "FollowPath: asigna el Waypoint de destino en el Inspector.",
                this);
            return;
        }

        currentNode = FindClosestWaypoint();

        if (currentNode == null || currentNode == destination)
            return;

        bool found = g.AStar(currentNode, destination);

        if (!found)
        {
            pathActive = false;
            Debug.LogWarning(
                "No existe una ruta hasta " + destination.name +
                ". Comprueba las conexiones del grafo.");
            return;
        }

        currentWP = 0;
        pathActive = true;

        g.printPath();
    }
    



    // Update is called once per frame
    void LateUpdate()
    {
        //Testear si no hay un camino definido (empty path) o currentWP esta ya al final del camino

        //Guardamos en currentNode el nodo que tenemos mas cercano en este momento

        //Si estamos suficientemente cerca al nodo currentWP, pasamos a buscar el siguiente

        //Si aun no hemos llegado al final del camino, nos dirigimos al siguiente nodo
        //if (...) {
        //Guardamos en goal el nuevo nodo a visitar

        //Miramos hacia nuestro destino (goal)

        //Movemos el tanque

        //}
        if (g == null || !pathActive)
            return;

        int pathLength = g.getPathLength();

        if (pathLength == 0 || currentWP >= pathLength)
        {
            pathActive = false;
            return;
        }

        // Obtener el siguiente punto de la ruta.
        GameObject nextPoint = g.getPathPoint(currentWP);

        if (nextPoint == null)
        {
            pathActive = false;
            return;
        }

        Vector3 target = nextPoint.transform.position;

        // Comprobar si hemos llegado al punto actual.
        if (Vector3.Distance(transform.position, target) <= accuracy)
        {
            currentWP++;

            if (currentWP >= pathLength)
            {
                pathActive = false;
                return;
            }

            nextPoint = g.getPathPoint(currentWP);

            if (nextPoint == null)
            {
                pathActive = false;
                return;
            }

            target = nextPoint.transform.position;
        }

        // Orientar el tanque hacia el siguiente punto.
        Vector3 direction = target - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotSpeed * Time.deltaTime);
        }

        // Mover el tanque hacia el punto de la ruta.
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime);
    
         g.debugDraw();

    }
}
