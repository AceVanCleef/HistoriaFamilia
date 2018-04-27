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

	//public string Name;public string Name;
	public TopographicalFeature Topography;
	public GameObject TileVisualPrefab;

	/*
	// Tried to implement factory pattern for tile attributes such as cover, fuel consumption and iswalkable for unittype x etc.
	public TileTypeAttributes TileAttributes;

	public TileType()
	{
		switch (Topography)
		{
			case TopographicalFeature.Grassland: 
				TileAttributes = new GrasslandAttributes();
				Debug.Log("creating grassland");
				break;
			case TopographicalFeature.Forest: 
				TileAttributes = new ForestAttributes();
				Debug.Log("creating forest");
				break;
			case TopographicalFeature.Mountains: 
				TileAttributes = new MountainsAttributes();
				Debug.Log("creating mountains");
				break;
			default: 
				TileAttributes = null;
				break;
		}
	}
	*/


	public float MovementCost = 1;

	public bool IsWalkable = true;

	// Add more attributes such as movement costs/fuel consumption
}
