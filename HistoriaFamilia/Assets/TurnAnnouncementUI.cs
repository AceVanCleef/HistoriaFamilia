using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnAnnouncementUI : MonoBehaviour {
	
	public SlideInAndOutAnimation animator;

	private Text _text;
	private string _prefix = "Day ";
	private int _turnCount = 1;

	void Start() {
		transform.position = new Vector3 (-13f, 0f, 0f);
		animator = gameObject.GetComponent<SlideInAndOutAnimation>();
		Debug.Log(animator);
		animator.Animate(transform);

		_text = GetComponentInChildren<Text>();
		_text.text = _prefix + _turnCount;
	}

	public void AnnounceTurn() {
		_text.text = _prefix + _turnCount;
		animator.Animate(transform);
	}

	public void IncrementTurnCount() {
		++_turnCount;
	}
}
