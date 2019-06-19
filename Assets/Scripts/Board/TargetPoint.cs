using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public Tile tile { get { return equivalentTile; } }

    [Range(1f, 20f)]
    [SerializeField]
    private float rangeToExplose = 10f;
    [SerializeField]
    private Tile equivalentTile = default;

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
