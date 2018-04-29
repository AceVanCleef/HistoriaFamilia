using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	//Coordinates of tile in Unit Model.
	public int TileX;
	public int TileY;
	public BoardManager Map;

	//pathfinding: stores the path from this unit ( = source) to target Node
	public List<Node> CurrentPath = null;

	// activates from tile to tile Lerp - transition
	public bool IsWalking = false;

	void Update()
	{
		if (CurrentPath != null)
		{
			int currNode = 0;

			while (currNode < CurrentPath.Count - 1)
			{
				Vector3 start = Map.TileCoordToWorldCoord( CurrentPath[currNode].x, CurrentPath[currNode].y );
				Vector3 end = Map.TileCoordToWorldCoord( CurrentPath[currNode + 1].x, CurrentPath[currNode + 1].y );
				/*
				Debug.Log("start" + start.x + " - " + start.y);
				Debug.Log("end" + end.x + " - " + end.y);
				Debug.Log("--------------------------------------");
				*/
				Debug.DrawLine( start, end, Color.red, 0, false);
				currNode++;
			}
		}

		// Lerp - transition from tile to tile
		if (IsWalking)
		{
			if(Vector2.Distance(transform.position, Map.TileCoordToWorldCoord( TileX, TileY )) < 0.1f)
				MoveToNextTile();

			// Smoothly animate towards the correct map tile.
			transform.position = Vector2.Lerp(transform.position, Map.TileCoordToWorldCoord( TileX, TileY ), 5f * Time.deltaTime);
		}

	}

	public void MoveToTargettile()
	{
		//Todo: timing DOES NOT WORK yet.
		while (CurrentPath != null)
		{
			// Have we moved our visible piece close enough to the target tile that we can
			// advance to the next step in our pathfinding?
			Debug.Log(Vector2.Distance(transform.position, Map.TileCoordToWorldCoord( TileX, TileY )) < 0.1f);
			if(Vector2.Distance(transform.position, Map.TileCoordToWorldCoord( TileX, TileY )) < 0.1f)
				MoveToNextTile();

			// Smoothly animate towards the correct map tile.
			transform.position = Vector2.Lerp(transform.position, Map.TileCoordToWorldCoord( TileX, TileY ), 25f * Time.deltaTime);
			Debug.Log(transform.position);
		}
	}

	public void MoveToNextTile()
	{
		if (CurrentPath == null) return;
		
		//Remove the old current/first node from the path.
		CurrentPath.RemoveAt(0);

		//Now grab the new first node and move us to that position.
		TileX = CurrentPath[0].x;
		TileY = CurrentPath[0].y;
		transform.position = Map.TileCoordToWorldCoord( TileX, TileY );

		if (CurrentPath.Count == 1)
		{
			// We only have one tile left in the path, and that tile MUST be our ultimate
			// destination. So let's just clear our pathfinding info.
			CurrentPath = null;
			IsWalking = false;
		}
		Debug.Log("IsWalking = " + IsWalking);
	}

}
