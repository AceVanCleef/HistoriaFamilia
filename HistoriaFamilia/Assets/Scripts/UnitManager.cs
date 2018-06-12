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
	[SerializeField][Tooltip("Stores which units are currently alive and fighting on the battlefield. Note: Do not use this field. Instead define units using Spawn Data field.")]
    private List <GameObject> AllUnits = null;

	[HideInInspector]
    public BoardManager BoardManager;

	[Tooltip("Define what unit types do exist and which attribute values they have.")]
	public UnitType[] UnitTypes;	//defined in inspector.


	[Tooltip("Define where what kind of unit will spawn at the beginning of a match.")]
	public List <UnitSpawnData> SpawnData;


	//note: each unit prefab can have its click handler which will inform the map to mark it as selected.
	[SerializeField][Tooltip("Hint: Leave this empty.")]
	private GameObject SelectedUnit;

	private GameObject TargetedUnit;
	//private bool _enemyCanBeSelected;

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
		_unitsHolder = new GameObject("Units").transform;
		foreach (UnitSpawnData usd in SpawnData)
		{
			CreateUnitAt(usd.SpawnPosX, usd.SpawnPosY, usd.Unit_Type);
		}
	}

	//eventually public if fabrics will call CreateUnitAt().
	private void CreateUnitAt(int x, int y, UnitType.UnitArcheType unitType)
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
		unitScript.Unit_Type = unitType;
        unitScript.Map = BoardManager;
		unitScript.CurrentHealth = UnitTypes[(int) unitType].MaxHealth;
		//Initialize ClickOnUnitHandler.
		go.GetComponent<ClickOnUnitHandler>().UnitManager = this;

		//add unit to AllUnits
		AllUnits.Add(go);
	}

	// -------------------------- Targeting a hostile Unit ---------------------------------
	public bool HasEnemyOnTile(int x, int y)
	{
		//TODO: differentiate between hostile and friendly units.
		return AllUnits.Any(unit => unit.GetComponent<Unit>().TileX == x && unit.GetComponent<Unit>().TileY == y);
	}

	public GameObject TargetUnitAt(int x, int y)
	{
		TargetedUnit = AllUnits.Where(unit => unit.GetComponent<Unit>().TileX == x && unit.GetComponent<Unit>().TileY == y)
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
	public bool ReadyToLockOnTarget() {
		return false ;// _enemyCanBeSelected;
	}

	public Unit[] GetTargetsInRangetsFrom(int x, int y)
	{
		//TODO: ensure that selected units first walk, then attack.
		//XXX: Maybe a UI should be implemented first.
		return null;
	}

	public void AttackTargetedUnit()
	{
		Unit selectedUnit = SelectedUnit.GetComponent<Unit>();
		Unit targetedUnit = TargetedUnit.GetComponent<Unit>();
		int su = (int) selectedUnit.Unit_Type;

		selectedUnit.GetUnitState().SelectingTarget2Attacking();


		targetedUnit.CurrentHealth -= CalculateDamage(UnitTypes[su].BaseAttackPower);
		if (targetedUnit.CurrentHealth < 0.01f)
		{
			DestroyUnit(TargetedUnit);
			DeselectTarget(); //just for safety reasons. might be removed in the future.
		}
		selectedUnit.GetUnitState().Attacking2Resting();
	}

	private float CalculateDamage(float baseAttackPower)
	{
		return baseAttackPower;
	}

	private void DestroyUnit(GameObject unit)
	{
		AllUnits.Remove(unit);
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
		//TODO: differentiate between your units and other players units.
		return AllUnits.Any(unit => unit.GetComponent<Unit>().TileX == x && unit.GetComponent<Unit>().TileY == y);
	}

	public GameObject ChooseUnitAsSelectedOnTile(int x, int y)
	{
		SelectedUnit = AllUnits.Where(unit => unit.GetComponent<Unit>().TileX == x && unit.GetComponent<Unit>().TileY == y)
			.First();

		// only get selected when in Ready state.
		if ( !SelectedUnit.GetComponent<Unit>().GetUnitState().InReadyState ) {
			SelectedUnit = null;
			return null;
		}
		SelectedUnit.GetComponent<Unit>().GetUnitState().Ready2UnitSelected();
		ShowUnitUI();

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

	private void Cancel() {
		Debug.Log("Cancel");
	}

	private void Wait() {
		Debug.Log("Wait");
		DeselectTarget();
		DeselectUnit();
	}

	// -------------------------- Pseudo State Machine ---------------------------------

	private void AllowTargetingEnemy() {
		//_enemyCanBeSelected = true;
		//do additional UI stuff, e.g. display targeting area.
	}

	private void AquireTarget() {
		Unit su = SelectedUnit.GetComponent<Unit>();
		if ( !su.GetUnitState().InUnitArrivedState) return;
		su.GetUnitState().UnitArrived2SelectingTarget();
		//Todo: - display TargetSelector cursor in UI. - do logic stuff regarding selecting a target.

		//do additional UI stuff, e.g. display targeting area.
	}

	private void DisableTargetingenemy() {
		//_enemyCanBeSelected = false;
	}

}
