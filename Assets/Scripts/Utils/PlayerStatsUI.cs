using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private Text moneyText = default;
    [SerializeField]
    private Image lifeBarImg = default;

    void Update()
    {
        moneyText.text = PlayerStats.Money.ToString();
        lifeBarImg.fillAmount = ((float) PlayerStats.Lives / (float) PlayerStats.StartLives);
    }
}
