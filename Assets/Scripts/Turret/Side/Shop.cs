using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private GameObject[] listTurretPrefab = new GameObject[1];

    [SerializeField]
    private GameObject buttonShopTurretPrefab = default;

    // ---- INTERN ----
    private BuildManager buildManager;


    void Start()
    {
        buildManager = BuildManager.Instance;
        InitShop();
    }

    public void NotifyBtnClicked(GameObject turretToBuild)
    {
        buildManager.SetTurretToBuild(turretToBuild);
    }

    private void InitShop()
    {
        foreach(GameObject turretGO in listTurretPrefab)
        {
            GameObject btnGO = Instantiate(buttonShopTurretPrefab, transform);
            ButtonShopTurret button = btnGO.GetComponent<ButtonShopTurret>();
            button.InitWith(turretGO, this);
        }
    }
}
