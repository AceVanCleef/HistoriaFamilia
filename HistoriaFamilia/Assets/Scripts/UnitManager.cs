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

	// asserts correct mapping from UnitType.UnitArcheType to UnitType[]'s index 
	// and prevents duplicates of the same UnitType.UnitArcheType value.
	// Note: If a designer wants to add two types of e.g. Archers such as Longbow- and CompositeBowArchers,  
	// extend the UnitType.UnitArcheType enum.
	private void PreventEnumToIndexMappingErrorOFUnitTypes()
	{
		Debug.Log("Entering Prevent Index error of Units");
		UnitTypes = UnitTypes.OrderBy(ut => ut.Unit_Type)
					.DistinctBy(ut => ut.Unit_Type)
					.ToArray();
		foreach(UnitType unit in UnitTypes)
		{
			Debug.Log(unit.Unit_Type + "-" + unit.MovementReach);
		}
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
		//???: rename unitType to unitTypeCode?

		//instantiate at x, y as UnitArcheType unitType such as Archer, Infantry or others:
		Debug.Log(x + "-" + y + " is " + unitType);
		UnitType ut = UnitTypes[ (int) unitType ];
		GameObject go = Instantiate(ut.UnitVisualPrefab, new Vector3(x, y, 0), 
			Quaternion.identity) as GameObject;
		go.transform.SetParent(_unitsHolder);

		// Initialize Unit script.
		Unit unitScript = go.GetComponent<Unit>();
		unitScript.TileX = (int)go.transform.position.x;
        unitScript.TileY = (int)go.transform.position.y;
		unitScript.OwningPlayerID = p.PlayerID;
		unitScript.Unit_Type = unitType;
        unitScript.Map = BoardManager;
		unitScript.CurrentHealth = UnitTypes[(int) unitType].MaxHealth;
		//Initialize ClickOnUnitHandler.
		go.GetComponent<ClickOnUnitHandler>().UnitManager = this;

		p.AllUnits.Add(go);
	}

	// -------------------------- Targeting a hostile Unit ---------------------------------
	private void PrepareAttackRangeVisual() {
		Unit su = SelectedUnit.GetComponent<Unit>();
		int attackRange = (int) UnitTypes[(int) su.Unit_Type].AttackRange;
		_tilesInAttackRange = BoardManager.GetTilesInAttackRange(su.TileX, su.TileY, attackRange);
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
		int su = (int) selectedUnit.Unit_Type;

		selectedUnit.GetUnitState().SelectingTarget2Attacking();


		targetedUnit.CurrentHealth -= (int) CalculateDamage(UnitTypes[su].BaseAttackPower);
		targetedUnit.SetHPText(targetedUnit.CurrentHealth);
		if (targetedUnit.CurrentHealth < 0.01f)
		{
			DestroyUnit(TargetedUnit);
			DeselectTarget(); //just for safety reasons. might be removed in the future.
		}
		selectedUnit.GetUnitState().Attacking2Resting();
		ResetHightlightingOfTiles(_allSHMInAttackRange);
	}

	private float CalculateDamage(float baseAttackPower)
	{
		return baseAttackPower;
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
		int movePoints = (int) UnitTypes[(int) su.Unit_Type].MovementReach;
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
        int index = (int) SelectedUnit.GetComponent<Unit>().Unit_Type;
        Debug.Log("unittype key: " + index + " is " + UnitTypes[index].Unit_Type);
        return !UnitTypes[index].ProhibitedToWalkOn.Contains(tile.Topography);
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
	Debug.Log("ShowUnitUI");
		_displayUnitUI = true;
		UnitUI.SetActive(_displayUnitUI);
	}

	private void HideUnitUI() {
		Debug.Log("HideUnitUI");
		_displayUnitUI = false;
		UnitUI.SetActive(_displayUnitUI);
	}


	// -------------------------- Unit UI Control Callbacks for State Machine ---------------------------------

	private void AquireTarget() {
		Unit su = SelectedUnit.GetComponent<Unit>();
		if ( !su.GetUnitState().InUnitArrivedState) return;
		su.GetUnitState().UnitArrived2SelectingTarget();
		PrepareAttackRangeVisual();
		//Todo: - display TargetSelector cursor in UI. - do logic stuff regarding selecting a target.
	}

	private void Cancel() {
		Debug.Log("Cancel");
		Unit su = SelectedUnit.GetComponent<Unit>();
		if (su.GetUnitState().InUnitSelectedState) {
			su.GetUnitState().UnitSelected2Ready();
			HideUnitUI();
			DeselectTarget();
			DeselectUnit();

			ResetHightlightingOfTiles(_allSHMInMovementRange);
		}
		if (su.GetUnitState().InUnitArrivedState) {
			//move backwards.
			su.TeleportToPreviousPosition();
			su.GetUnitState().UnitArrived2UnitSelected();
		}
		if (su.GetUnitState().InSelectingTargetState){
			su.GetUnitState().SelectingTarget2UnitArrived();
			ResetHightlightingOfTiles(_allSHMInAttackRange);
			HighlightTilesInMovementRange();
		}
	}

	private void Wait() {
		Debug.Log("Wait");
		Unit su = SelectedUnit.GetComponent<Unit>();
		if ( !su.GetUnitState().InUnitArrivedState) return;
		su.GetUnitState().UnitArrived2Resting();
		HideUnitUI();
		ResetHightlightingOfTiles(_allSHMInMovementRange);
		ResetHightlightingOfTiles(_allSHMInAttackRange);
		DeselectTarget();
		DeselectUnit();
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