using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Map : MonoBehaviour
{
    [Header("Setup")]
    public NavMeshSurface surface;
    [SerializeField]
    private Transform tilesContainer = default;

    // ---- INTERN ----
    private Dictionary<Point, Tile> tilesDictionary = new Dictionary<Point, Tile>();
    private Dictionary<Point, Tile> emptyTileDictionary = new Dictionary<Point, Tile>();

    private void Awake()
    {
        BuildDictionary();

        NotifyUpdateWall();
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void NotifyUpdateWall()
    {
        surface.BuildNavMesh();
    }

    public Tile GetTileAtPos(Point pos)
    {
        return tilesDictionary.ContainsKey(pos) ? tilesDictionary[pos] : null;
    }

    public bool TryToBuild(List<Tile> listTileToBuild, List<Tile> listSpawn, Tile tileNeededToBeReashable, bool isThereASpawner)
    {
        return CanBuild(listTileToBuild, listSpawn, tileNeededToBeReashable, true, isThereASpawner);
    }

    public bool IsThereAReashablePlaceFrom(Tile tileNeededToBeReashable)
    {
        bool res = false;
        List<Tile> listToCheck = new List<Tile>(emptyTileDictionary.Values);

        LaunchWave(tileNeededToBeReashable);

        int i = 0;
        while(i < listToCheck.Count && !res)
        {
            if (listToCheck[i].isWet)
                res = true;
            ++i;
        }

        DryAll();

        return res;
    }

    public bool IsThereAPlaceFor(GameObject tileContainer, List<Tile> listTileToBuild, List<Tile> listSpawn, Tile tileNeededToBeReashable, bool isThereASpawner)
    {
        Vector3 startPos = tileContainer.transform.position;
        Quaternion startRot = tileContainer.transform.rotation;

        List<Tile> listToCheck = new List<Tile>(emptyTileDictionary.Values);

        int i = 0;
        bool res = false;
        while(i < listToCheck.Count && !res)
        {
            List<Tile> listTileToCheck = new List<Tile>();
            listTileToCheck.Add(listToCheck[i]);
            // add the tiles to check around the current
            foreach (Neighbour neighbour in (Neighbour[])Enum.GetValues(typeof(Neighbour)))
            {
                // check if we already have checked it
                Tile t = GetNeighbour(listToCheck[i], neighbour);
                if (t != null && !t.isVisited)
                {
                    listTileToCheck.Add(t);
                    t.isVisited = true;
                }
            }

            for(int j = 0; j < listTileToCheck.Count && !res; ++j)
            {
                tileContainer.transform.position = new Vector3(listTileToCheck[j].gameObject.transform.position.x, 0.2f, listTileToCheck[j].gameObject.transform.position.z);
                // 6 rotations
                for (int k = 0; k < 6 && !res; ++k)
                {
                    tileContainer.transform.Rotate(new Vector3(0f, (float)(k * (360f / 6f)), 0f));
                    foreach (Tile tile in listTileToBuild)
                    {
                        Tile tmp = GetTileAtWorldPos(tile.transform);
                        if (tmp == null)
                            break;

                        tile.pos = tmp.pos;
                    }

                    if (CanBuild(listTileToBuild, listSpawn, tileNeededToBeReashable, false, isThereASpawner))
                    {
                        foreach (Tile tile in listTileToBuild)     // affD
                        {
                            Debug.Log("CAN BUILD HERE : " + tile.pos);
                        }
                        res = true;
                    }
                }

                // undo rotation
                tileContainer.transform.rotation = startRot;
                // undo translation
                tileContainer.transform.position = startPos;
            }

            ++i;
        }

        UnvisiteAll();

        return res;
    }

    public Tile GetTileAtWorldPos(Transform pos)
    {
        Tile res = null;
        RaycastHit[] hits = Physics.RaycastAll(pos.transform.position, pos.TransformDirection(-Vector3.up), Mathf.Infinity, LayerMask.GetMask("MapComponent"));
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                Tile t = hit.transform.GetComponent<Tile>();
                if (t != null)
                {
                    res = t;
                    break;
                }
            }
        }

        return res;
    }

    /**
     * the tiles listTileToBuild need to fit the coordinate
     * return true if built
     */
    private bool CanBuild(List<Tile> listTileToBuild, List<Tile> listSpawn, Tile tileNeededToBeReashable, bool needToBuild, bool isThereASpawner)
    {
        bool res = true;

        // check if can build walls
        foreach (Tile t in listTileToBuild)
        {
            if (!tilesDictionary.ContainsKey(t.pos) || (tilesDictionary[t.pos].content != null && t.content != null))
            {
                res = false;
            }
        }

        if (res)
        {
            // temporary build the walls
            List<Tile> listTileToUndoChange = new List<Tile>();
            foreach (Tile t in listTileToBuild)
            {
                Tile currentTile = tilesDictionary[t.pos];
                listTileToUndoChange.Add(currentTile);
                currentTile.content = t.content;
                
                if (isThereASpawner && t.content != null)
                {
                    EnemySpawner es = t.content.GetComponent<EnemySpawner>();
                    if(es != null)
                    {
                        listSpawn.Add(currentTile);
                    }
                }
            }

            // check if tileNeededToBeReashable is reashable by listSpawn tiles
            res = AreTilesReashableFrom(listSpawn, tileNeededToBeReashable);
            // if not, cancel changes
            if (!res || !needToBuild)
            {
                foreach (Tile t in listTileToUndoChange)
                {
                    t.content = null;
                }
            }
            else
            {
                // if we can, change the transform and the position of the walls
                for (int i = 0; i < listTileToBuild.Count; ++i)
                {
                    if (listTileToBuild[i].content != null)
                    {
                        listTileToUndoChange[i].content.transform.parent = listTileToUndoChange[i].transform;
                        listTileToUndoChange[i].content.transform.position = listTileToUndoChange[i].center;

                        // remove the tiles with a content from the emptyTileDictionary
                        emptyTileDictionary.Remove(listTileToUndoChange[i].pos);
                    }
                }
            }
        }

        return res;
    }

    private void BuildDictionary()
    {
        tilesDictionary.Clear();
        foreach(Transform tileChild in tilesContainer)
        {
            Tile t = tileChild.gameObject.GetComponent<Tile>();
            if (t != null)
            {
                tilesDictionary.Add(t.pos, t);
                if (t.content == null)
                {
                    emptyTileDictionary.Add(t.pos, t);
                }
            }
        }
    }

    /**
     * ------------------------------------
     * ----------- PATH FINDING -----------
     * ------------------------------------ 
     */

    private bool AreTilesReashableFrom(List<Tile> listTileToReach, Tile startPosTile)
    {
        LaunchWave(startPosTile);

        bool res = true;
        int i = 0;
        while (res && i < listTileToReach.Count)
        {
            if (!listTileToReach[i].isWet)
                res = false;
            ++i;
        }

        DryAll();

        return res;
    }

    private Stack<Tile> GetShortestPath(Tile from, Tile to)
    {
        Search(from, ExpandSearchWalk);

        Stack<Tile> path = new Stack<Tile>();
        Tile next = to;
        while (next != null)
        {
            path.Push(next);
            // change the color of the tile (TODO : remove)
            next.ChangeColorToGreen();

            next = next.prev;
        }

        return path;
    }

    private List<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();
        //We need two lists, one for the tiles we need to check and one for the next tiles
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        start.distance = 0;
        checkNow.Enqueue(start);

        while (checkNow.Count > 0)
        {
            Tile tile = checkNow.Dequeue();
            for (int i = 0; i < 6; ++i)
            {
                Tile next = GetNeighbour(tile, (Neighbour) i);

                if (next != null && next.distance > tile.distance + 1) // exists or we already pass it with bigger distance if (next == null || next.distance <= t.distance + 1)
                {
                    if (addTile(tile, next))
                    {
                        next.distance = tile.distance + 1;
                        next.prev = tile;

                        checkNext.Enqueue(next);
                        retValue.Add(next);
                    }
                }
            }

            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkNext);
        }

        return retValue;
    }

    private void LaunchWave(Tile from)
    {
        Queue<Tile> queue = new Queue<Tile>();
        queue.Enqueue(from);

        while(queue.Count > 0)
        {
            Tile currentTile = queue.Dequeue();
            foreach(Neighbour n in (Neighbour[]) Enum.GetValues(typeof(Neighbour)))
            {
                Tile neighbourTile = GetNeighbour(currentTile, n);
                if (neighbourTile != null && !neighbourTile.isWet &&
                    (neighbourTile.content == null || neighbourTile.content is WalkableTileContent))
                {
                    //neighbourTile.ChangeColorToBlue(); // todo remove
                    neighbourTile.isWet = true;
                    queue.Enqueue(neighbourTile);
                }
            }
        }
    }

    private void DryAll()
    {
        foreach(Tile t in tilesDictionary.Values)
        {
            t.isWet = false;
        }
    }

    private void UnvisiteAll()
    {
        foreach(Tile t in tilesDictionary.Values)
        {
            t.isVisited = false;
        }
    }

    private void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }

    private void ClearSearch()
    {
        foreach (Tile tile in tilesDictionary.Values)
        {
            tile.Reset();
        }
    }

    private bool ExpandSearchWalk(Tile from, Tile to)
    {
        // Skip if the tile is occupied by something
        return (to.content == null || to.content is WalkableTileContent);
    }

    private Tile GetNeighbour(Tile tile, Neighbour neighbour)
    {
        Tile res = null;
        Point pos = tile.pos;
        Point newPos = new Point();
        switch (neighbour)
        {
            case Neighbour.TopRight:
                newPos = pos + new Point(0, 1);
                break;

            case Neighbour.Right:
                newPos = pos + new Point(1, 0);
                break;

            case Neighbour.BottomRight:
                newPos = pos + new Point(1, -1);
                break;

            case Neighbour.BottomLeft:
                newPos = pos + new Point(0, -1);
                break;

            case Neighbour.Left:
                newPos = pos + new Point(-1, 0);
                break;

            case Neighbour.TopLeft:
                newPos = pos + new Point(-1, 1);
                break;
        }

        if (tilesDictionary.ContainsKey(newPos))
            res = tilesDictionary[newPos];

        return res;
    }

    private enum Neighbour
    {
        TopRight,
        Right,
        BottomRight,
        BottomLeft,
        Left,
        TopLeft,
    }
}
