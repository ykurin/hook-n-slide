﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public CharacterController2D controller;

    public float runSpeed = 40f;

    private float horizontalMove = 0f;
    private bool jump = false;

    // Update is called once per frame
    void Update () {
        // Get input
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        // Apply input
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
