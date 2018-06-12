using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Unit : MonoBehaviour {

	//Coordinates of Unit on GameBoard.
	//[HideInInspector]
	public int TileX;
	//[HideInInspector]
	public int TileY;
	//[HideInInspector]
	public BoardManager Map;
	//For StateMachine's Cancel transition.
	private int _previousPosX;
	private int _previousPosY;

	//pathfinding: stores the path from this unit ( = source) to target Node
	public List<Node> CurrentPath = null;

	// activates from-tile-to-tile Lerp - transition
	private bool IsWalking = false;
	//Coordinates of the tile which is the final destination of this unit's movement.
	private int finalWalkDestinationX;
	private int finalWalkDestinationY;
	// How many seconds should the movement transition last?
	[Tooltip("Duration of tile-to-tile movement in sec. / tile")]
	public float LerpDuration = 0.25f;
	// keeps track and updates the current lerp time.
	private float _currentLerpTime = 0f;

	// identifies what UnitType it is by mapping Unit to UnitType.
    [HideInInspector]
    public UnitType.UnitArcheType Unit_Type;

	// Health System
	public float CurrentHealth;

	[SerializeField] //made visible in inspector for debugging.
	private UnitState _unitState;
	//a really dumb implementation of a state machine.
	[Serializable]
	public class UnitState {

		/*Links to FSM:
			https://millionlords.com/finite-state-machine-unity/
			https://www.youtube.com/watch?v=D6hAftj3AgM
		*/

		//Variables to check against.
		public bool InReadyState			= true;
		public bool InUnitSelectedState		= false;
		public bool InUnitArrivedState		= false;
		public bool InSelectingTargetState	= false;
		public bool InAttackingState		= false;
		public bool InRestingState			= false;

		// ------------ transitions (forward, backward where allowed) ----------------
		//Ready state
		public void Ready2UnitSelected () {
			InReadyState = false;
			InUnitSelectedState = true;
		}
		public void UnitSelected2Ready () {
			InReadyState = true;
			InUnitSelectedState = false;
		}
		//UnitSelected state
		public void UnitSelected2UnitArrived () {
			InUnitSelectedState = false;
			InUnitArrivedState = true;
		}
		public void UnitArrived2UnitSelected () {
			InUnitSelectedState = true;
			InUnitArrivedState = false;
		}
		//UnitArrived state
		public void UnitArrived2SelectingTarget () {
			InUnitArrivedState = false;
			InSelectingTargetState = true;
		}
		public void SelectingTarget2UnitArrived () {
			InUnitArrivedState = true;
			InSelectingTargetState = false;
		}
		//SelectingTarget state
		public void SelectingTarget2Attacking () {
			InSelectingTargetState = false;
			InAttackingState = true;
		}
		//Attacking state
		public void Attacking2Resting () {
			InAttackingState = false;
			InRestingState = true;
		}
		//Resting state
		public void Resting2Ready () {
			InRestingState = false;
			InReadyState = true;
		}
		//Back to Ready state. The cycle is now closed.

		//transitions for Wait
		public void UnitArrived2Resting () {
			InUnitArrivedState = false;
			InRestingState = true;
		}
	}

	void Start()
	{
		// Set Position of selected unit.
        TileX = (int)transform.position.x;
        TileY = (int)transform.position.y;

		_unitState = new UnitState();
	}

	void Update()
	{
		//DrawDebugLine();
		
		ManageUnitMovement();
	}

	private void DrawDebugLine()
	{
		// Draw our debug line showing the pathfinding!
		// NOTE: This won't appear in the actual game view.
		if (CurrentPath != null)
		{
			int currNode = 0;

			while (currNode < CurrentPath.Count - 1)
			{
				Vector3 start = Map.TileCoordToWorldCoord( CurrentPath[currNode].x, CurrentPath[currNode].y );
				Vector3 end = Map.TileCoordToWorldCoord( CurrentPath[currNode + 1].x, CurrentPath[currNode + 1].y );
				/*
				Debug.Log("start" + start.x + " - " + start.y);
				Debug.Log("end" + end.x + " - " + end.y);
				Debug.Log("--------------------------------------");
				*/
				Debug.DrawLine( start, end, Color.red, 0, false);
				currNode++;
			}
		}
	}

	public UnitState GetUnitState() {
		return _unitState;
	}

	// -------------------------- Unit Movement ---------------------------------

	private void ManageUnitMovement()
	{
		// Lerp - transition from-tile-to-tile.
		if (IsWalking)
		{
			if( HasReachedNextTile() )
				MoveToNextTile();

			// controls the speed of each tile transition.
			_currentLerpTime += Time.deltaTime;
			if (_currentLerpTime == LerpDuration)
			{
				_currentLerpTime = LerpDuration;
			}
			// percentage = [0,1] where 0 means "at startPosition" and 1 means "reached lerp endPosition". Anything in between means "still moving".
			float percentage = _currentLerpTime / LerpDuration;
			// Smoothly animate towards the correct map tile.
			transform.position = Vector2.Lerp(transform.position, Map.TileCoordToWorldCoord( TileX, TileY ), percentage);	//25f * Time.deltaTime

			if ( HasReachedFinalDestination() ) {
				IsWalking = false;
				// Teleport us to our correct "current" position, in case we
				// haven't finished the animation yet.
				transform.position = Map.TileCoordToWorldCoord( TileX, TileY );
				// update state machine.
				_unitState.UnitSelected2UnitArrived();
			}
		}
	}

	public void MoveUnitToTileAt(int x, int y)
	{
		if (! _unitState.InUnitSelectedState) return;
		_previousPosX = TileX;
		_previousPosY = TileY;
		IsWalking = true; //initializes walking.
		finalWalkDestinationX = x;
		finalWalkDestinationY = y;
	}

	private void MoveToNextTile()
	{
		if (CurrentPath == null) return;
		
		//Remove the old current/first node from the path.
		CurrentPath.RemoveAt(0);
		//reset lerp timer.
		_currentLerpTime = 0f;

		// Teleport us to our correct "current" position, in case we
		// haven't finished the animation yet.
		transform.position = Map.TileCoordToWorldCoord( TileX, TileY );

		//Now grab the new first node and move us to that position (= endPos of Lerp).
		TileX = CurrentPath[0].x;
		TileY = CurrentPath[0].y;

		if (CurrentPath.Count == 1)
		{
			// We only have one tile left in the path, and that tile MUST be our ultimate
			// destination. So let's just clear our pathfinding info.
			CurrentPath = null;
		}
	}

	private bool HasReachedNextTile()
	{
		return Vector2.Distance(transform.position, Map.TileCoordToWorldCoord( TileX, TileY )) < 0.1f;
	}

	public bool HasReachedFinalDestination()
	{
		return Vector2.Distance(transform.position, Map.TileCoordToWorldCoord( finalWalkDestinationX, finalWalkDestinationY )) < 0.001f;
	}

	public void TeleportToPreviousPosition () {
		transform.position = Map.TileCoordToWorldCoord( _previousPosX, _previousPosY );
		TileX = _previousPosX;
		TileY = _previousPosY;
	}
}
