using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make this class visibile in the inspector. IMPORTANT: remove MONOBEHAVIOR. Otherwise 
// This class won't show up in the inspector.
[System.Serializable]
public class TileType {

	//Doesn't get attached to a unity game object -> no MonoBehavior.
	
	public string Name;
	public GameObject TileVisualPrefab;

	public float MovementCost = 1;

	public bool IsWalkable = true;

	// Add more attributes such as movement costs/fuel consumption
}
