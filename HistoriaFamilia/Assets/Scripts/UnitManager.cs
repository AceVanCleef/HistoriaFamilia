using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    public List <GameObject> AllUnit = null;
    public int[,] PossitionUnit = null;
    public BoardManager BoardManager;

	//note: each unit prefab can have its click handler which will inform the map to mark it as selected.
	public GameObject SelectedUnit;

     void Start()
    {
       // PossitionUnit = new int[BoardManager.BoardSizeX,BoardManager.BoardSizeY];

	   UpdateSelectedUnitValues();
    }

	public void UpdateSelectedUnitValues()
    {
        // Set Position of selected unit.
        Unit unit = SelectedUnit.GetComponent<Unit>();
        unit.TileX = (int)SelectedUnit.transform.position.x;
        unit.TileY = (int)SelectedUnit.transform.position.y;
        unit.Map = BoardManager;
    }
  
    public GameObject GetUnitAt(int x , int y)
    {
     GameObject KlickedUnit = AllUnit.Find(u => x == u.GetComponent<Unit>().TileX && y == u.GetComponent<Unit>().TileY);
        Debug.Log("KlickedUnit: " + KlickedUnit);
        return KlickedUnit;


    }

}
