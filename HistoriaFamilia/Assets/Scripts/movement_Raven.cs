using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement_Raven : MonoBehaviour {
    float timer = 0f;
    public GameObject Raven;
	// Use this for initialization
	void Start () {

        //  Find Ravens's model
        Raven = GameObject.Find("Ravens_Right");
        //  Set animation mode to loop
        Raven.GetComponent<Animator>().Play("crow");
            }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        float speed = Time.deltaTime * 1.0f;
        transform.position += transform.right * speed;
       
    }
    
    
}

