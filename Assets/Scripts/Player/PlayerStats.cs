using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public static int Lives;
    public static int StartLives;
    public static int Rounds;

    public int startMoney = 400;
    public int startLives = 1000;

    void Start()
    {
        Money = startMoney;
        StartLives = startLives;
        Lives = StartLives;

        Rounds = 0;
    }
}
