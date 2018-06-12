using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInteraction : MonoBehaviour {
    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
            anim.SetBool("PickedUp", true);
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("PickedUp", false);

    }


}
