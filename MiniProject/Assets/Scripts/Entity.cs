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
    public float rotateSpeed = 10;
    public bool lockHorizontalRotation = true;
    public float health = 10;
    public EntityAnimator.AnimatorState state;
    public bool dead = false;

    public Transform weapons;
    protected Weapon weapon;
    public int weaponIndex = 0;

    //Private 
    Vector3 move = Vector3.zero;
    float jump = 0f;

    protected Vector3 knockback = Vector3.zero;

    //Components
    protected CharacterController character;
    protected GameController gameController;

    protected virtual void Start()
    {
        gameController = FindObjectOfType<GameController>();
        character = GetComponent<CharacterController>();

        if(weapons != null)
        {

            if (weaponIndex >= weapons.childCount || weaponIndex < 0)
            {
                weaponIndex = 0;
            }
            weapon = weapons.GetChild(weaponIndex).GetComponent<Weapon>();
            weapon.gameObject.SetActive(true);
        }

    }

    protected virtual void Update()
    {
        if (dead) return;

        Move(Vector3.up * jump * Time.deltaTime);
        if (useGravity)
        {
            jump -= G * Time.deltaTime;
            if (jump < -G)
            {
                jump = -G;
            }
        }

        character.Move(knockback * Time.deltaTime);
        knockback -= knockback.normalized * 2 * Time.deltaTime;
        if(knockback.magnitude < 0.2f)
        {
            knockback = Vector3.zero;
        }

        RotateTowardsMovement();
        character.Move(move);


        //Debug.Log(state);

        move = Vector3.zero;
    }

    public void DamageEntity(float damage, Vector3 knockback)
    {
        if (dead) return;

        //state = EntityAnimator.AnimatorState.GET_HIT;
        this.knockback = knockback;
        health -= damage;

        if(health <= 0)
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        dead = true;
        state = EntityAnimator.AnimatorState.DEATH;
        //StartCoroutine(Death());

        IEnumerator Death()
        {
            yield return new WaitForSeconds(1);
            Destroy(this.gameObject);
        }
    }


    public void Move(Vector3 motion)
    {
        move += motion;
    }

    public void Jump(float height)
    {
        jump = height * G_BASE;
    }

    void RotateTowardsMovement()
    {
        Vector3 test = move;
        test.y = 0;
        if (test != Vector3.zero)
        {
            RotateTowards(transform.position + move);
        }
    }

    protected void RotateTowards(Vector3 pos)
    {
        if (lockHorizontalRotation)
        {
            pos.y = transform.position.y;
        }
        Quaternion rotation = transform.rotation;

        Vector3 direction = pos - transform.position;
        var lookRotation = Quaternion.LookRotation(direction);


        rotation = Quaternion.Slerp(rotation, lookRotation, Time.deltaTime * rotateSpeed);
        transform.rotation = rotation;
    }
}
