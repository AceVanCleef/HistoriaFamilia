using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    public List <GameObject> AllUnit = null;
    public int[,] PositionUnit = null;
    public BoardManager BoardManager;

	//for testing:
	public GameObject Unit1;
	public GameObject Unit2;
	public GameObject Unit3;

	//note: each unit prefab can have its click handler which will inform the map to mark it as selected.
	public GameObject SelectedUnit;

     void Start()
    {
       // PositionUnit = new int[BoardManager.BoardSizeX,BoardManager.BoardSizeY];

	   //UpdateSelectedUnitValues();

	   //will be replaced with initialize/createUnits()
	   Unit1.GetComponent<Unit>().Map = BoardManager;
	   Unit2.GetComponent<Unit>().Map = BoardManager;
	   Unit3.GetComponent<Unit>().Map = BoardManager;
    }

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


  
    public GameObject GetUnitAt(int x , int y)
    {
     GameObject KlickedUnit = AllUnit.Find(u => x == u.GetComponent<Unit>().TileX && y == u.GetComponent<Unit>().TileY);
        Debug.Log("KlickedUnit: " + KlickedUnit);
        return KlickedUnit;


    }

}
