using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootingTurret : Turret
{
    public Strategy Strat { get { return focusStratey.GetStrat() ; } }

    [Header("Setup")]
    public string enemyTag = "EnemyWalker";       // target type
    public LayerMask enemyMask;

    // ---- INTERN ----
    protected int enemyLayer; 
    protected Transform target;
    protected Enemy targetEnemy;
    protected FocusStrategy focusStratey;


    protected override void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        focusStratey = new FocusVisibleNearest(this);
        enemyLayer = Mathf.RoundToInt(Mathf.Log(enemyMask.value, 2));
    }

    protected override void UpdateCall()
    {
        if (target != null)
        {
            LockOnTarget();
        }
    }

    public void ChangeStrategy(FocusStrategy focusStratey)
    {
        this.focusStratey = focusStratey;
    }

    // if we want to change the target strategy, it's here
    protected void UpdateTarget()
    {
        GameObject enemyToFocus = focusStratey.GetEnemy();

        if (enemyToFocus != null)
        {
            target = enemyToFocus.transform;
            targetEnemy = enemyToFocus.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    protected void LockOnTarget()
    {
        // Y
        Vector3 directionY = target.position - partToRotateY.position;   // direction to the target
        Quaternion lookRatationY = Quaternion.LookRotation(directionY);
        Vector3 rotationY = Quaternion.Lerp(partToRotateY.rotation, lookRatationY, Time.deltaTime * stats.turnSpeed).eulerAngles;
        partToRotateY.rotation = Quaternion.Euler(0f, rotationY.y, 0f);

        // X
        Vector3 directionX = target.position - partToRotateX.position;   // direction to the target
        Quaternion lookRatationX = Quaternion.LookRotation(directionX);
        Vector3 rotationX = Quaternion.Lerp(partToRotateX.rotation, lookRatationX, Time.deltaTime * stats.turnSpeed).eulerAngles;
        partToRotateX.localRotation = Quaternion.Euler(rotationX.x, 0f, 0f);
    }
}
