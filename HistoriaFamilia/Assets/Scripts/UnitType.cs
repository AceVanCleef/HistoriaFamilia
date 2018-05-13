using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make this class visibile in the inspector. IMPORTANT: remove MONOBEHAVIOR. Otherwise 
// This class won't show up in the inspector.
[System.Serializable]
public class UnitType {

	//Doesn't get attached to a unity game object -> no MonoBehavior.
	
	// Note: For now, aslong factory pattern does not work, keep the order in the inspector 
	// IDENTICAL to the order of the enum values because the enum values are actually the indices of
	// the UnitTypes[] in BoardManager.
	[SerializeField]
	public enum UnitArcheType {
		Archer,
		Light_Infantry,
		Phalanx
	}

	public UnitArcheType Unit_Type;
	public GameObject UnitVisualPrefab;

	public float MaxHealth = 10;
	public float MovementReach = 6;
	public float BaseAttackPower = 5;
 	public List<TileType.TopographicalFeature> ProhibitedToWalkOn;
}
