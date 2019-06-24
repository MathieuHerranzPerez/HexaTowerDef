using UnityEngine;

public class FocusStrategyFactory : MonoBehaviour
{
    public static FocusStrategyFactory Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public FocusStrategy CreateStrategy(Strategy strat, ShootingTurret turret)
    {
        switch (strat)
        {
            case Strategy.Nearest:
                return new FocusVisibleNearest(turret);
            case Strategy.Biggest:
                return new FocusBiggest(turret);
            case Strategy.Smallest:
                return new FocusSmallest(turret);
            case Strategy.Fastest:
                return new FocusFastest(turret);
            case Strategy.Slowest:
                return new FocusSlowest(turret);
            case Strategy.HighestLife:
                return new FocusHighestLife(turret);
            case Strategy.LowestLife:
                return new FocusLowestLife(turret);
            case Strategy.Strongest:
                return new FocusStrongest(turret);
            case Strategy.Weakest:
                return new FocusWeakest(turret);
            default:
                throw new System.Exception("Unsupprted Strategy");
        }
    }
}

public enum Strategy
{
    Nearest,
    Biggest,
    Smallest,
    Fastest,
    Slowest,
    Strongest,
    Weakest,
    HighestLife,
    LowestLife,
}
