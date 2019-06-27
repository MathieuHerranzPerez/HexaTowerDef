using UnityEngine;
using UnityEngine.UI;

public class ButtonShopTurret : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private Image turretImg = default;
    [SerializeField]
    private Color imgSelectedColor = default;
    [SerializeField]
    private Text priceText = default;
    [SerializeField]
    private Button btn = default;

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

    public void ResetColor()
    {
        turretImg.color = Color.white;
    }

    private void NotifyShop()
    {
        shop.NotifyBtnClicked(this, turretPrefab);
        Highlight();
    }

    private void Highlight()
    {
        turretImg.color = imgSelectedColor;
    }
}
