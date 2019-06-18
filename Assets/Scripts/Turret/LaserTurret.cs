using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : ShootingTurret
{
    public override int GetDamage()
    {
        return (int) stats.slowPercent;
    }
}
