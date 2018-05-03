using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    public List <GameObject> AllUnit = null;
    public int[,] PossitionUnit = null;
    public BoardManager BoardManager;

     void Start()
    {
       // PossitionUnit = new int[BoardManager.BoardSizeX,BoardManager.BoardSizeY];
    }
  
    public GameObject GetUnitAt(int x , int y)
    {
     GameObject KlickedUnit = AllUnit.Find(u => x == u.GetComponent<Unit>().TileX && y == u.GetComponent<Unit>().TileY);
        Debug.Log("KlickedUnit: " + KlickedUnit);
        return KlickedUnit;


    }

}
