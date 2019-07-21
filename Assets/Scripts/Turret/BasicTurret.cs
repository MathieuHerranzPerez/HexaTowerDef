
using UnityEngine;

public class BasicTurret : BulletTurret
{
    protected override bool CheckShootCondition()
    {
        bool[] listCheckerRes = new bool[listFirePointChecker.Length];
        for (int i = 0; i < listCheckerRes.Length; ++i)
        {
            listCheckerRes[i] = false;
        }

        RaycastHit hit;

        // check if there is no obstacle between the fire point and the target
        int index = 0;
        foreach(Transform checker in listFirePointChecker)
        {
            if (Physics.Raycast(checker.position, checker.TransformDirection(Vector3.forward), out hit, stats.range))
            {
                foreach (LayerMask enemyLayer in listEnemyLayer)
                {
                    if (hit.collider.gameObject.layer == enemyLayer)
                    {
                        Debug.DrawRay(checker.position, checker.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);  // affD
                        listCheckerRes[index] = true;
                    }
                }
            }
            ++index;
        }

        bool res = true;
        foreach (bool b in listCheckerRes)
        {
            if (!b)
                res = false;
        }

        return res;
    }
}
