using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviour {

	public List<Player> AllPlayers = new List<Player>(2);

	private readonly int FIRST_PLAYER_ID = 0;
	private Player _currentPlayer;
	private int _currentPlayerID;

	void Start() {
		_currentPlayerID = FIRST_PLAYER_ID;
		_currentPlayer = AllPlayers[0];
	}

	public Player GetCurrentPlayer() {
		return _currentPlayer;
	}

	public bool HasCurrentPlayerUsedAllHisUnits() {
		return _currentPlayer.AllUnits.All(unit => unit.GetComponent<Unit>().GetUnitState().InRestingState == true);
	}

	public Player NextPlayer () {
		_currentPlayerID++;
		// If last player finished turn, select the first player.
		if (_currentPlayerID > AllPlayers.Count - 1) _currentPlayerID = FIRST_PLAYER_ID;
		_currentPlayer = AllPlayers[_currentPlayerID];
		//reactivate units.
		foreach (GameObject unit in _currentPlayer.AllUnits) {
			unit.GetComponent<Unit>().GetUnitState().Resting2Ready();
		}
		return _currentPlayer;
	}

	public List<GameObject> GetAllHostileUnits() {
		List<Player> enemies = GetAllHostilePlayers();
		return enemies.SelectMany(enemy => enemy.AllUnits).ToList();
		// SelectMany: https://stackoverflow.com/questions/958949/difference-between-select-and-selectmany
	}

	public List<Player> GetAllHostilePlayers() {
		return AllPlayers.Where(player => player.TeamID != _currentPlayer.TeamID).ToList();
	}

	public void OwningPlayerLooses(GameObject unit) {
		Player owningPlayer = AllPlayers.Where(p => p.PlayerID == unit.GetComponent<Unit>().OwningPlayerID).First();
		owningPlayer.AllUnits.Remove(unit);
	}

	public bool HasCurrentPlayerWon() {
		return GetAllHostileUnits() == null;
	}
}
