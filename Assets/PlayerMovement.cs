using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    

    public CharacterController2D controller;

    public float runSpeed = 20f;
    float horizontalMove = 0f;
    bool jumpMove = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal")*runSpeed;
        jumpMove = Input.GetAxisRaw("Vertical") == 1;
        controller.Move(horizontalMove, false, jumpMove);
    }

}
