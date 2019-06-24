using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public TargetPoint Target { get { return targetPoint; } }

    // ---- INTERN ----
    private TargetPoint targetPoint;
    private Enemy enemy;
    private NavMeshAgent agent;

    private float previousSpeed;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, targetPoint.transform.position) <= targetPoint.GetRangeToExplose())
        {
            enemy.Explode();
        }

        agent.speed = enemy.speed;
    }

    public void SetTarget(TargetPoint target)
    {
        targetPoint = target;
        agent.SetDestination(target.transform.position);
    }
}
