using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Enemy : MonoBehaviour
{
    //Fields
    public float speed = 1;

    public float health = 2;

    public float immunityTime = 0.3f;

    private float _stoppedTill = 0;

    private Vector3 knockbackVelocity = Vector3.zero;

    public bool ragdoll = false;


    public bool dead = false;

    //Components
    private CharacterController characterController;

    //Cached Object
    private GameController gc;
    public GameObject target;
    private PlayerModel model;
    public GameObject root;


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
        if (dead)
        {
            if (characterController.isGrounded)
            {
                EnableRagdoll();
                StartCoroutine(DestroyAfter(10));
            }
            return;
        }
        if (knockbackVelocity.magnitude == 0)
        {
            MoveTowardPlayer();
        }
        characterController.Move(knockbackVelocity * Time.deltaTime);
        
        if(knockbackVelocity.magnitude <= 0.2)
        {
            knockbackVelocity = Vector3.zero;
        }
        else
        {
            knockbackVelocity -= knockbackVelocity.normalized * 0.05f;
        }
    }

    public void EnemyHit(float damage, float knockback, float knockHeight = 0.7f)
    {
        if(dead){
            return;
        }

        health -= damage;
        _stoppedTill = Time.time + immunityTime;

        Vector3 moveDir = gc.playerController.model.transform.position - transform.position;
        moveDir.y = 0;
        var move = -moveDir.normalized * knockback;
        move.y = knockHeight;

        knockbackVelocity = move;

        if (health <= 0)
        {
            if (ragdoll)
            {
                dead = true;
                EnableRagdoll();
                //_stoppedTill = Time.time + 0.2f;

            }
            else
            {
                Destroy(this.gameObject);
                return;
            }

        }
        Debug.Log("Enemy Hit");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dead)
        {
            return;
        }
        if (other.transform.tag == "Weapon")
        {
            var obj = other.transform.GetComponent<Weapon>();
            if (obj != null )
            {
                if (gc.playerController.attacking && _stoppedTill < Time.time)
                {
                    EnemyHit(obj.damage, obj.knockback);

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
        if(Vector3.Distance(transform.position, target.transform.position) < 1)
        {
            model.animatorState = PlayerModel.AnimatorState.MELEE_ATTACK_1H;
            return;
        }
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

    private void EnableRagdoll()
    {
        GetComponent<Animator>().enabled = false;
        characterController.enabled = false;
        root.SetActive(true);
        root.transform.GetChild(0).GetComponent<Rigidbody>().velocity += knockbackVelocity;
    }

    IEnumerator DestroyAfter(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
