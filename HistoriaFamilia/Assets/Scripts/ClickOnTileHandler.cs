using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnTileHandler : MonoBehaviour {

	// Position of clicked Tile.
	public int TilePositionX;
	public int TilePositionY;
	public BoardManager Map;
    public UnitManager UnitManager;
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
    void OnMouseDown()
    {
        Debug.Log(Map.SelectedUnit != null);
        if (Map.SelectedUnit != null)
        {
            //Move Selected Unit

            //Debug.Log("onmouseenter? yes");
            Map.GeneratePathTo(TilePositionX, TilePositionY);
            //Debug.Log("generated path? yes");
            Map.SelectedUnit.GetComponent<Unit>().InitializeWalkingTo(TilePositionX, TilePositionY);
            //Debug.Log("moved to targettile and finished? yes");
        }
        else
        {

          Map.SelectedUnit = UnitManager.GetUnitAt(TilePositionX, TilePositionY);
            Debug.Log("I just Tried to Klick on a Unit");
            Map.UpdateSelectedUnitValues();
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
}
