using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

	// utility variable - holds the GameBoard's children under one root node in unity's "Hierarchy" window.
	private Transform _unitsHolder;
	[Tooltip("Stores which units are currently alive and fighting on the battlefield.")]
    public List <GameObject> AllUnits = null;

    public BoardManager BoardManager;

	[Tooltip("Define what unit types do exist and which attribute values they have.")]
	public UnitType[] UnitTypes;	//defined in inspector.


	[Tooltip("Define where what kind of unit will spawn at the beginning of a match.")]
	public List <UnitSpawnData> SpawnData;

	//for testing:
	public GameObject Unit1;
	public GameObject Unit2;
	public GameObject Unit3;

	//note: each unit prefab can have its click handler which will inform the map to mark it as selected.
	public GameObject SelectedUnit;

     void Start()
    {
		InitializeUnitsOnMapCreation();
		//will be replaced with initialize/createUnits()
		Unit1.GetComponent<Unit>().Map = BoardManager;
		Unit2.GetComponent<Unit>().Map = BoardManager;
		Unit3.GetComponent<Unit>().Map = BoardManager;
		Unit1.GetComponent<ClickOnUnitHandler>().UnitManager = this;
		Unit2.GetComponent<ClickOnUnitHandler>().UnitManager = this;
		Unit3.GetComponent<ClickOnUnitHandler>().UnitManager = this;
    }

	// -------------------------- Unit Creation ---------------------------------
	private void InitializeUnitsOnMapCreation()
	{
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
		_unitsHolder = new GameObject("Units").transform;
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
	}


	// -------------------------- Unit Selection ---------------------------------
	public void UpdateSelectedUnitValues()
    {
        // Set Position of selected unit.
        Unit unit = SelectedUnit.GetComponent<Unit>();
        unit.TileX = (int)SelectedUnit.transform.position.x;
        unit.TileY = (int)SelectedUnit.transform.position.y;
        //unit.Map = BoardManager;
    }

	public bool HasUnitOnTile(int x, int y)
	{
		if (Unit1.GetComponent<Unit>().TileX == x && Unit1.GetComponent<Unit>().TileY == y)
			return true;
		if (Unit2.GetComponent<Unit>().TileX == x && Unit2.GetComponent<Unit>().TileY == y)
			return true;
		if (Unit3.GetComponent<Unit>().TileX == x && Unit3.GetComponent<Unit>().TileY == y)
			return true;
		return false;
	}

	public GameObject ChooseUnitAsSelectedOnTile(int x, int y)
	{
		if (Unit1.GetComponent<Unit>().TileX == x && Unit1.GetComponent<Unit>().TileY == y)
			SelectedUnit = Unit1;
		if (Unit2.GetComponent<Unit>().TileX == x && Unit2.GetComponent<Unit>().TileY == y)
			SelectedUnit = Unit2;
		if (Unit3.GetComponent<Unit>().TileX == x && Unit3.GetComponent<Unit>().TileY == y)
			SelectedUnit = Unit3;
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

  /*
    public GameObject GetUnitAt(int x , int y)
    {
     GameObject KlickedUnit = AllUnits.Find(u => x == u.GetComponent<Unit>().TileX && y == u.GetComponent<Unit>().TileY);
        Debug.Log("KlickedUnit: " + KlickedUnit);
        return KlickedUnit;


    }
	*/
}
