
public class SupportWall : Wall
{
    protected override bool AreConditionsOK()
    {
        return (buildManager.CanBuild && buildManager.turretToBuildPrefab.GetComponent<Turret>() is SupportTurret);
    }
}
