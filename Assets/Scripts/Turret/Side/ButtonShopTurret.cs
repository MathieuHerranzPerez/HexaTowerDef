using UnityEngine;
using UnityEngine.UI;

public class ButtonShopTurret : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private Image turretImg;
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private Button btn;

    // ---- INTERN ----
    private GameObject turretPrefab;
    private Shop shop;

    public void InitWith(GameObject turretPrefab, Shop shop)
    {
        this.turretPrefab = turretPrefab;
        this.shop = shop;

        Turret t = turretPrefab.GetComponent<Turret>();

        turretImg.sprite = t.GetImg();
        priceText.text = t.stats.cost.ToString();

        btn.onClick.AddListener(NotifyShop);
    }

    private void NotifyShop()
    {
        shop.NotifyBtnClicked(turretPrefab);
    }
}
