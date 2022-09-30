using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public float speed = 10;
    public float jumpHeight = 3;

    override protected void Start()
    {
        base.Start();


    }

    override protected void Update()
    {
        base.Update();

        Vector3 v = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;
        Move(v * Time.deltaTime);

        if(Input.GetAxis("Jump") > 0 && character.isGrounded)
        {
            Jump(jumpHeight);
        }

    }
}
