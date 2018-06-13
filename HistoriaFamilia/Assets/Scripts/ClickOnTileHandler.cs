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
	public SpriteHighlightManager SHM;

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
        Unit su = UnitManager.GetSelectedUnit().GetComponent<Unit>();
		su.MoveUnitToTileAt(TilePositionX, TilePositionY);
		//UnitManager.DeselectUnit(); //moving to Unit script? Or not needed here at all? Maybe as reaction to UnitUI.
	}


	void OnMouseEnter() {
		SHM.SetToHoverColor();
	}
	
	void OnMouseExit() {
		SHM.ResetColor();
	}

}
