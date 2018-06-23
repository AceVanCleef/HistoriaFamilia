using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make this class visibile in the inspector. IMPORTANT: remove MONOBEHAVIOR. Otherwise 
// This class won't show up in the inspector.
[System.Serializable]
public class TileType {

	//Doesn't get attached to a unity game object -> no MonoBehavior.
	
	// Note: For now, aslong factory pattern does not work, keep the order in the inspector 
	// IDENTICAL to the order of the enum values because the enum values are actually the indices of
	// the TileTypes[] in BoardManager.
	public enum TopographicalFeature {
		Grassland,
		Forest,
		Mountains
	}

	public TopographicalFeature Topography;
	public GameObject TileVisualPrefab;

	[Range(1, 10)]
	public int MovementCost = 1;
	[Range(-1, 5)]
	public int TerrainDefenseValue = 1;

	// Add more attributes such as movement costs/fuel consumption
}
