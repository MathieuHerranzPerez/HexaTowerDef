using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapCreator : MonoBehaviour
{
    [SerializeField]
    private float hexagoneSize = 1f;

    [SerializeField]
    private GameObject hexagonePrefab = default;

    // size of the map (nb hexagone tiles)
    [SerializeField]
    private int width = 20;
    [SerializeField]
    private int height = 20;

    [SerializeField]
    private Transform whereToPutTheTilesTransform = default;


    // ---- INTERN ----
    private float xOffset = 0.866f;
    private float zOffset = 0.75f;

    [SerializeField]
    private TileDictionary pointToTileStorage = TileDictionary.New<TileDictionary>();
    private Dictionary<Point, Tile> tileDictionary
    {
        get { return pointToTileStorage.dictionary; }
    }

    public void InitGrid()
    {
        CalculOffsets();

        for (int x = 0; x < width; ++x)
        {
            //int logicPosY = x;
            for (int z = 0; z < height; ++z)
            {
                float xPos = x * xOffset;
                if (z % 2 == 1)
                {
                    xPos += xOffset / 2f;
                    //++logicPosY;
                }
                int logicPosX = x + (int)(z * -0.5f);
                Point pos = new Point(logicPosX/*, logicPosY*/, z);

                bool existButNull = (tileDictionary.ContainsKey(pos) && tileDictionary[pos] == null);

                if (!tileDictionary.ContainsKey(pos) || existButNull)
                {
                    if(existButNull)
                    {
                        tileDictionary.Remove(pos);
                    }

                    GameObject tileCloneGO = Instantiate(hexagonePrefab, new Vector3(xPos, 0, z * zOffset), Quaternion.identity, whereToPutTheTilesTransform);
                    Tile tileClone = tileCloneGO.GetComponent<Tile>();

                    tileClone.pos = pos;

                    tileDictionary.Add(tileClone.pos, tileClone);

                    tileCloneGO.name = "Tile-" + logicPosX /*+ "_" + logicPosY */+ "_" + z;
                }
            }
        }
    }

    public void Clear()
    {
        foreach(Tile t in tileDictionary.Values)
        {
            if(t != null)
            {
                DestroyImmediate(t.gameObject);
            }
        }

        tileDictionary.Clear();
    }


    public List<Tile> BuildCircle(Transform container)
    {
        CalculOffsets();
        List<Tile> res = new List<Tile>();

        for (int x = 0; x < 3; ++x)
        {
            //int logicPosY = x;
            for (int z = 0; z < 3; ++z)
            {
                if ((!(x == 0) && !(z == 0)) || (!(x == 0) && !(z == 2)))
                {
                    float xPos = x * xOffset;
                    if (z % 2 == 1)
                    {
                        xPos += xOffset / 2f;
                        //++logicPosY;
                    }
                    int logicPosX = x + (int)(z * -0.5f);
                    Point pos = new Point(logicPosX/*, logicPosY*/, z);


                    GameObject tileCloneGO = Instantiate(hexagonePrefab, container);
                    tileCloneGO.transform.localPosition = new Vector3(xPos, 0, z * zOffset);
                    tileCloneGO.transform.localRotation = Quaternion.identity;

                    Tile tileClone = tileCloneGO.GetComponent<Tile>();
                    tileClone.pos = pos;

                    tileCloneGO.name = "Tile-" + logicPosX /*+ "_" + logicPosY */+ "_" + z;

                    res.Add(tileClone);
                }
            }
        }
        return res;
    }

    public List<Tile> BuildSquare(Transform container)
    {
        CalculOffsets();
        List<Tile> res = new List<Tile>();

        for (int x = 0; x < 2; ++x)
        {
            //int logicPosY = x;
            for (int z = 0; z < 2; ++z)
            {
                float xPos = x * xOffset;
                if (z % 2 == 1)
                {
                    xPos += xOffset / 2f;
                    //++logicPosY;
                }
                int logicPosX = x + (int)(z * -0.5f);
                Point pos = new Point(logicPosX/*, logicPosY*/, z);


                GameObject tileCloneGO = Instantiate(hexagonePrefab, container);
                tileCloneGO.transform.localPosition = new Vector3(xPos, 0, z * zOffset);
                tileCloneGO.transform.localRotation = Quaternion.identity;

                Tile tileClone = tileCloneGO.GetComponent<Tile>();
                tileClone.pos = pos;

                tileCloneGO.name = "Tile-" + logicPosX /*+ "_" + logicPosY */+ "_" + z;

                res.Add(tileClone);
            }
        }
        return res;
    }


    /**
     * Set the x and z offsets to put the tiles correctly in world pos
     */
    private void CalculOffsets()
    {
        float rad = 60 * Mathf.PI / 180;
        xOffset = ((hexagoneSize / 4) * Mathf.Tan(rad)) * 2;
        zOffset = hexagoneSize * (3f / 4f);
    }
}

