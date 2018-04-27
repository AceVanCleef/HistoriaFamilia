using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnTileHandler : MonoBehaviour {

	public int TilePositionX;
	public int TilePositionY;
	public BoardManager Map;

	void OnMouseUp()
	{
		Debug.Log("onmouseenter? yes");
		Map.GeneratePathTo(TilePositionX, TilePositionY);
		Debug.Log("generated path? yes");
		Map.SelectedUnit.GetComponent<Unit>().MoveToTargettile();
		Debug.Log("moved to targettile and finished? yes");
	}
}
