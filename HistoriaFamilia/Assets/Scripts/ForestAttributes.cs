using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForestAttributes : TileTypeAttributes {

	//public string Name = "Forest";

	public string Name 
	{
		get { return "Forest"; }
	}

	public float MovementCost = 3;
	public bool IsWalkable = true;
}
