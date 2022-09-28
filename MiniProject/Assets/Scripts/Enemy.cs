using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Enemy : MonoBehaviour
{
    //Fields
    public float speed = 1;

    public float health = 2;

    public float immunityTime = 0.3f;

    private float _stoppedTill = 0;

    //Components
    private CharacterController characterController;

    //Cached Object
    private GameController gc;
    public GameObject target;
    private PlayerModel model;


    void Start()
    {
        if(target == null)
        {
            target = FindObjectOfType<PlayerController>().model.gameObject;
        }

        characterController = GetComponent<CharacterController>();
        gc = FindObjectOfType<GameController>();

        model = GetComponent<PlayerModel>();
    }

    void Update()
    {
        MoveTowardPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon")
        {
            var obj = other.transform.GetComponent<Weapon>();
            if (obj != null )
            {
                if (gc.playerController.attacking && _stoppedTill < Time.time)
                {
                    health -= obj.damage;
                    _stoppedTill = Time.time + immunityTime;

                    if(health <= 0)
                    {
                        Destroy(this.gameObject);
                        return;
                    }
                    Debug.Log("Enemy Hit");
                }
            }
            else
            {
                Debug.LogError("GameObject " + other.transform.name + " was tagged as a 'Weapon', but is missing the relevant component");
            }
        }
    }

    private void MoveTowardPlayer()
    {
        var move = target.transform.position - transform.position;
        move = move.normalized;

        RotateTowardsMovement(transform.position + move);

        characterController.Move(move * speed * Time.deltaTime);
        model.animatorState = PlayerModel.AnimatorState.RUN_FORWARD;
    }

    void RotateTowardsMovement(Vector3 newPos)
    {
        newPos.y = model.transform.position.y;
        Quaternion rotation = model.transform.rotation;

        Vector3 direction = newPos - model.transform.position;
        var lookRotation = Quaternion.LookRotation(direction);


        rotation = Quaternion.Slerp(rotation, lookRotation, Time.deltaTime * speed);
        model.transform.rotation = rotation;
    }
}
