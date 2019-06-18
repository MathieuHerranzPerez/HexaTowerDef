using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUpgradeTurretUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Setup")]
    [SerializeField]
    public TurretUI turretUI = default;
    [SerializeField]
    private Button btn = default;

    // ---- INTERN ----
    bool isActivitated = true;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isActivitated)
            turretUI.DisplayUpgrade();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        turretUI.HideUpgrade();
    }

    public void ActiveButton()
    {
        btn.interactable = true;
        isActivitated = true;
    }

    public void DesactiveButton()
    {
        btn.interactable = false;
        isActivitated = false;
    }
}
