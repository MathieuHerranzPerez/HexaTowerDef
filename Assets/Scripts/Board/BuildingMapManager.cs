using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMapManager : MonoBehaviour
{
    [SerializeField]
    private int waveToGetASpawn = 4;
    [SerializeField]
    private int nbBuildingToSpawnPerWave = 2;

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
    private int nbBuildingBuilt = 0;

    void Start()
    {
        tileGroup.SetBuildingMapManager(this);
    }

    public void StartPhase(int numWave)
    {
        currentWave = numWave;
        //cameraContainer.GoTopCenter();
        List<Tile> listTilesBuilt = tileGroup.GenerateCircle(NeedToPopASpawner(numWave));

        CheckIfCanBuild(listTilesBuilt);
    }

    public bool TryToBuild(List<Tile> listTileToBuild)      /// TODO SPAWNER !
    {
        // get all the spawn point and the targetPoint
        List<Tile> listEnemySpawnerTile = spawnerManager.GetListTileOfSpawners();

        EnemySpawner es = null;
        if (NeedToPopASpawner(currentWave))
        {
            // search the new spawner
            int i = 0;
            while (es == null && i < listTileToBuild.Count)
            {
                if (listTileToBuild[i].content != null)
                    es = listTileToBuild[i].content.GetComponent<EnemySpawner>();
                ++i;
            }
        }

        bool res = map.TryToBuild(listTileToBuild, listEnemySpawnerTile, spawnerManager.EnemyTarget.tile, NeedToPopASpawner(currentWave));

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

    public void NotifyBuilt()
    {
        ++nbBuildingBuilt;

        if (nbBuildingBuilt == nbBuildingToSpawnPerWave)
        {
            nbBuildingBuilt = 0;
            EndPhase();
        }
        else
        {
            if (nbBuildingBuilt % 2 == 0)
            {
                List<Tile> listTilesBuilt = tileGroup.GenerateCircle(false);

                CheckIfCanBuild(listTilesBuilt);
            }
            else
            {
                List<Tile> listTilesBuilt = tileGroup.GenerateSquare(false);

                CheckIfCanBuild(listTilesBuilt);
            }
        }
    }

    private void EndPhase()
    {
        //cameraContainer.ReturnToPrevious();
        GameManager.Instance.NotifyBuilt();
    }

    private void CheckIfCanBuild(List<Tile> listTileToBuild)
    {
        // get all the spawn point and the targetPoint
        List<Tile> listEnemySpawnerTile = spawnerManager.GetListTileOfSpawners();

        for (int i = 0; i < listEnemySpawnerTile.Count; ++i)
        {
            Debug.Log("spanwer : " + listEnemySpawnerTile[i]); // affD
        }

        if(!map.IsThereAPlaceFor(tileGroup.Container, listTileToBuild, listEnemySpawnerTile, spawnerManager.EnemyTarget.tile, NeedToPopASpawner(currentWave)))
        {
            GameManager.Instance.EndByWin();
        }
    }

    private bool NeedToPopASpawner(int numWave)
    {
        return numWave % waveToGetASpawn == 0;
    }
}
