using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MountainsAttributes : TileTypeAttributes {

	public string Name = "Mountains";
	public float MovementCost = 99999;
	public bool IsWalkable = false;
}
