using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerModel playerModel;
    public float autoRotateSpeed = 1;

    public float rotateSpeed = 1;
    public float zoomSpeed = 1;
    public float moveSpeed = 1;

    public bool rotateCamera = true;

    public float cameraHeight = 8;
    public float cameraDistance = 10;

    void Update()
    {
        ZoomWithScroll();
        if (Input.GetMouseButton(1))
        {
            RotateWithMouse();
        }
        if (rotateCamera)
        {
            RotateTowardsPlayer();
        }
        
        MoveTowardsPlayer();
    }

    void ZoomWithScroll()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            cameraHeight -= Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;

            if(Input.mouseScrollDelta.y > 0)
            {
                if (cameraHeight < 5)
                {
                    cameraDistance -= Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
                    if (cameraDistance < 4)
                    {
                        cameraDistance = 4;
                    }
                }
            }
            else
            {
                cameraDistance -= Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
                if(cameraDistance > 10)
                {
                    cameraDistance = 10;
                }
            }

            

            if (cameraHeight < 2)
            {
                cameraHeight = 2;
            }
            if (cameraHeight > 30)
            {
                cameraHeight = 30;
            }
        }
    }

    void RotateWithMouse()
    {
        float rotateAmount = (Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);

        transform.RotateAround(playerModel.transform.position, Vector3.up, rotateAmount);
    }

    void RotateTowardsPlayer()
    {
        Quaternion rotation = transform.rotation;

        Vector3 target = playerModel.transform.position + (Vector3.up * playerModel.modelHeight);

        Vector3 direction = target - transform.position;
        var lookRotation = Quaternion.LookRotation(direction);


        rotation = Quaternion.Slerp(rotation, lookRotation, Time.deltaTime * autoRotateSpeed);
        transform.rotation = rotation;
    }

    void MoveTowardsPlayer()
    {
        Vector3 newPosition = transform.position;
        Vector3 target = playerModel.transform.position;
        

        Vector3 temp = newPosition;
        temp.y = target.y;

        Vector3 dir = (target - temp).normalized;
        newPosition = target - (dir * cameraDistance);
        newPosition.y = target.y + cameraHeight;

        newPosition = Vector3.Slerp(transform.position, newPosition, moveSpeed * Time.deltaTime);
        transform.position = newPosition;
    }
}
