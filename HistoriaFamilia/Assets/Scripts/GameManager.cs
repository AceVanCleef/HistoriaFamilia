using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	//#ensure singleton pattern
	public static GameManager instance = null;

	//public BoardManager BoardManager;

	// Use this for initialization
	void Start () {
		//#ensure singleton pattern
		// Note: gameObject = the GameObject containing this script.
		// Note: this = this script (component).
		if (instance == null)
		{
			instance = this;
		} else if (instance != this){
			Destroy(gameObject);
		}
		// avoid desctruction of GameManager, which keeps track of scores and level progression.
		DontDestroyOnLoad(gameObject);

		//BoardScript = GetComponent<BoardManager>();
		//InitGame();
	}

	void InitGame()
	{
		//BoardScript.SetupScene(level);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
