using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnouncementUI : MonoBehaviour {
	
	private SlideInAndOutAnimation animator;

	private Text _mainText;
	private Text _subText;
	private string _turnPrefix = "Day ";
	private int _turnCount = 1;
	private string _playerPrefix = "Player ";

	void Start() {
		animator = gameObject.GetComponent<SlideInAndOutAnimation>();
		transform.position = animator.StartPosition;
		//Search Text components in grand Children by GameObject's name.
		_mainText = transform.Find("Canvas/MainText").GetComponent<Text>();
		_subText = transform.Find("Canvas/SubText").GetComponent<Text>();
	}

	// -------------------------- General Announcement ---------------------------------


	public void AnnounceMessage(string mainText, string subText) {
		_mainText.text = mainText;
		_subText.text = subText;
		animator.Animate(transform);
	}

	// -------------------------- Turn Announcement ---------------------------------


	public void AnnounceTurnOf(int nextPlayerID) {
		_mainText.text = _turnPrefix + _turnCount;
		_subText.text = _playerPrefix + nextPlayerID;
		animator.Animate(transform);
	}

	public void IncrementTurnCount() {
		++_turnCount;
	}
}
