using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGroup : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float wallPopChance = 0.5f;
    [SerializeField]
    [Range(0f, 0.8f)]
    private float supportWallPopChanceAfterWall = 0.3f;
    [SerializeField]
    [Range(0f, 0.4f)]
    private float solidWallPopChanceAfterWall = 0.1f;
    [SerializeField]
    [Range(0f, 1f)]
    private float emptyWallPopChanceAfterWall = 0.4f;

    [SerializeField]
    private Transform tileContainerDefaultPos = default;

    [Header("Setup")]
    [SerializeField]
    private TileGroupeContainer tileContainer = default;
    [SerializeField]
    private GameObject wallPrefab = default;
    [SerializeField]
    private GameObject wallSupportPrefab = default;
    [SerializeField]
    private GameObject emptyWallPrefab = default;
    [SerializeField]
    private GameObject solidWallPrefab = default;
    [SerializeField]
    private GameObject spawnPrefab = default;

    [SerializeField]
    private MapCreator mapCreator = default;

    // ---- INTERN ----
    private List<Tile> listTile;
    private Tile currentCenter;
    private BuildingMapManager buildingMapManager;

    public void SetBuildingMapManager(BuildingMapManager buildingMapManager)
    {
        this.buildingMapManager = buildingMapManager;
    }

    public void GenerateSquare(bool withSpawn)
    {
        Clear();
        listTile = mapCreator.BuildSquare(tileContainer.transform, out currentCenter);

        BuildWalls(listTile, withSpawn);
    }

    public void GenerateCircle(bool withSpawn)
    {
        Clear();
        listTile = mapCreator.BuildCircle(tileContainer.transform, out currentCenter);

        BuildWalls(listTile, withSpawn);
    }

    public bool TryToBuild(Transform worldPos, int nbRotationLeft)
    {
        // get the map tile corresponding to the worldPos
        Tile tileWhereBuild = buildingMapManager.GetTileAtWorldPos(worldPos);
        if (tileWhereBuild == null)
            throw new System.Exception("We are trying to build on an empty GameObject (Tile) at : " + worldPos.position);

        // translate the logic tile pos to fit the map tile
        Point pointOffset = currentCenter.pos - tileWhereBuild.pos;
        nbRotationLeft = nbRotationLeft % 6;
        foreach (Tile t in listTile)
        {
            if (nbRotationLeft == 0)
                t.pos -= pointOffset;
            else
            {
                Tile tmp = buildingMapManager.GetTileAtWorldPos(t.transform);
                if (tmp == null)
                    return false;
                t.pos = tmp.pos;
            }
        }

        return buildingMapManager.TryToBuild(listTile);
    }

    private void BuildWalls(List<Tile> listTile, bool withSpawn)
    {
        if(withSpawn)
        {
            int randomIndex = Random.Range(0, listTile.Count);
            int i = 0;
            foreach (Tile t in listTile)
            {
                if (withSpawn && i == randomIndex)
                {
                    BuildSpawn(t);
                }
                else
                {
                    BuildRandomOnTile(t);
                }
                ++i;
                ChangeLayer(t.gameObject);
            }
        }

        tileContainer.Activate();
    }


    private void Clear()
    {
        listTile = new List<Tile>();
        foreach (Transform child in tileContainer.transform)
        {
            Destroy(child);
        }

        tileContainer.transform.position = tileContainerDefaultPos.position;
        tileContainer.transform.rotation = tileContainerDefaultPos.rotation;

        tileContainer.Desactivate();
    }

    private void BuildRandomOnTile(Tile t)
    {
        float random = Random.Range(0f, 1f);
        GameObject wallCloneGO;
        if (random < wallPopChance)
        {
            
            float random2 = Random.Range(0f, 1f);
            if(random2 < supportWallPopChanceAfterWall)
            {
                // pop support wall
                wallCloneGO = (GameObject)Instantiate(wallSupportPrefab, t.center, t.transform.rotation, t.transform);
            }
            else if(random2 < supportWallPopChanceAfterWall + solidWallPopChanceAfterWall)
            {
                // pop solid wall
                wallCloneGO = (GameObject)Instantiate(solidWallPrefab, t.center, t.transform.rotation, t.transform);
            }
            else
            {
                // pop wall
                wallCloneGO = (GameObject)Instantiate(wallPrefab, t.center, t.transform.rotation, t.transform);
            }

            //
            TileContent wallClone = wallCloneGO.GetComponent<TileContent>();
            t.content = wallClone;
        }
        else
        {
            float random2 = Random.Range(0f, 1f);
            if(random2 < emptyWallPopChanceAfterWall)
            {
                wallCloneGO = (GameObject)Instantiate(emptyWallPrefab, t.center, t.transform.rotation, t.transform);
                TileContent wallClone = wallCloneGO.GetComponent<TileContent>();
                t.content = wallClone;
            }
        }
    }

    private void BuildSpawn(Tile t)
    {
        GameObject spawnGO = (GameObject)Instantiate(spawnPrefab, t.center, t.transform.rotation, t.transform);
        EnemySpawner es = spawnGO.GetComponent<EnemySpawner>();
        t.content = es;
        es.tile = t;

        // tod register the spawn
    }

    private void ChangeLayer(GameObject go)
    {
        go.layer = 10;
        foreach(Transform child in go.transform)
        {
            child.gameObject.layer = 10;
        }
    }
}
