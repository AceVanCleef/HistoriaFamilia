using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player {
	
	public int PlayerID;
	public int TeamID;
	public bool IsAIControlled = false;
	[Tooltip("Define where what kind of unit will spawn at the beginning of a match.")]
	public List<UnitSpawnData> SpawnData;
	[Tooltip("Stores which units are currently alive and fighting on the battlefield. Note: Do not use this field. Instead define units using Spawn Data field.")]
	public List<GameObject> AllUnits;
	
}
