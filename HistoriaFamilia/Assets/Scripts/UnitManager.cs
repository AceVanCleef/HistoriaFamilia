using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UnityEngine.UI;

public class UnitManager : MonoBehaviour {

	//UnitUI (Attack, Wait, Cancel)
	public GameObject UnitUI;
	private bool _displayUnitUI = false;
	public Button AttackBtn;
	public Button WaitBtn;
	public Button CancelBtn;

	// utility variable - holds the GameBoard's children under one root node in unity's "Hierarchy" window.
	private Transform _unitsHolder;

	private PlayerManager _pm;
	[HideInInspector]
    public BoardManager BoardManager;

	[Tooltip("Define what unit types do exist and which attribute values they have.")]
	public UnitType[] UnitTypes;	//defined in inspector.


	//note: each unit prefab can have its click handler which will inform the map to mark it as selected.
	[SerializeField][Tooltip("Hint: Leave this empty.")]
	private GameObject SelectedUnit;

	private GameObject TargetedUnit;

	//Unit movement and attacking range
	List<Node> _validMoves;
	List<Node> _tilesInAttackRange;
	List<SpriteHighlightManager> _allSHMInMovementRange;
	List<SpriteHighlightManager> _allSHMInAttackRange;

    void Start()
    {
		PreventEnumToIndexMappingErrorOFUnitTypes();
		InitializeUnitsOnMapCreation();

		AttackBtn.onClick.AddListener(AquireTarget);
		WaitBtn.onClick.AddListener(Wait);
		CancelBtn.onClick.AddListener(Cancel);

    }

	// -------------------------- UnitType mapping ---------------------------------

	// asserts correct mapping from UnitType.UnitArcheType to UnitType[]'s index 
	// and prevents duplicates of the same UnitType.UnitArcheType value.
	// Note: If a designer wants to add two types of e.g. Archers such as Longbow- and CompositeBowArchers,  
	// extend the UnitType.UnitArcheType enum.
	private void PreventEnumToIndexMappingErrorOFUnitTypes()
	{
		UnitTypes = UnitTypes.OrderBy(ut => ut.Unit_Type)
					.DistinctBy(ut => ut.Unit_Type)
					.ToArray();
	}

	private UnitType GetUnitTypeOf(Unit u) {
		return UnitTypes[(int) u.Unit_Type];
	}

	private UnitType GetUnitTypeBy(UnitType.UnitArcheType uat) {
		return UnitTypes[(int) uat];
	}

	// -------------------------- Unit Creation ---------------------------------
	private void InitializeUnitsOnMapCreation()
	{
		_pm = GetComponent<PlayerManager>();
		_unitsHolder = new GameObject("Units").transform;
		foreach (Player p in _pm.AllPlayers)
		{
			foreach (UnitSpawnData usd in p.SpawnData)
			{
				CreateUnitAt(usd.SpawnPosX, usd.SpawnPosY, usd.Unit_Type, p);
			}
		}
		
	}

	//eventually public if fabrics will call CreateUnitAt().
	private void CreateUnitAt(int x, int y, UnitType.UnitArcheType unitType, Player p)
	{
		//instantiate at x, y as UnitArcheType unitType such as Archer, Infantry or others:
		UnitType ut = GetUnitTypeBy( unitType );
		GameObject go = Instantiate(ut.UnitVisualPrefab, new Vector3(x, y, 0), 
			Quaternion.identity) as GameObject;
		go.transform.SetParent(_unitsHolder);

		// Initialize Unit script.
		Unit unitScript = go.GetComponent<Unit>();
		unitScript.TileX = (int)go.transform.position.x;
        unitScript.TileY = (int)go.transform.position.y;
		unitScript.OwningPlayerID = p.PlayerID;
		unitScript.Unit_Type = unitType;
		unitScript.IsRangedUnit = ut.MaxAttackRange > 1;
        unitScript.Map = BoardManager;
		unitScript.CurrentHealth = ut.MaxHealth;
		//Initialize ClickOnUnitHandler.
		go.GetComponent<ClickOnUnitHandler>().UnitManager = this;
		//colouring the Units
		go.GetComponentInChildren<SpriteRenderer>().material.color = p.UnitColouring;
		// How to change sprite colouring: https://www.youtube.com/watch?v=J66UkLJHzCY
		// GetComponent functions overview: https://docs.unity3d.com/ScriptReference/Component.html

		p.AllUnits.Add(go);
	}

	// -------------------------- Targeting a hostile Unit ---------------------------------
	private void PrepareAttackRangeVisual() {
		Unit su = SelectedUnit.GetComponent<Unit>();
		int maxAttackRange = GetUnitTypeOf(su).MaxAttackRange;
		int minAttackRange = GetUnitTypeOf(su).MinAttackRange;
		_tilesInAttackRange = BoardManager.GetTilesInAttackRange(su.TileX, su.TileY, maxAttackRange, minAttackRange);
		ResetHightlightingOfTiles(_allSHMInMovementRange);
		_allSHMInAttackRange = BoardManager.GetSpriteHighlightManagersInRangeBy(_tilesInAttackRange);
		HighlightTilesInAttackRange();
	}
	
	public bool HasEnemyOnTile(int x, int y)
	{
		return _pm.GetAllHostileUnits().Any(unit => unit.GetComponent<Unit>().TileX == x && unit.GetComponent<Unit>().TileY == y);
	}
	
	public bool IsTargetInRangeOfSelectedUnitAt(int x, int y) {
		return _tilesInAttackRange.Any(node => node.x == x && node.y == y);
	}

	public GameObject TargetUnitAt(int x, int y)
	{
		TargetedUnit = _pm.GetAllHostileUnits().Where(unit => unit.GetComponent<Unit>().TileX == x && unit.GetComponent<Unit>().TileY == y)
			.First();
		return TargetedUnit;
	}

	public bool IsTargetSelected()
	{
		return TargetedUnit != null;
	}

	public void DeselectTarget()
	{
		TargetedUnit = null;
	}

	// -------------------------- Unit Combat ---------------------------------

	public void AttackTargetedUnit()
	{
		Unit selectedUnit = SelectedUnit.GetComponent<Unit>();
		Unit targetedUnit = TargetedUnit.GetComponent<Unit>();

		selectedUnit.GetUnitState().SelectingTarget2Attacking();

		Attack(targetedUnit, selectedUnit);
		if (targetedUnit.CurrentHealth < 0.01f)
		{
			DestroyUnit(TargetedUnit);
		} 
		else 
		{
			//counter attack
			if (IsInFiringRange(selectedUnit, targetedUnit) ) {
				Attack(selectedUnit, targetedUnit);
				if (selectedUnit.CurrentHealth < 0.01f) 
				{
					DestroyUnit(SelectedUnit);
				}
			}
		}
		//clean up
		selectedUnit.GetUnitState().Attacking2Resting();
		ResetHightlightingOfTiles(_allSHMInAttackRange);
		DeselectTarget();
		DeselectUnit();
		if (_pm.HasCurrentPlayerUsedAllHisUnits() ) _pm.NextPlayer();
	}

	private bool IsInFiringRange(Unit target, Unit attacker) {
		//Strategy 1: using BoardManager.GetTilesInAttackRange().Contains(target.x, target.y) 
		//			  --> BFS, Contains has a complexity of O(n)
		//Strategy 2: calculation: MinRange <= |Diff_x| + |Diff_Y| <= MaxRange --> O(1)
		//				Note: might only work for an attackable area shapend like a diamond.

		//Strategy 2
		int diffX = System.Math.Abs(attacker.TileX - target.TileX);
		int diffY = System.Math.Abs(attacker.TileY - target.TileY);
		UnitType ut = GetUnitTypeOf(attacker);
		Debug.Log(ut.MinAttackRange + " < " + diffX + " + " + diffY + " <= " + ut.MaxAttackRange);
		int sum = diffX + diffY;
		return ut.MinAttackRange < sum && sum <= ut.MaxAttackRange;
	}

	private void Attack(Unit target, Unit attacker) {
		UnitType ut = GetUnitTypeOf(attacker);
		
		//Todo: implement commander attack and defense bonuses. Potential range: [80, 130]
		target.CurrentHealth -= CalculateDamage(ut.BaseAttackPower, 100f, attacker.CurrentHealth,
										100f, BoardManager.GetTileTypeAt(target.TileX, target.TileY).TerrainDefenseValue ,
										target.CurrentHealth);
		target.SetHPText( (int) (target.CurrentHealth + 0.5) );
	}

	private float CalculateDamage(float baseAttPwr, float commanderAttackMultiplier, float attackerHP, 
		float defendingCommanderDefenseValue, float defendingTerrainStars, float defenderHP)
	{
		//Formula: http://awbw.wikia.com/wiki/Damage_Formula

		float randomNumber = Random.Range(0f , 1f);
		float damage = ( (baseAttPwr * commanderAttackMultiplier) / 100 + randomNumber) *
			(attackerHP / 10) * 
			( (200 - (defendingCommanderDefenseValue + defendingTerrainStars * defenderHP)) / 100);
		return damage;
	}

	private void DestroyUnit(GameObject unit)
	{
		_pm.OwningPlayerLooses(unit);
		Destroy(unit);
	}

	// -------------------------- Unit Selection ---------------------------------
	private void UpdateSelectedUnitValues()
    {
        // Set Position of selected unit.
        Unit unit = SelectedUnit.GetComponent<Unit>();
        unit.TileX = (int)SelectedUnit.transform.position.x;
        unit.TileY = (int)SelectedUnit.transform.position.y;
    }

	public bool HasUnitOnTile(int x, int y)
	{
		return _pm.GetCurrentPlayer().AllUnits.Any(unit => unit.GetComponent<Unit>().TileX == x && unit.GetComponent<Unit>().TileY == y);
	}

	public GameObject ChooseUnitAsSelectedOnTile(int x, int y)
	{
		SelectedUnit = _pm.GetCurrentPlayer().AllUnits.Where(unit => unit.GetComponent<Unit>().TileX == x && unit.GetComponent<Unit>().TileY == y)
			.First();

		// only get selected when in Ready state.
		if ( !SelectedUnit.GetComponent<Unit>().GetUnitState().InReadyState ) {
			SelectedUnit = null;
			return null;
		}
		Unit su = SelectedUnit.GetComponent<Unit>();
		su.GetUnitState().Ready2UnitSelected();
		ShowUnitUI();
		//show movement range area on map
		int movePoints = (int) GetUnitTypeOf(su).MovementReach;
		_validMoves = BoardManager.GetValidMoves(su.TileX, su.TileY, movePoints);
		_allSHMInMovementRange = BoardManager.GetSpriteHighlightManagersInRangeBy(_validMoves);
		HighlightTilesInMovementRange();

		return SelectedUnit;
	}

	public bool IsUnitSelected()
	{
		return SelectedUnit != null;
	}

	public void DeselectUnit()
	{
		SelectedUnit = null;
		HideUnitUI();
		BoardManager.MousePointer.SetCursorColorTo(Color.white);
	}

	public GameObject GetSelectedUnit()
	{
		return SelectedUnit;
	}

	// -------------------------- SelectedUnit movement ---------------------------------

	public bool CanSelectedUnitReachTileAt(int x, int y) {
		return _validMoves.Any(node => node.x == x && node.y == y);
	}

	public bool IsAllowedToWalk(TileType tile)
    {
        // UnitTypes[indextoselectunittypeattributes]
        //var CantWalkOn = SelectedUnit.GetComponent<Unit>().GetComponent<UnitType>().ProhibitedToWalkOn;
        return !GetUnitTypeOf(SelectedUnit.GetComponent<Unit>()).ProhibitedToWalkOn.Contains(tile.Topography);
    }

	public bool HasSelectedUnitReachedFinalDestination() {
		return SelectedUnit.GetComponent<Unit>().HasReachedFinalDestination();
	}

	// -------------------------- Unit UI Control ---------------------------------
	private void ToggleUnitUI() {
		_displayUnitUI = !_displayUnitUI;
		UnitUI.SetActive(_displayUnitUI);
	}

	private void ShowUnitUI() {
		_displayUnitUI = true;
		UnitUI.SetActive(_displayUnitUI);
	}

	private void HideUnitUI() {
		_displayUnitUI = false;
		UnitUI.SetActive(_displayUnitUI);
	}


	// -------------------------- Unit UI Control Callbacks for State Machine ---------------------------------

	private void AquireTarget() {
		Unit su = SelectedUnit.GetComponent<Unit>();
		//Todo
		//if ( !AnyTargetInRange()) return;
		if (su.GetUnitState().InUnitSelectedState) {
			//Attacking from the spot
			su.StoreCurrentLocationAsPreviousPosition();
			su.GetUnitState().UnitSelected2SelectingTarget();
		} else if (su.GetUnitState().InUnitArrivedState) {
			//Attacking after unit has been moved to another location.
			su.GetUnitState().UnitArrived2SelectingTarget();
		} else {
			//invalid state.
			return;
		}
		PrepareAttackRangeVisual();
		//Todo: - display TargetSelector cursor in UI. - do logic stuff regarding selecting a target.
	}

	private void Cancel() {
		Unit su = SelectedUnit.GetComponent<Unit>();
		//from most advance state...
		if (su.GetUnitState().InSelectingTargetState){
			su.GetUnitState().SelectingTarget2UnitArrived();
			//usability improvement: Transition from InSelectingTargetState to InUnitSelectedState in case
			// the player chose to attack whitouth moving the target from its original position.
			if (su.WasUnitUsedWithoutMovingToAnotherTile())
				su.GetUnitState().UnitArrived2UnitSelected();
			ResetHightlightingOfTiles(_allSHMInAttackRange);
			HighlightTilesInMovementRange();
		} else if (su.GetUnitState().InUnitArrivedState) {
			//move backwards.
			su.TeleportToPreviousPosition();
			su.GetUnitState().UnitArrived2UnitSelected();
		} else if (su.GetUnitState().InUnitSelectedState) {			//...to first state.
			su.GetUnitState().UnitSelected2Ready();
			HideUnitUI();
			DeselectTarget();
			DeselectUnit();

			ResetHightlightingOfTiles(_allSHMInMovementRange);
		}
	}

	private void Wait() {
		Unit su = SelectedUnit.GetComponent<Unit>();
		//include valid states for this transition.
		if (su.GetUnitState().InUnitSelectedState) {
			su.GetUnitState().UnitSelected2Resting();
		} else if (su.GetUnitState().InUnitArrivedState) {
			su.GetUnitState().UnitArrived2Resting();
		} else {
			//invalid state.
			return;
		}
		HideUnitUI();
		ResetHightlightingOfTiles(_allSHMInMovementRange);
		ResetHightlightingOfTiles(_allSHMInAttackRange);
		DeselectTarget();
		DeselectUnit();
		if (_pm.HasCurrentPlayerUsedAllHisUnits() ) _pm.NextPlayer();
	}

	// ---------------- Tiles Highlighting for Movement and Attacking range of SelectedUnit -------------------

	private void HighlightTilesInMovementRange() {
		foreach (SpriteHighlightManager shm in _allSHMInMovementRange) {
			shm.SetToMovementColor();
		}
	}

	private void HighlightTilesInAttackRange() {
		foreach (SpriteHighlightManager shm in _allSHMInAttackRange) {
			shm.SetToAttackColor();
		}
	}

	private void ResetHightlightingOfTiles(List<SpriteHighlightManager> _allSHMInRange) {
		if (_allSHMInRange == null) return;
		foreach (SpriteHighlightManager shm in _allSHMInRange) {
			shm.ResetColor();
		}
	}
}