using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInteraction : MonoBehaviour {
    private Animator anim;
    private AudioSource source;
    public AudioClip book;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;
    public string levelname;


    // Use this for initialization
    void Start () {
        anim = this.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        anim.SetBool("PickedUp", true);
        float vol = Random.Range(volLowRange, volHighRange);
        source.PlayOneShot(book, vol);
    }
  


    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("PickedUp", false);

    }
    


}
