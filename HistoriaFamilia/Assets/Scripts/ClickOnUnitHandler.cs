﻿using System.Collections;
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
		// Note: Work in progress. [Stefan]
		if (UnitManager.IsUnitSelected() && !UnitManager.IsTargetSelected()) {
			Unit su = UnitManager.GetSelectedUnit().GetComponent<Unit>();

			//Target enemy if SelectedUnit is in Arrived state. Note: Can't attack itself
			if (su.GetUnitState().InSelectingTargetState && (su.TileX != _unit.TileX || su.TileY != _unit.TileY) ) {
				TargetEnemy();
			}
			if (su.GetUnitState().InUnitSelectedState && (su.TileX == _unit.TileX && su.TileY == _unit.TileY)) {
				//Unit staying on the spot:.
				su.GetUnitState().UnitSelected2UnitArrived();
				return;
			}
			
		}

		if (!UnitManager.IsUnitSelected()) {
			SelectUnit();
		}
	}

	private void SelectUnit()
	{
		if (UnitManager.HasUnitOnTile(_unit.TileX, _unit.TileY))
			{
				GameObject selectedUnit = UnitManager.ChooseUnitAsSelectedOnTile(_unit.TileX, _unit.TileY);
				UnitManager.BoardManager.MousePointer.SetCursorColorTo(Color.red);
			}
	}

	private void TargetEnemy()
	{
		if (UnitManager.HasEnemyOnTile(_unit.TileX, _unit.TileY) && UnitManager.IsTargetInRangeOfSelectedUnitAt(_unit.TileX, _unit.TileY) )
			{
				//todo: check IsUnitINRange?
				GameObject selectedTarget = UnitManager.TargetUnitAt(_unit.TileX, _unit.TileY);
				AttackEnemy();
			}
	}

	//todo: move to unitmanager UnitUI
	private void AttackEnemy()
	{
		UnitManager.AttackTargetedUnit();
	}


	void OnMouseEnter() {
		UnitManager.BoardManager.MousePointer.DrawCursor = true;
		UnitManager.BoardManager.MousePointer.SetCursorPosition(_unit.TileX, _unit.TileY);
	}
}
