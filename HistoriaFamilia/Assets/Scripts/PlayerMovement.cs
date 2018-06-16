using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    public float speed;             //Floating point variable to store the player's movement speed.
    public Vector3 MoveDirection;
    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private float vertical;
    private float horizontal;
    public GameObject canvas, transitionObject;
    private String levelName;
    private GameObject obj;
    public PlayerInteraction interact;
    private bool bookbool, bedbool;
    public string myTrigger;
    // Use this for initialization
    void Start()
    {
        bedbool = false;
        bookbool = false;
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        Hide();
        obj = GameObject.Find("Book");
    }
    private void Update()
    {
      
        MoveDirection = CalculateDirection();
        TriggerSpave();
    }
    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        transform.Translate(MoveDirection * speed);
    }
        public Vector2 CalculateDirection()
        {
       
        Vector2 direction = Vector2.zero;

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
           
        {
            anim.SetInteger("Direction", 1);

            direction.y += speed;

        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
            anim.SetInteger("Direction", 2);

            direction.x -= speed;
        }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
            anim.SetInteger("Direction", 0);

            direction.y -= speed;
        }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
            anim.SetInteger("Direction", 3);

            direction.x += speed;
        }
            return direction.normalized;
        }
    void Hide()
    {

        canvas.SetActive(false);
    }
    void Show()
    {
        canvas.SetActive(true);
    }
   /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
         if (Input.GetKeyDown(KeyCode.Space)) {
            bookbool = true;

            Debug.Log("Space key was pressed.");
        
            //Add somwthing here  
            Invoke("Show", 1);
                Debug.Log("UI Active");


        }
        Debug.Log("I collided with"+collision.name);
        if (collision.gameObject.name == "Trainsition")
            Debug.Log("I try to send a message");
        bedbool = true;
           
        if (bookbool && bedbool)
        {
            SceneManager.LoadScene(levelName);
        }
        Debug.Log("send a message to Transition");

    }*/

    private void OnTriggerExit2D(Collider2D collision)
    {
        Hide();
        Debug.Log("I Hid The UI Senpai");
    }

   public void SetLevelName(string name)
    {
        levelName = name;
    }
    void TriggerSpave()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger(myTrigger);
            bookbool = true;

            Debug.Log("Space key was pressed.");

            //Add somwthing here  
            Invoke("Show", 1);
            Debug.Log("UI Active");
        }
    }
}