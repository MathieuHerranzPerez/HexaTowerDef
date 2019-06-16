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

    void Start()
    {
        BuildDictionary();

        NotifyUpdateWall();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Point p1 = new Point(0, 0);
            Point p2 = new Point(13, 21);
            GetShortestPath(tilesDictionary[p1], tilesDictionary[p2]);
        }
    }

    public void NotifyUpdateWall()
    {
        surface.BuildNavMesh();
    }


    private void BuildDictionary()
    {
        tilesDictionary.Clear();
        foreach(Transform tileChild in tilesContainer)
        {
            Tile t = tileChild.gameObject.GetComponent<Tile>();
            tilesDictionary.Add(t.pos, t);
        }
    }

    /**
     * ------------------------------------
     * ----------- PATH FINDING -----------
     * ------------------------------------ 
     */

    private bool IsThereAPath(Tile from, Tile to)
    {
        Search(from, ExpandSearchWalk);

        return to.prev != null;
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

                        // todo remove
                        next.ChangeColor(0.02f * (float)next.distance);

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
        return (to.content == null || to.content is EmptyTileElement);
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
