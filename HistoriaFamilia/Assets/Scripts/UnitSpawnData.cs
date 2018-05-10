using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make this class visibile in the inspector. IMPORTANT: remove MONOBEHAVIOR. Otherwise 
// This class won't show up in the inspector.
[System.Serializable]
public struct UnitSpawnData {
	public UnitType.UnitArcheType Unit_Type;
	public int SpawnPosX;
	public int SpawnPosY;

}
