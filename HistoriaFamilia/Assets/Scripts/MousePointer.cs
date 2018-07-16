using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MousePointer  : MonoBehaviour {

	public static MousePointer Instance = null;

	//[HideInInspector]
	public GameObject Cursor;
	public bool DrawCursor = false;

	public Sprite tileCursorSprite;

	void Start() {
	//Singleton pattern.
		if (Instance == null)
		{
			Instance = this;
		} else if (Instance != this){
			Destroy(gameObject);
		}
	}


	void InitialiseTileCursor() {
		Cursor = new GameObject("Cursor");
		Cursor.transform.position = Vector3.zero;
		Cursor.transform.rotation = Quaternion.identity;
		SpriteRenderer sr = Cursor.AddComponent<SpriteRenderer>();
		sr.sprite = tileCursorSprite;
		sr.sortingOrder = 9999;
	}

	public void SetCursorPosition(int x, int y) {
		if (!DrawCursor) {
			if (Cursor != null && Cursor.activeInHierarchy) {
				Cursor.SetActive(false);
			}
			return;
		}
		if (Cursor == null) {
			InitialiseTileCursor();
		}
		if (!Cursor.activeInHierarchy) {
			Cursor.SetActive(true);
		}

		Cursor.transform.position = new Vector3(x, y, 0);
	}

	public void SetCursorColorTo(Color c) {
		SpriteRenderer sr = Cursor.GetComponent<SpriteRenderer>();
		sr.material.color = c;
	}
}
