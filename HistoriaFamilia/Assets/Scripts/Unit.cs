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
	}
	public void MoveToTargettile()
	{
		// timing: https://answers.unity.com/questions/425477/wait-for-5-seconds-.html
		float timer = 0.0f;
		float timerMax = 3.0f;
		float deltaTime = 3.0f;

		//Todo: timing DOES NOT WORK yet.

		while (CurrentPath != null)
		{
		/*
			timer += Time.deltaTime;
			if(timer >= timerMax)
			{
				Debug.Log("entered");
				MoveToNextTile();
				timerMax = timerMax + deltaTime;
			}
			*/
			MoveToNextTile();
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
		}
	}
}
