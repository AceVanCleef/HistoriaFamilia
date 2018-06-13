using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raven_left : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float speed = Time.deltaTime * 1.0f;
        transform.position += transform.right * speed;
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
