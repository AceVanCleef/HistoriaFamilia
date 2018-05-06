using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnTileHandler : MonoBehaviour {

	// Position of clicked Tile.
	public int TilePositionX;
	public int TilePositionY;

	//Inizialized in BoardManager.GenerateMapVisuals().
	[HideInInspector]
	public BoardManager Map;
	[HideInInspector]
    public UnitManager UnitManager;

	void OnMouseDown()
	{
		Debug.Log("Klicked on Tile " + TilePositionX + ":" + TilePositionY + ". Has a Unit? "+ UnitManager.HasUnitOnTile(TilePositionX, TilePositionY));
	}

    /*
	void OnMouseUp()
	{
		//Debug.Log("onmouseenter? yes");
		Map.GeneratePathTo(TilePositionX, TilePositionY);
		//Debug.Log("generated path? yes");
		Map.SelectedUnit.GetComponent<Unit>().InitializeWalkingTo(TilePositionX, TilePositionY);
		//Debug.Log("moved to targettile and finished? yes");
	}
    */
	/*
    void OnMouseDown()
    {
        Debug.Log(UnitManager.SelectedUnit != null);
        if (UnitManager.SelectedUnit != null)
        {
            //Move Selected Unit

            //Debug.Log("onmouseenter? yes");
            Map.GeneratePathTo(TilePositionX, TilePositionY);
            //Debug.Log("generated path? yes");
            UnitManager.SelectedUnit.GetComponent<Unit>().InitializeWalkingTo(TilePositionX, TilePositionY);
            //Debug.Log("moved to targettile and finished? yes");
        }
        else
        {

          UnitManager.SelectedUnit = UnitManager.GetUnitAt(TilePositionX, TilePositionY);
            Debug.Log("I just Tried to Klick on a Unit");
            UnitManager.UpdateSelectedUnitValues();
        }

     }

    bool UnitIsNotNull(Unit u)
    {
        if(u != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
	*/
}
