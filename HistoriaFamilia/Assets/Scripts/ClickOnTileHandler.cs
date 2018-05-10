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
		Debug.Log("Klicked on Tile " + TilePositionX + ":" + TilePositionY + " called " + gameObject.name);
		if (UnitManager.IsUnitSelected()){
			MoveSelectedUnit();
		}
	}

	private void MoveSelectedUnit()
	{
		Debug.Log("Move selected unit now!");
		Map.GeneratePathTo(TilePositionX, TilePositionY);
        UnitManager.GetSelectedUnit().GetComponent<Unit>().MoveUnitToTileAt(TilePositionX, TilePositionY);
		UnitManager.DeselectUnit();
	}

}
