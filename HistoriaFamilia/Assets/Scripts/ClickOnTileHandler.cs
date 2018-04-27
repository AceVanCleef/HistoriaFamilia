using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnTileHandler : MonoBehaviour {

	public int TilePositionX;
	public int TilePositionY;
	public BoardManager Map;

	void OnMouseUp()
	{
		Debug.Log("clicking on" + TilePositionX + "-" + TilePositionY);
		Map.GeneratePathTo(TilePositionX, TilePositionY);
		Map.SelectedUnit.GetComponent<Unit>().MoveToTargettile();
	}
}
