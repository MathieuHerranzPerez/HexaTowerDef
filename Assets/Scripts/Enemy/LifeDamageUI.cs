using UnityEngine;
using UnityEngine.UI;

public class LifeDamageUI : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve textSizeByDamage = default;

    [Header("Setup")]
    [SerializeField]
    private Text damageText = default;

    public void SetDamage(float damage)
    {
        transform.LookAt(Camera.main.transform);

        damageText.text = "-" + damage.ToString();
        damageText.fontSize = (int) textSizeByDamage.Evaluate(damage);
    }
}
