using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public CharacterController2D controller;

    public float runSpeed = 40f;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool onHook = false;
    private DistanceJoint2D joint2D;
    private SpriteRenderer sprite;

    void Awake ()
    {
        joint2D = gameObject.GetComponent<DistanceJoint2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }


    void Start ()
    {
    }

    void Update () {
        // Get input
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Hook") && !controller.m_Grounded)
        {
            Debug.Log("Hook pressed");
            joint2D.enabled = true;
            onHook = true;
            sprite.color = Color.red;
        }
        if (onHook && (controller.m_Grounded || Input.GetButtonUp("Hook")))
        {
            Debug.Log("Hook released");
            joint2D.enabled = false;
            onHook = false;
            sprite.color = Color.white;
        }

    }

    void FixedUpdate()
    {
        if (!onHook)
        {
            // Apply input
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        }

        jump = false;
    }
}
