using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnUnitHandler : MonoBehaviour {

	// direct access to position of clicked Unit.
	private Unit _unit;

	//Inizialized in UnitManager.GenerateMapVisuals().
	//[HideInInspector]
	//public BoardManager Map; //remove if not needed.
	[HideInInspector]
    public UnitManager UnitManager;

	void Start()
	{
		_unit = gameObject.GetComponent<Unit>();
	}

	void OnMouseDown()
	{
		Debug.Log("Clicked on a unit." + "Has a Unit? " + UnitManager.HasUnitOnTile(_unit.TileX, _unit.TileY));
		
		if (!UnitManager.IsUnitSelected()) {
			SelectUnit();
		}
		//Debug.Log("Selected unit == null?" + !UnitManager.IsUnitSelected());
	}

	private void SelectUnit()
	{
		if (UnitManager.HasUnitOnTile(_unit.TileX, _unit.TileY))
			{
				GameObject selectedUnit = UnitManager.ChooseUnitAsSelectedOnTile(_unit.TileX, _unit.TileY);
				Debug.Log("Selected unit is on (" + selectedUnit.GetComponent<Unit>().TileX + ":" + selectedUnit.GetComponent<Unit>().TileY + ")");
			}
	}
}
