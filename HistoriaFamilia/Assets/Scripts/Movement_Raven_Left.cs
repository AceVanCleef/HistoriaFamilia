using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Raven_Left : MonoBehaviour {
    float timer = 0f;
    public GameObject GameObject;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

       
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
