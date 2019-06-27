using System.Collections.Generic;
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
    private List<ButtonShopTurret> listBtn = new List<ButtonShopTurret>();
    private ButtonShopTurret lastBtn;


    void Start()
    {
        buildManager = BuildManager.Instance;
        InitShop();
    }

    public void NotifyBtnClicked(ButtonShopTurret btn, GameObject turretToBuild)
    {
        buildManager.SetTurretToBuild(turretToBuild);
        if(lastBtn != null)
            lastBtn.ResetColor();
        lastBtn = btn;
    }

    private void InitShop()
    {
        foreach(GameObject turretGO in listTurretPrefab)
        {
            GameObject btnGO = Instantiate(buttonShopTurretPrefab, transform);
            ButtonShopTurret button = btnGO.GetComponent<ButtonShopTurret>();
            button.InitWith(turretGO, this);
            listBtn.Add(button);
        }
    }
}
