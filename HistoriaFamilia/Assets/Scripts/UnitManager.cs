using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour {

	// utility variable - holds the GameBoard's children under one root node in unity's "Hierarchy" window.
	private Transform _unitsHolder;
	[SerializeField][Tooltip("Stores which units are currently alive and fighting on the battlefield. Note: Do not use this field. Instead define units using Spawn Data field.")]
    private List <GameObject> AllUnits = null;

	[HideInInspector]
    public BoardManager BoardManager;

	[Tooltip("Define what unit types do exist and which attribute values they have.")]
	public UnitType[] UnitTypes;	//defined in inspector.


	[Tooltip("Define where what kind of unit will spawn at the beginning of a match.")]
	public List <UnitSpawnData> SpawnData;


	//note: each unit prefab can have its click handler which will inform the map to mark it as selected.
	[SerializeField][Tooltip("Hint: Leave this empty.")]
	private GameObject SelectedUnit;

     void Start()
    {
		PreventEnumToIndexMappingErrorOFUnitTypes();
		InitializeUnitsOnMapCreation();
    }

	// asserts correct mapping from UnitType.UnitArcheType to UnitType[]'s index 
	// and prevents duplicates of the same UnitType.UnitArcheType value.
	// Note: If a designer wants to add two types of e.g. Archers such as Longbow- and CompositeBowArchers,  
	// extend the UnitType.UnitArcheType enum.
	private void PreventEnumToIndexMappingErrorOFUnitTypes()
	{
		Debug.Log("Entering Prevent Index error of Units");
		UnitTypes = UnitTypes.OrderBy(ut => ut.Unit_Type)
					.DistinctBy(ut => ut.Unit_Type)
					.ToArray();
		foreach(UnitType unit in UnitTypes)
		{
			Debug.Log(unit.Unit_Type + "-" + unit.MovementReach);
		}
	}

	// -------------------------- Unit Creation ---------------------------------
	private void InitializeUnitsOnMapCreation()
	{
		_unitsHolder = new GameObject("Units").transform;
		foreach (UnitSpawnData usd in SpawnData)
		{
			CreateUnitAt(usd.SpawnPosX, usd.SpawnPosY, usd.Unit_Type);
		}
	}

	//eventually public if fabrics will call CreateUnitAt().
	private void CreateUnitAt(int x, int y, UnitType.UnitArcheType unitType)
	{
		//???: rename unitType to unitTypeCode?

		//instantiate at x, y as UnitArcheType unitType such as Archer, Infantry or others:
		Debug.Log(x + "-" + y + " is " + unitType);
		UnitType ut = UnitTypes[ (int) unitType ];
		GameObject go = Instantiate(ut.UnitVisualPrefab, new Vector3(x, y, 0), 
			Quaternion.identity) as GameObject;
		go.transform.SetParent(_unitsHolder);

		// Initialize Unit script.
		Unit unitScript = go.GetComponent<Unit>();
		unitScript.TileX = (int)go.transform.position.x;
        unitScript.TileY = (int)go.transform.position.y;
        unitScript.Map = BoardManager;
		//Initialize ClickOnUnitHandler.
		go.GetComponent<ClickOnUnitHandler>().UnitManager = this;

		//add unit to AllUnits
		AllUnits.Add(go);
	}


	// -------------------------- Unit Selection ---------------------------------
	private void UpdateSelectedUnitValues()
    {
        // Set Position of selected unit.
        Unit unit = SelectedUnit.GetComponent<Unit>();
        unit.TileX = (int)SelectedUnit.transform.position.x;
        unit.TileY = (int)SelectedUnit.transform.position.y;
    }

	public bool HasUnitOnTile(int x, int y)
	{
		return AllUnits.Any(unit => unit.GetComponent<Unit>().TileX == x && unit.GetComponent<Unit>().TileY == y);
	}

	public GameObject ChooseUnitAsSelectedOnTile(int x, int y)
	{
		SelectedUnit = AllUnits.Where(unit => unit.GetComponent<Unit>().TileX == x && unit.GetComponent<Unit>().TileY == y)
			.First();
		return SelectedUnit;
	}

	public bool IsUnitSelected()
	{
		return SelectedUnit != null;
	}

	public void DeselectUnit()
	{
		SelectedUnit = null;
	}

	public GameObject GetSelectedUnit()
	{
		return SelectedUnit;
	}
}
