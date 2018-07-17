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

	[Range(1, 10)]
	public int MaxHealth = 10;
	[Range(1, 10)]
	public int MovementReach = 6;
	[Range(0f, 10f)]
	public float BaseAttackPower = 5;
	//Todo: make MaxAttackRange slider's lower boundary value equal to MinAttackRange silder's upper boundary value + 1. this must be dynamically adjusted.
	[Range(1, 10)][Tooltip("1 => Melee Unit. Any value > 1 => ranged unit.")]
	public int MaxAttackRange = 1;
	[Range(0, 10)][Tooltip("Default value: 0. Set this to a higher value if this unit type has a blind spot where it can't shoot to. For melee units, leave this at 0.")]
	public int MinAttackRange = 0;
 	public List<TileType.TopographicalFeature> ProhibitedToWalkOn;
}
