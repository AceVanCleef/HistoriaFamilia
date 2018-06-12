using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    public float speed;             //Floating point variable to store the player's movement speed.
    public Vector3 MoveDirection;
    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private float vertical;
    private float horizontal;
    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
    }
    private void Update()
    {
      
        MoveDirection = CalculateDirection();
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

    }