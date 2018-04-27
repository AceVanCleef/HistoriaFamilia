using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TileTypeAttributes {

	public string Name = "none";
	public float MovementCost = -1;
	public bool IsWalkable = true;

}
