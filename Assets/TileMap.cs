using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileMap : MonoBehaviour
{

    public GameObject selectedUnit;
    public GameObject spawnUnit;

    public TileType[] tileTypes;

    int[,] tiles;
    Node[,] graph;

    //use to move rows up(x) or right(y).
    int leftBorder = 5;
    int rightBorder = 1;
    
    int topBorder = 3;
    int bottomBorder = 3;
        
    //specify map size including border +6x,6y (3 tiles each side)
    public int mapSizeX = 16;
    public int mapSizeY = 11;

    protected int totalMapSizeX;
    protected int totalMapSizeY;

    //position to travel to
    public int tileFinishX = 7;
    public int tileFinishY = 4;

    void Start()
    {

        // Setup the selectedUnit's variable
        selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
        selectedUnit.GetComponent<Unit>().tileY = (int)selectedUnit.transform.position.y;
        selectedUnit.GetComponent<Unit>().map = this;

        totalMapSizeX = mapSizeX + leftBorder + rightBorder; //22
        totalMapSizeY = mapSizeY + topBorder + bottomBorder; //17

        GenerateMapData();
        GeneratePathfindingGraph();
        GenerateMapVisual();

    }

    public void SpawnWave()
    {

        //TODO FIX Instantiate
         Instantiate(spawnUnit, new Vector3(20, 8, 0), spawnUnit.transform.rotation);


        // Taken from clickable tile, calls the move pawn function
        GeneratePathTo(tileFinishX, tileFinishY);
    }

    void GenerateMapData()
    {
        // Allocate our map tiles
        tiles = new int[totalMapSizeX, totalMapSizeY];


        //tile location is [0,0] [row first, column second]
        int x, y;

        // Initialize our map tiles to be aisles
        for (x = 0; x < totalMapSizeX; x++)
        {
            for (y = 0; y < totalMapSizeY; y++)
            {
                tiles[x, y] = 0;
            }
        }

        //Hack
        // Make a wall border area left
        for (x = 0; x < leftBorder; x++)
        {
            for (y = 0; y < totalMapSizeY; y++)
            {
                tiles[x, y] = 3;
            }
        }

        // Make a wall border area right
        for (x = totalMapSizeX - rightBorder; x < totalMapSizeX; x++)
        {
            for (y = 0; y < totalMapSizeY; y++)
            {
                tiles[x, y] = 3;
            }
        }


        // Make a wall border area bottom
        for (y = 0; y < bottomBorder; y++)
        {
            for (x = 0; x < totalMapSizeX; x++)
            {
                tiles[x, y] = 3;
            }
        }

        // Make a wall border area top
        for (y = totalMapSizeY - topBorder; y < totalMapSizeY; y++)
        {
            for (x = 0; x < totalMapSizeX; x++)
            {
                tiles[x, y] = 3;
            }
        }

        //Tower Locations

        //Tower1
        tiles[1 , 2] = 4;
        //Tower2
        tiles[1, 5] = 5;
        //Tower3
        tiles[1, 8] = 6;
        //Tower4
        tiles[1, 11] = 7;
        //Tower5
        tiles[1, 14] = 8;

        //TowerNode1
        tiles[0 + leftBorder, 12 + bottomBorder] = 9;
        //TowerNode2
        tiles[2 + leftBorder, 1] = 10;
        //TowerNode3
        tiles[4 + leftBorder, 12 + bottomBorder] = 9;
        //TowerNode4
        tiles[6 + leftBorder, 1] = 10;
        //TowerNode5
        tiles[8 + leftBorder, 12 + bottomBorder] = 9;
        //TowerNode6
        tiles[10 + leftBorder, 1] = 10;
        //TowerNode7
        tiles[12 + leftBorder, 12 + bottomBorder] = 9;
        //TowerNode8
        tiles[14 + leftBorder, 1] = 10;

        //Hack

        //Row 14
        tiles[14 + leftBorder, 10 + bottomBorder] = 0;
        tiles[14 + leftBorder, 9 + bottomBorder] = 1;
        tiles[14 + leftBorder, 8 + bottomBorder] = 1;
        tiles[14 + leftBorder, 7 + bottomBorder] = 1;
        tiles[14 + leftBorder, 6 + bottomBorder] = 1;
        tiles[14 + leftBorder, 5 + bottomBorder] = 0;
        tiles[14 + leftBorder, 4 + bottomBorder] = 1;
        tiles[14 + leftBorder, 3 + bottomBorder] = 1;
        tiles[14 + leftBorder, 2 + bottomBorder] = 1;
        tiles[14 + leftBorder, 1 + bottomBorder] = 1;
        tiles[14 + leftBorder, 0 + bottomBorder] = 0;
        //Row 12
        tiles[12 + leftBorder, 10 + bottomBorder] = 2;
        tiles[12 + leftBorder, 9 + bottomBorder] = 1;
        tiles[12 + leftBorder, 8 + bottomBorder] = 1;
        tiles[12 + leftBorder, 7 + bottomBorder] = 1;
        tiles[12 + leftBorder, 6 + bottomBorder] = 1;
        tiles[12 + leftBorder, 5 + bottomBorder] = 2;
        tiles[12 + leftBorder, 4 + bottomBorder] = 1;
        tiles[12 + leftBorder, 3 + bottomBorder] = 1;
        tiles[12 + leftBorder, 2 + bottomBorder] = 1;
        tiles[12 + leftBorder, 1 + bottomBorder] = 1;
        tiles[12 + leftBorder, 0 + bottomBorder] = 0;
        //Row 10
        tiles[10 + leftBorder, 10 + bottomBorder] = 2;
        tiles[10 + leftBorder, 9 + bottomBorder] = 1;
        tiles[10 + leftBorder, 8 + bottomBorder] = 1;
        tiles[10 + leftBorder, 7 + bottomBorder] = 1;
        tiles[10 + leftBorder, 6 + bottomBorder] = 1;
        tiles[10 + leftBorder, 5 + bottomBorder] = 0;
        tiles[10 + leftBorder, 4 + bottomBorder] = 1;
        tiles[10 + leftBorder, 3 + bottomBorder] = 1;
        tiles[10 + leftBorder, 2 + bottomBorder] = 1;
        tiles[10 + leftBorder, 1 + bottomBorder] = 1;
        tiles[10 + leftBorder, 0 + bottomBorder] = 2;
        //Row 8
        tiles[8 + leftBorder, 10 + bottomBorder] = 0;
        tiles[8 + leftBorder, 9 + bottomBorder] = 1;
        tiles[8 + leftBorder, 8 + bottomBorder] = 1;
        tiles[8 + leftBorder, 7 + bottomBorder] = 1;
        tiles[8 + leftBorder, 6 + bottomBorder] = 1;
        tiles[8 + leftBorder, 5 + bottomBorder] = 2;
        tiles[8 + leftBorder, 4 + bottomBorder] = 1;
        tiles[8 + leftBorder, 3 + bottomBorder] = 1;
        tiles[8 + leftBorder, 2 + bottomBorder] = 1;
        tiles[8 + leftBorder, 1 + bottomBorder] = 1;
        tiles[8 + leftBorder, 0 + bottomBorder] = 2;
        //Row 6
        tiles[6 + leftBorder, 10 + bottomBorder] = 2;
        tiles[6 + leftBorder, 9 + bottomBorder] = 1;
        tiles[6 + leftBorder, 8 + bottomBorder] = 1;
        tiles[6 + leftBorder, 7 + bottomBorder] = 1;
        tiles[6 + leftBorder, 6 + bottomBorder] = 1;
        tiles[6 + leftBorder, 5 + bottomBorder] = 0;
        tiles[6 + leftBorder, 4 + bottomBorder] = 1;
        tiles[6 + leftBorder, 3 + bottomBorder] = 1;
        tiles[6 + leftBorder, 2 + bottomBorder] = 1;
        tiles[6 + leftBorder, 1 + bottomBorder] = 1;
        tiles[6 + leftBorder, 0 + bottomBorder] = 2;
        //Row 4
        tiles[4 + leftBorder, 10 + bottomBorder] = 2;
        tiles[4 + leftBorder, 9 + bottomBorder] = 1;
        tiles[4 + leftBorder, 8 + bottomBorder] = 1;
        tiles[4 + leftBorder, 7 + bottomBorder] = 1;
        tiles[4 + leftBorder, 6 + bottomBorder] = 1;
        tiles[4 + leftBorder, 5 + bottomBorder] = 2;
        tiles[4 + leftBorder, 4 + bottomBorder] = 1;
        tiles[4 + leftBorder, 3 + bottomBorder] = 1;
        tiles[4 + leftBorder, 2 + bottomBorder] = 1;
        tiles[4 + leftBorder, 1 + bottomBorder] = 1;
        tiles[4 + leftBorder, 0 + bottomBorder] = 0;
        //Row 2
        tiles[2 + leftBorder, 10 + bottomBorder] = 0;
        tiles[2 + leftBorder, 9 + bottomBorder] = 1;
        tiles[2 + leftBorder, 8 + bottomBorder] = 1;
        tiles[2 + leftBorder, 7 + bottomBorder] = 1;
        tiles[2 + leftBorder, 6 + bottomBorder] = 1;
        tiles[2 + leftBorder, 5 + bottomBorder] = 0;
        tiles[2 + leftBorder, 4 + bottomBorder] = 1;
        tiles[2 + leftBorder, 3 + bottomBorder] = 1;
        tiles[2 + leftBorder, 2 + bottomBorder] = 1;
        tiles[2 + leftBorder, 1 + bottomBorder] = 1;
        tiles[2 + leftBorder, 0 + bottomBorder] = 2;
        //Row 0
        tiles[0 + leftBorder, 10 + bottomBorder] = 0;
        tiles[0 + leftBorder, 9 + bottomBorder] = 1;
        tiles[0 + leftBorder, 8 + bottomBorder] = 1;
        tiles[0 + leftBorder, 7 + bottomBorder] = 1;
        tiles[0 + leftBorder, 6 + bottomBorder] = 1;
        tiles[0 + leftBorder, 5 + bottomBorder] = 1;
        tiles[0 + leftBorder, 4 + bottomBorder] = 1;
        tiles[0 + leftBorder, 3 + bottomBorder] = 1;
        tiles[0 + leftBorder, 2 + bottomBorder] = 1;
        tiles[0 + leftBorder, 1 + bottomBorder] = 1;
        tiles[0 + leftBorder, 0 + bottomBorder] = 0;
    }

    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {

        TileType tt = tileTypes[tiles[targetX, targetY]];

        if (UnitCanEnterTile(targetX, targetY) == false)
            return Mathf.Infinity;

        float cost = tt.movementCost;

        if (sourceX != targetX && sourceY != targetY)
        {
            // We are moving diagonally!  Fudge the cost for tie-breaking
            // Purely a cosmetic thing!
            cost += 0.001f;
        }

        return cost;

    }

    void GeneratePathfindingGraph()
    {
        // Initialize the array
        graph = new Node[totalMapSizeX, totalMapSizeY];

        // Initialize a Node for each spot in the array
        for (int x = 0; x < totalMapSizeX; x++)
        {
            for (int y = 0; y < totalMapSizeY; y++)
            {
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;
            }
        }

        // Now that all the nodes exist, calculate their neighbours
        for (int x = 0; x < totalMapSizeX; x++)
        {
            for (int y = 0; y < totalMapSizeY; y++)
            {

                // This is the 4-way connection version:
                if (x > 0)
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                if (x < totalMapSizeX - 1)
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                if (y > 0)
                    graph[x, y].neighbours.Add(graph[x, y - 1]);
                if (y < totalMapSizeY - 1)
                    graph[x, y].neighbours.Add(graph[x, y + 1]);


                // This is the 8-way connection version (allows diagonal movement)
                // Try left
                //if(x > 0) {
                //	graph[x,y].neighbours.Add( graph[x-1, y] );
                //	if(y > 0)
                //		graph[x,y].neighbours.Add( graph[x-1, y-1] );
                //	if(y < totalMapSizeY-1)
                //		graph[x,y].neighbours.Add( graph[x-1, y+1] );
                //}

                //// Try Right
                //if(x < totalMapSizeX-1) {
                //	graph[x,y].neighbours.Add( graph[x+1, y] );
                //	if(y > 0)
                //		graph[x,y].neighbours.Add( graph[x+1, y-1] );
                //	if(y < totalMapSizeY-1)
                //		graph[x,y].neighbours.Add( graph[x+1, y+1] );
                //}

                //// Try straight up and down
                //if(y > 0)
                //	graph[x,y].neighbours.Add( graph[x, y-1] );
                //if(y < totalMapSizeY-1)
                //	graph[x,y].neighbours.Add( graph[x, y+1] );

                // This also works with 6-way hexes and n-way variable areas (like EU4)
            }
        }
    }

    void GenerateMapVisual()
    {
        for (int x = 0; x < totalMapSizeX; x++)
        {
            for (int y = 0; y < totalMapSizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];

                GameObject go;

                if (tt.isTower) // check if tower and move by .5
                {

                    float xAdj = x + 0.5f;
                    //float yAdj = y + 0.0f;

                    go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(xAdj, y, 0), tt.tileVisualPrefab.transform.rotation); //Rotation of prefab
                }
                else
                {
                    go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity); //Quaternion.identity disables any rotation
                }


                ClickableTile ct = go.GetComponent<ClickableTile>();
                ct.tileX = x;
                ct.tileY = y;
                ct.map = this;

                //TODO
                //Set Tower tag using prefab tile type not needed but maybe use for rows??
                /*switch (tiles[x, y])
                {
                    case 8:
                        go.tag = "Tower5";
                        break;
                    case 7:
                        go.tag = "Tower4";
                        break;
                    case 6:
                        go.tag = "Tower3";
                        break;
                    case 5:
                        go.tag = "Tower2";
                        break;
                    case 4:
                        go.tag = "Tower1";
                        break;
                    default:
                        break;
                }*/
            }

        }
    }

    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(x, y, 0);
    }

    public bool UnitCanEnterTile(int x, int y)
    {

        // We could test the unit's walk/hover/fly type against various
        // terrain flags here to see if they are allowed to enter the tile.

        return tileTypes[tiles[x, y]].isWalkable;
    }

    public void GeneratePathTo(int x, int y)
    {
        // Clear out our unit's old path.
        selectedUnit.GetComponent<Unit>().currentPath = null;

        if (UnitCanEnterTile(x, y) == false)
        {
            // We probably clicked on a mountain or something, so just quit out.
            return;
        }

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        // Setup the "Q" -- the list of nodes we haven't checked yet.
        List<Node> unvisited = new List<Node>();

        Node source = graph[
                            selectedUnit.GetComponent<Unit>().tileX,
                            selectedUnit.GetComponent<Unit>().tileY
                            ];

        Node target = graph[
                            x,
                            y
                            ];

        dist[source] = 0;
        prev[source] = null;

        // Initialize everything to have INFINITY distance, since
        // we don't know any better right now. Also, it's possible
        // that some nodes CAN'T be reached from the source,
        // which would make INFINITY a reasonable value
        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            // "u" is going to be the unvisited node with the smallest distance.
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if (u == target)
            {
                break;  // Exit the while loop!
            }

            unvisited.Remove(u);

            foreach (Node v in u.neighbours)
            {
                //float alt = dist[u] + u.DistanceTo(v);
                float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        // If we get there, the either we found the shortest route
        // to our target, or there is no route at ALL to our target.

        if (prev[target] == null)
        {
            // No route between our target and the source
            return;
        }

        List<Node> currentPath = new List<Node>();

        Node curr = target;

        // Step through the "prev" chain and add it to our path
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        // Right now, currentPath describes a route from out target to our source
        // So we need to invert it!

        currentPath.Reverse();

        selectedUnit.GetComponent<Unit>().currentPath = currentPath;
    }

}
