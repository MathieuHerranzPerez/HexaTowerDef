using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    [Range(1f, 20f)]
    [SerializeField]
    private float rangeToExplose = 10f;

    public float GetRangeToExplose()
    {
        return rangeToExplose;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, rangeToExplose);
    }
}
