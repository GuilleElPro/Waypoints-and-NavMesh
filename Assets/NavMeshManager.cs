using UnityEngine;
using UnityEngine.AI;
public class NavMeshManager : MonoBehaviour
{
    [SerializeField] private GameObject heliPoint;
    [SerializeField] private GameObject ruinPoint;
    [SerializeField] private NavMeshAgent agent;
    private Transform targetD;
    private Transform currentD;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("No se ha encontrado un NavMeshAgent en el Player.");
        }
    }

    public void SetHeli()
    {
        Debug.Log("No se ha encontrado un NavMeshAgent en el Player.");
        SetDestination(heliPoint.transform);
        
    }
    public void SetRuin()
    {
        SetDestination(ruinPoint.transform);
    }


    // Update is called once per frame
    public void SetDestination(Transform destination)
    {
        if (destination == null || destination == currentD) return;
        targetD = destination;
        agent.SetDestination(destination.position);
        currentD = destination;

    }
}
