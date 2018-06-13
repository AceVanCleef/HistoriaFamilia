using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHighlightManager : MonoBehaviour {
	// SpriteRenderer Doc: https://docs.unity3d.com/ScriptReference/SpriteRenderer.html

	private SpriteRenderer sp;
	private Color _initialColor;

	public Color HoverColor;
	public Color InMovementRangeColor;
	public Color InAttackRangeColor;
	
	void Start () {
		sp = GetComponent<SpriteRenderer>();
		_initialColor = sp.color;
	}

	public void SetToHoverColor () {
		sp.color = HoverColor;
	}

	public void ResetColor() {
		sp.color = _initialColor;
	}

	public void SetToMovementColor () {
		sp.color = InMovementRangeColor;
	}
}
