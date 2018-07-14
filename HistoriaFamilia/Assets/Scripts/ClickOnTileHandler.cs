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
	private Color _previousColor;

	void OnMouseDown()
	{
		Debug.Log("Klicked on Tile " + TilePositionX + ":" + TilePositionY + " called " + gameObject.name);
		if ( UnitManager.IsUnitSelected() && UnitManager.CanSelectedUnitReachTileAt(TilePositionX, TilePositionY) ){
			MoveSelectedUnit();
		}
	}

	private void MoveSelectedUnit()
	{
		Debug.Log("Move selected unit now!");
		Map.GeneratePathTo(TilePositionX, TilePositionY);
        Unit su = UnitManager.GetSelectedUnit().GetComponent<Unit>();
		su.MoveUnitToTileAt(TilePositionX, TilePositionY);
	}


	void OnMouseEnter() {
		_previousColor = SHM.GetCurrentColor();
		SHM.SetToHoverColor();
		Map.MousePointer.DrawCursor = true;
		Map.MousePointer.SetCursorPosition(TilePositionX, TilePositionY);
	}
	
	void OnMouseExit() {
		SHM.SetColorTo(_previousColor);
	}

}
