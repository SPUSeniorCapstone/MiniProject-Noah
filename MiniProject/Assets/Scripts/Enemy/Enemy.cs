using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Enemy : Entity
{
    //Fields
    public float speed = 1;

    public float immunityTime = 0.3f;

    private float _stoppedTill = 0;

    private Vector3 knockbackVelocity = Vector3.zero;

    public bool ragdoll = false;

    public float jumpHeight = 1;
    public float jumpCoolDown = 1f;
    private float lockJumpTill = 0;

    private Vector3 lastPos = Vector3.zero;

    //Components
    private CharacterController characterController;



    //Cached Object
    private GameController gc;
    public GameObject target;
    private PlayerModel model;
    public GameObject root;


    protected override void Start()
    {
        base.Start();
        if(target == null)
        {
            target = FindObjectOfType<Player>().gameObject;
        }

        characterController = GetComponent<CharacterController>();
        gc = FindObjectOfType<GameController>();

        model = GetComponent<PlayerModel>();


    }

    protected override void Update()
    {
        base.Update();
        if (dead)
        {
            if (characterController.isGrounded)
            {
                EnableRagdoll();
                StartCoroutine(DestroyAfter(10));
            }
            return;
        }
        if(knockback.magnitude > 0)
        {
            state = EntityAnimator.AnimatorState.GET_HIT;
        }
        else
        {
            MoveTowardPlayer();
        }
        lastPos = transform.position;
    }

    /*
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
    }*/

    private void MoveTowardPlayer()
    {
        if (character.isGrounded)
        {
            state = EntityAnimator.AnimatorState.RUN_FORWARD;
        }
        else
        {
            state = EntityAnimator.AnimatorState.FALLING_LOOP;
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 1)
        {
            state = EntityAnimator.AnimatorState.PUNCH_LEFT;
            return;
        }
        if(Vector3.Distance(lastPos, transform.position) < (speed * Time.deltaTime)/3 && lockJumpTill < Time.time && character.isGrounded && Vector3.Distance(transform.position, target.transform.position) > 2)
        {
            if (state == EntityAnimator.AnimatorState.RUN_FORWARD || state == EntityAnimator.AnimatorState.SPRINT)
            {
                state = EntityAnimator.AnimatorState.JUMP_WHILE_RUNNING;
            }
            else
            {
                state = EntityAnimator.AnimatorState.JUMP;
            }
            Jump(jumpHeight);
            lockJumpTill = Time.time + jumpCoolDown;
        }

        var move = target.transform.position - transform.position;
        move = move.normalized;

        Move(move * speed * Time.deltaTime);

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
