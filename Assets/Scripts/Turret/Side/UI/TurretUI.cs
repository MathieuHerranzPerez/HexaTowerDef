using System;
using System.Collections;
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
    private Dropdown focusStrategyDropdown = default;

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
        damageText.text = target.Turret.GetDamage().ToString();
        rangeText.text = target.Turret.stats.range.ToString();
        fireRateText.text = target.Turret.stats.fireRate.ToString();

        if (target.Turret is LaserTurret)
        {
            LaserTurret t = (LaserTurret) target.Turret;
            slowAmountText.text = ((int) t.stats.slowPercent).ToString() + "%";
        }
        else
        {
            slowAmountText.text = "0%";
        }

        damageUpText.text = target.Turret.stats.damageUP.ToString();
        rangeUpText.text = target.Turret.stats.rangeUP.ToString();
        fireRateUpText.text = target.Turret.stats.fireRateUP.ToString();
        slowAmountUpText.text = target.Turret.stats.slowUP.ToString();

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

        if(target.Turret is ShootingTurret)
        {
            ShootingTurret st = (ShootingTurret)target.Turret;
            ChangeDropdownStrategy((int) st.Strat);
            focusStrategyDropdown.gameObject.SetActive(true);
        }
        else
        {
            focusStrategyDropdown.gameObject.SetActive(false);
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
    }

    public void HideUpgrade()
    {
        uiUpgrade.SetActive(false);
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
