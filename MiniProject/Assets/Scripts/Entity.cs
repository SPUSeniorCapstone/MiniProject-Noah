using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Entity : MonoBehaviour
{
    public const float G_BASE = 10;
    public static float G = 10;



    //Public
    public bool useGravity = true;

    //Private 
    Vector3 move = Vector3.zero;
    float jump = 0f;


    //Components
    protected CharacterController character;

    protected virtual void Start()
    {
        character = GetComponent<CharacterController>();
    }

    protected virtual void Update()
    {
        Move(Vector3.up * jump * Time.deltaTime);
        if (useGravity)
        {
            jump -= G * Time.deltaTime;
            if (jump < -G)
            {
                jump = -G;
            }
            Debug.Log(jump);
        }

        character.Move(move);// += velocity * Time.deltaTime
        move = Vector3.zero;

    }


    public void Move(Vector3 motion)
    {
        move += motion;
    }

    public void Jump(float height)
    {
        jump = height * G_BASE;
    }
}
