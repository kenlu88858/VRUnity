using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMove : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
