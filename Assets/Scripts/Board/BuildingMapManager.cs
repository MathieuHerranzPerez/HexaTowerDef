using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMapManager : MonoBehaviour
{
    [SerializeField]
    private int waveToGetASpawn = 9;

    [Header("Setup")]
    [SerializeField]
    private CameraContainer cameraContainer = default;
    [SerializeField]
    private TileGroup tileGroup = default;
    [SerializeField]
    private Map map = default;
    [SerializeField]
    private SpawnerManager spawnerManager = default;

    // ---- INTERN ----
    private int currentWave = 0;

    void Start()
    {
        tileGroup.SetBuildingMapManager(this);
    }

    public void StartPhase(int numWave)
    {
        currentWave = numWave;
        cameraContainer.GoTopCenter();
        tileGroup.GenerateCircle(needToPopASpawner(numWave));
    }

    public bool TryToBuild(List<Tile> listTileToBuild)
    {
        // get all the spawn point and the targetPoint
        List<Tile> listEnemySpawnerTile = spawnerManager.GetListTileOfSpawners();
        EnemySpawner es = null;
        if (needToPopASpawner(currentWave))
        {
            // search the new spawner
            int i = 0;
            while(es == null && i < listTileToBuild.Count)
            {
                if(listTileToBuild[i].content != null)
                    es = listTileToBuild[i].content.GetComponent<EnemySpawner>();
                ++i;
            }

            if(es != null)
            {
                listEnemySpawnerTile.Add(map.GetTileAtPos(es.tile.pos));
            }
        }

        bool res = map.TryToBuild(listTileToBuild, listEnemySpawnerTile, spawnerManager.EnemyTarget.tile);

        if(res && es != null)
        {
            es.tile = map.GetTileAtPos(es.tile.pos);
            spawnerManager.AddSpawner(es);
        }

        return res;
    }

    public Tile GetTileAtWorldPos(Transform pos)
    {
        return map.GetTileAtWorldPos(pos);
    }

    private void EndPhase()
    {
        cameraContainer.ReturnToPrevious();
    }

    private bool needToPopASpawner(int numWave)
    {
        return numWave % waveToGetASpawn == 0;
    }
}
