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
		
		return false;
	}
  
    public GameObject GetUnitAt(int x , int y)
    {
     GameObject KlickedUnit = AllUnit.Find(u => x == u.GetComponent<Unit>().TileX && y == u.GetComponent<Unit>().TileY);
        Debug.Log("KlickedUnit: " + KlickedUnit);
        return KlickedUnit;


    }

}
