using UnityEngine;

public class TurretRangeDisplayer : MonoBehaviour
{
    public static TurretRangeDisplayer Instance { get; private set; }

    [Header("Setup")]
    [SerializeField]
    private GameObject sphereRangeGO = default;
    [SerializeField]
    private GameObject sphereRangeInsideGO = default;

    void Awake()
    {
        Instance = this;
    }

    public void DisplayRange(Turret turret)
    {
        this.transform.position = turret.transform.position;
        Vector3 scale = new Vector3(turret.stats.range * 2, turret.stats.range * 2, turret.stats.range * 2);
        sphereRangeGO.transform.localScale = scale;
        sphereRangeGO.SetActive(true);
        sphereRangeInsideGO.transform.localScale = scale;
        sphereRangeInsideGO.SetActive(true);
    }

    public void HideRange()
    {
        sphereRangeGO.SetActive(false);
        sphereRangeInsideGO.SetActive(false);
    }
}
