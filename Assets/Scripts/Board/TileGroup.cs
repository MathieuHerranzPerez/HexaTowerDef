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
    [Range(0f, 1f)]
    private float emptyWallPopChanceAfterWall = 0.4f;

    [Header("Setup")]
    [SerializeField]
    private Transform tileContainer = default;
    [SerializeField]
    private GameObject wallPrefab = default;
    [SerializeField]
    private GameObject wallSupportPrefab = default;
    [SerializeField]
    private GameObject emptyWallPrefab = default;

    [SerializeField]
    private MapCreator mapCreator = default;

    // ---- INTERN ----
    private List<Tile> listTile;

    public void GenerateSquare()
    {
        Clear();
        List<Tile> listTile = mapCreator.BuildSquare(tileContainer);

        foreach(Tile t in listTile)
        {
            BuildRandomOnTile(t);
        }
    }

    public void GenerateCircle()
    {
        Clear();
        List<Tile> listTile = mapCreator.BuildCircle(tileContainer);
    }


    private void Clear()
    {
        listTile = new List<Tile>();
        foreach (Transform child in tileContainer)
        {
            Destroy(child);
        }
    }

    private void BuildRandomOnTile(Tile t)
    {
        float random = Random.Range(0f, 1f);
        if (random < wallPopChance)
        {
            float random2 = Random.Range(0f, 1f);
            if(random2 < supportWallPopChanceAfterWall)
            {
                // pop support wall
            }
            else
            {
                // pop wall
            }

            //
        }
        else
        {
            float random2 = Random.Range(0f, 1f);
            if(random2 < emptyWallPopChanceAfterWall)
            {

            }
        }
    }
}
