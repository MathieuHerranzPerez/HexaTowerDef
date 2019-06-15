using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{

    // ---- INTERN ----
    private Transform targetWaypoint;
    private Enemy enemy;
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
    }

    public void SetTarget(Transform target)
    {
        targetWaypoint = target;
        agent.SetDestination(target.position);
    }
}
