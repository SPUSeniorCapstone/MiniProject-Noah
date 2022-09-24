using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 1;
    public float sprintModifier = 2;

    public Camera cam;
    public PlayerModel model;
    public Terrain terrain;

    void Start()
    {
        if(cam == null)
        {
            cam = Camera.main;
        }
        if(model == null)
        {
            Debug.LogError("PlayerController: Missing Player Model");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandelInput();
    }

    void HandelInput()
    {
        float speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = moveSpeed * sprintModifier;
        }
        else
        {
            speed = moveSpeed;
        }

        Vector3 newPos = model.transform.position;
        if (Input.GetAxis("Horizontal") != 0)
        {
            Vector3 moveDir = cam.transform.right;
            moveDir.y = 0;

            float moveHorizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            Vector3 temp = model.transform.position + moveDir * moveHorizontal;
            if (CheckAngle(temp))
            {
                newPos += moveDir * moveHorizontal;
            }
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            Vector3 moveDir = cam.transform.forward;
            moveDir.y = 0;

            float moveVertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            Vector3 temp = model.transform.position + moveDir * moveVertical;
            if (CheckAngle(temp))
            {
                newPos += moveDir * moveVertical;
            }
        }

        RotateTowardsMovement(newPos);

        if (CheckAngle(newPos))
        {
            float height = terrain.SampleHeight(newPos);
            newPos.y = height;

            if(newPos != model.transform.position)
            {
                model.animator.Play("RunForward");
            }else
            {
                model.animator.Play("Idle");
            }
            model.transform.position = newPos;
        }
    }

    bool CheckAngle(Vector3 newPos)
    {
        float height = terrain.SampleHeight(newPos);
        float slope = Mathf.Abs((height - model.transform.position.y) / Vector3.Distance(newPos, model.transform.position));
        if(slope > 1)
        {
            return false;
        }

        return true;
    }

    void RotateTowardsMovement(Vector3 newPos)
    {
        newPos.y = model.transform.position.y;
        Quaternion rotation = model.transform.rotation;

        Vector3 direction = newPos - model.transform.position;
        var lookRotation = Quaternion.LookRotation(direction);


        rotation = Quaternion.Slerp(rotation, lookRotation, Time.deltaTime * moveSpeed);
        model.transform.rotation = rotation;
    }
}
