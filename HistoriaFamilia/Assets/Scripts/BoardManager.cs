using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//to allow [Serializable] which allows showing variables of subclasses in the inspector.
using System;

public class BoardManager : MonoBehaviour {

	// utility variable - holds the GameBoard's children under one root node in unity's "Hierarchy" window.
	private Transform _boardHolder;

	public UnitManager UnitManager;

	public TileType[] TileTypes;	//defined in inspector.
	//Idee für Indexsicherheit: public Dictionary<TileType.TopographicalFeature, TileType> TileTypez;

	// stores the tile type key.
	int[,] _tiles = null;

	public int BoardSizeX = 10;
	public int BoardSizeY = 10;

	// Pathfinding
	Node[,] graph;
	List<Node> currentPath = null;

	void Start () {

        GenerateMapData();
		GeneratePathfindingGraph();
		GenerateMapVisuals();
	}

	// asserts correct mapping from TileType.TopographicalFeature to TileType[]'s index 
	// and prevents duplicates of the same TileType.TopographicalFeature value.
	// Note: If a designer wants to add two types of e.g. Mountains such as IcyMountains and Volcanos,  
	// extend the TileType.TopographicalFeature enum.
	private void PreventEnumToIndexMappingErrorOFTileTypes()
	{
		Debug.Log("Entering Prevent Index error");
		TileTypes = TileTypes.OrderBy(tt => tt.Topography)
						.DistinctBy(tt => tt.Topography)
						.ToArray();
		foreach(TileType tile in TileTypes)
		{
			Debug.Log(tile.Topography + "-" + tile.MovementCost);
		}
	}

	void GenerateMapData()
	{
		PreventEnumToIndexMappingErrorOFTileTypes();
		_tiles = new int[BoardSizeX,BoardSizeY];
		
		//initialize our map tiles
		for( int x = 0; x < BoardSizeX; ++x)
		{
			for( int y = 0; y < BoardSizeY; ++y)
			{
				_tiles[x, y] = 0;
			}
		}

		// creates U - shaped mountain range.
		_tiles[4, 4] = 2;
		_tiles[5, 4] = 2;
		_tiles[6, 4] = 2;
		_tiles[7, 4] = 2;
		_tiles[8, 4] = 2;
		
		_tiles[8, 5] = 2;
		_tiles[8, 6] = 2;

		_tiles[4, 5] = 2;
		_tiles[4, 6] = 2;

		//create swamp region
		for( int x = 3; x < 5; ++x)
		{
			for( int y = 0; y < 4; ++y)
			{
				_tiles[x, y] = 1;
			}
		}
	}


	public bool UnitCanEnterTile(int x, int y) {
        TileType tt = TileTypes[_tiles[x, y]];

        // We could test the unit's walk/hover/fly type against various
        // terrain flags here to see if they are allowed to enter the tile.

        Debug.Log("allowed to walk on:" + tt.Topography + "? " + UnitManager.IsAllowedToWalk(tt));
        return UnitManager.IsAllowedToWalk(tt);
	}

	float CostToEnterTile(int x, int y)
	{
		TileType tt = TileTypes[ _tiles[x, y] ];

		//prevents walking into mountains.
		if ( UnitCanEnterTile(x, y) == false ) return Mathf.Infinity;

		return tt.MovementCost;
	}
	
	/// describes all the possible connections between the tiles.
	void GeneratePathfindingGraph()
	{
		// initialize the array.
		graph = new Node[BoardSizeX, BoardSizeY];
		// Initialize each Node (or element of the array).
		for (int x = 0; x < BoardSizeX; ++x)
		{
			for (int y = 0; y < BoardSizeY; ++y)
			{
				graph[x,y] = new Node(x, y);
			}
		}

		//register all neighbours for each tile.
		for (int x = 0; x < BoardSizeX; ++x)
		{
			for (int y = 0; y < BoardSizeY; ++y)
			{
				// We have a 4-way connected map.
				// This is also works with 6-way hexes, 8-way tiles and n-way variable areas (like EU4)

				// Add neighbours. Note: Only tiles within [1, BoardSizeX - 1] will have horizontal neighbours. 
				// Same goes for y-Axis vertically. 
				// Hint: In games like CIV, the tile[0,y] could have tile[BoardSizex-1,y] as
				// its left neighbour. But we don't do that here.'
				if( x > 0)
					graph[x,y].Neighbours.Add ( graph[x - 1, y] );	//left
				if( x < BoardSizeX - 1)
					graph[x,y].Neighbours.Add ( graph[x + 1, y] );	//right
				if( y > 0)
					graph[x,y].Neighbours.Add ( graph[x, y - 1] );	//above
				if( y < BoardSizeY - 1)
					graph[x,y].Neighbours.Add ( graph[x, y + 1] );	//below

				// IF YOU WANT TO HAVE 8-DIAGONAL MOVEMENT SYSTEM, ADD MORE NEIGHBOURS HERE by using if(x....) then graph[x,y].Neighbours.Add(...).
			}
		}
	}
	
	/// allocates visual prefabs
	void GenerateMapVisuals()
	{
		//create root node of gameBoard.
		_boardHolder = new GameObject("Board").transform;
		//setup floor tiles
		for( int x = 0; x < BoardSizeX; ++x)
		{
			for( int y = 0; y < BoardSizeY; ++y )
			{
				// _tiles[x,y] stores the index value which indicades which tile type it should have.
				// E.g. 0 = Grass, 1 = Swamp, 2 = Mountains.
				TileType tt = TileTypes[ _tiles[x,y] ];
				GameObject go = Instantiate(tt.TileVisualPrefab, new Vector3(x,y), Quaternion.identity) as GameObject;
				go.transform.SetParent(_boardHolder);
				//sorting layers so that ClickOnUnitHandler can detect units instead of tiles when SelectedUnit != null.
				// Note that camera is on z = -10f and underlying layers must have a z-value larger than 0 while 
				// closer to the camera ones lower than 0 (negative values).
				// In Short: z > 0 --> further away from camera; z < 0 --> closer to camera.
				// This is of utmost importance regarding OnMouseDown() - event detection.
				go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, 0.05f);

				//#UserInput: Let click handler of tile prefab know which tile it is related to (position information):
				ClickOnTileHandler coth = go.GetComponent<ClickOnTileHandler>();
				// where is this tile?
				coth.TilePositionX = x;
				coth.TilePositionY = y;
				// allow access to GameBoard's script --> required for access to MoveSelectedUnitTo() 
				// within coth.
				coth.Map = this; //this script component.
				coth.UnitManager = UnitManager;
				//enable tile hovering effect (color fill)
				coth.SHM = go.GetComponentsInChildren<SpriteHighlightManager>()[0];
			}
		}
	}


	public Vector2 TileCoordToWorldCoord(int x, int y)
	{
		return new Vector2(x, y);
	}

	//this responsibiliity could also be the one of the Unit.
	public void GeneratePathTo(int x, int y)
	{	
		// clear out our unit's old path.
		UnitManager.GetSelectedUnit().GetComponent<Unit>().CurrentPath = null;


		//prevents walking into mountains.
		// Note: Other strategy is to check when clicking on a tile if it IsWalkable.
		if ( UnitCanEnterTile(x, y) == false ) return;


		// Pathfinding using the Dijkstra algorithm.

		//tracks distances of paths
		Dictionary<Node, float> dist = new Dictionary<Node, float>();
		//contains chain of nodes the unit have to walk through.
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
		// Nodes we haven't checked yet.'
		List<Node> unvisited = new List<Node>();


		Node source = graph[
			UnitManager.GetSelectedUnit().GetComponent<Unit>().TileX,
			UnitManager.GetSelectedUnit().GetComponent<Unit>().TileY
		];
		Node target = graph[x, y];


		//populate with data:
		dist[source] = 0; //distance of source = 0.
		prev[source] = null; //source is the starting point -> no previous node.

		// Initialize verything to have INFINITY distance, since we
		// don't know any better right now. Also, it is possible that some
		// Nodes can't be reached from the source which would make INFINITY 
		// a reasonable value.
		foreach (Node v in graph)
		{
			if (v != source)
			{
				dist[v] = Mathf.Infinity;
				prev[v] = null;
			}

			unvisited.Add (v);
		}


		while (unvisited.Count > 0)
		{
			// 'u' is going to be the unvisited node with the smallest distance.
			Node u = null;
			// grabs a neighbouring node.
			foreach (Node possibleU in unvisited) 
			{
				if (u == null || dist[possibleU] < dist[u]) u = possibleU;
			}

			//target tile found?
			if (u == target) break;

			unvisited.Remove(u);

			//calculates actual distance (replaces distance = Infinity) for all nodes which have a neighbour.
			foreach (Node v in u.Neighbours) 
			{
				//float alt = dist[u] + u.DistanceTo(v);
				float alt = dist[u] + CostToEnterTile(v.x, v.y);	//second summand influences whether mountains are avoided or not.
				if ( alt < dist[v] )
				{
					dist[v] = alt;
					prev[v] = u;
				}
			}
		}

		// if we get there, then either we found the shortest route 
		// to target or there is no route at all, which is possible.

		if (prev[target] == null)
		{
			// No route to target.
			return;
		}
		//Add found path and make it available...
		currentPath = new List<Node>();
		Node curr = target;
		//...by stepping through the 'prev' chain and add it to the path.
		while (curr != null)
		{
			currentPath.Add(curr);
			curr = prev[curr];
		}
		//right now, currentPath describes a route from our target to our source, 
		// so we need to invert it.
		currentPath.Reverse();
		UnitManager.GetSelectedUnit().GetComponent<Unit>().CurrentPath = currentPath;
	}

	// Notes about path finding:
	// Two algorightms are available: Dijkstra and A*.
	// Dijkstra is a bit slower than A*, but simpler to implement and also guarantees to find the goal.


	// -------------------------- Movement and Attack Range ---------------------------------

	public List<Node> GetValidMoves(int startPosX, int startPosY, int movePoints)
	{
		List<Node> root = new List<Node>();
		Debug.Log("In GetValidMoves - root");
		GetValidMoves(startPosX, startPosY, movePoints, root);
		return root;
	}

	private void GetValidMoves(int startPosX, int startPosY, int movePoints, List<Node> validMoves) {
		//How to: https://answers.unity.com/questions/1063687/how-do-i-highlight-all-available-paths-with-dijkst.html
		Node startTile = graph[startPosX, startPosY];
		validMoves.Add(startTile);
		for (int i = 0; i < startTile.Neighbours.Count; i++)
		{
			//get moveCost:
			Node currentNeighbour = startTile.Neighbours[i];
			TileType tt = TileTypes[_tiles[currentNeighbour.x, currentNeighbour.y]];
			int moveCost = tt.MovementCost;
			//calculate remaining ???
			int nextMoveCost = movePoints - moveCost;
			if (nextMoveCost >= 0 && !validMoves.Contains(startTile.Neighbours[i]))
				GetValidMoves(startTile.Neighbours[i].x, startTile.Neighbours[i].y, nextMoveCost, validMoves);
		}
	}

	public List<SpriteHighlightManager> GetSpriteHighlightManagersInRangeBy(List<Node> validMoves) {
		//Getting all children from _boardHolder: https://answers.unity.com/questions/594210/get-all-children-gameobjects.html
		List<SpriteHighlightManager> shmBag = new List<SpriteHighlightManager>();
		foreach (Transform child in _boardHolder.transform) {
			foreach (Node currentNode in validMoves) {
				if (currentNode.x == child.position.x && currentNode.y == child.position.y) {
					//add SHM to List<SHM>
					SpriteHighlightManager currentSHM = child.GetComponentsInChildren<SpriteHighlightManager>()[0];
					shmBag.Add(currentSHM);
				}
			}
		}
		return shmBag;
	}

} //This is the End
