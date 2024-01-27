using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    private void Start()
    {

    }

    [ContextMenu("Test")]
    private void SetRandomDestination()
    {
        Vector3 randomDestination = Random.insideUnitSphere * 3f;
        randomDestination = new Vector3(randomDestination.x, randomDestination.y, 0);
        agent.SetDestination(randomDestination);
    }
}
