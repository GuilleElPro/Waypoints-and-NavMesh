using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NavMeshManager navMeshManager;
    [SerializeField] private FollowPath followPath;
   
    private bool isNavMeshSet = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  


    public void SetHeliDestination()
    {
        if (isNavMeshSet)
        {
            navMeshManager.SetHeli();
        }
        else if (!isNavMeshSet)
        {
            followPath.GoToHeli();
        }
    }
      
     public void SetRuinDestination()
    {
        if (isNavMeshSet)
        {
            navMeshManager.SetRuin();
        }
        else if (!isNavMeshSet)
        {
            followPath.GoToRuin();
        }
    }
    public void SetNavMesh()
    {
        isNavMeshSet = true;
        navMeshManager.ControlAgent(true);
        followPath.isNavMesh = true;

    }
    public void SetWayPoint()
    {
        isNavMeshSet = false;
        navMeshManager.ControlAgent(false);
        followPath.isNavMesh = false;
    }
}
