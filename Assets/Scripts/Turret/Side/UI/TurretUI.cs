using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretUI : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private GameObject CanvasUIGO = default;
    [SerializeField]
    private ButtonUpgradeTurretUI btnUpgradeTurret = default;

    [SerializeField]
    private Text upgradeCostText = default;
    [SerializeField]
    private Text sellAmountText = default;
    [SerializeField]
    private Image turretImg = default;

    [SerializeField]
    private Text damageText = default;
    [SerializeField]
    private Text rangeText = default;
    [SerializeField]
    private Text fireRateText = default;
    [SerializeField]
    private Text slowAmountText = default;
    [SerializeField]
    private Text boostAmountText = default;

    [SerializeField]
    private GameObject uiUpgrade = default;
    [SerializeField]
    private Text damageUpText = default;
    [SerializeField]
    private Text rangeUpText = default;
    [SerializeField]
    private Text fireRateUpText = default;
    [SerializeField]
    private Text slowAmountUpText = default;
    [SerializeField]
    private Text boostAmountUpText = default;

    [SerializeField]
    private Dropdown focusStrategyDropdown = default;

    [SerializeField]
    private Canvas ShootingTurretCanvas = default;
    [SerializeField]
    private Canvas SupportTurretCanvas = default;

    // ---- INTERN ----
    private Wall target;

    void Start()
    {
        InitDropDownStrategy();
    }

    public void SetTarget(Wall target)
    {
        this.target = target;

        CanvasUIGO.SetActive(true);

        sellAmountText.text = target.Turret.GetSellAmount().ToString();
        turretImg.sprite = target.Turret.GetImg();
        rangeText.text = target.Turret.stats.range.ToString();

        if (target.Turret is SlowDamageSpeedTurret)
        {
            SupportTurretCanvas.gameObject.SetActive(false);
            ShootingTurretCanvas.gameObject.SetActive(true);

            SlowDamageSpeedTurret t = (SlowDamageSpeedTurret) target.Turret;
            fireRateText.text = t.GetSpeed().ToString();
            fireRateUpText.text = t.GetSpeedUp().ToString();
            damageText.text = t.GetDamage().ToString();
            damageUpText.text = t.GetDamageUp().ToString();
            slowAmountText.text = ((int) t.GetSlow()).ToString() + "%";
            slowAmountUpText.text = ((int)t.GetSlowUp()).ToString() + "%";

            if (target.Turret is ShootingTurret)
            {
                ShootingTurret st = (ShootingTurret)target.Turret;
                ChangeDropdownStrategy((int)st.Strat);

                focusStrategyDropdown.gameObject.SetActive(true);
            }
            else
            {
                focusStrategyDropdown.gameObject.SetActive(false);
            }
        }
        else // support
        {
            SupportTurretCanvas.gameObject.SetActive(true);
            ShootingTurretCanvas.gameObject.SetActive(false);

            BoostTurret bt = (BoostTurret)target.Turret;
            boostAmountText.text = bt.GetBoost().ToString();
            boostAmountUpText.text = bt.GetBoostUp().ToString();
        }

        rangeUpText.text = target.Turret.stats.rangeUp.ToString(); 

        if (target.Turret.HasAnUpgrade)
        {
            upgradeCostText.text = target.Turret.stats.upgradeCost.ToString();
            btnUpgradeTurret.ActiveButton();
        }
        else
        {
            upgradeCostText.text = "";
            btnUpgradeTurret.DesactiveButton();
        }

        TurretRangeDisplayer.Instance.DisplayRange(target.Turret);
    }

    public void Hide()
    {
        CanvasUIGO.SetActive(false);
        TurretRangeDisplayer.Instance.HideRange();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.Instance.DeselectWall();
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.Instance.DeselectWall();
    }

    public void DisplayUpgrade()
    {
        uiUpgrade.SetActive(true);
        rangeUpText.gameObject.SetActive(true);
    }

    public void HideUpgrade()
    {
        uiUpgrade.SetActive(false);
        rangeUpText.gameObject.SetActive(false);
    }

    public void DropdownChanged()
    {
        ShootingTurret st = (ShootingTurret) target.Turret;
        st.ChangeStrategy(FocusStrategyFactory.Instance.CreateStrategy((Strategy) focusStrategyDropdown.value, st));
    }

    public void ChangeDropdownStrategy(int index)
    {
        focusStrategyDropdown.value = index;
    }

    private void InitDropDownStrategy()
    {
        string[] enumNames = Enum.GetNames(typeof(Strategy));
        List<string> listStrategy = new List<string>(enumNames);

        focusStrategyDropdown.ClearOptions();
        focusStrategyDropdown.AddOptions(listStrategy);
    }
}
