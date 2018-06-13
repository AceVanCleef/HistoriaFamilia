using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBringer : MonoBehaviour {

    GameObject g;

    private void Start()
    {
        g = GameObject.Find("Player");
    }
    public void IwantToTriggerThat()
    {
        g.SendMessage("SetLevelName","dev_Gameboard");
        Debug.Log("I sent message ");
    } 
}
