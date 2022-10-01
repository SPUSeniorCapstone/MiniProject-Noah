using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public float speed = 10;
    public float sprintModifier = 1.5f;
    public float jumpHeight = 3;

    public float attackCooldown = 0.5f;
    float lockAttackTill = 0;

    public Transform weapons;
    private Weapon weapon;
    public int weaponIndex = 0;

    override protected void Start()
    {
        base.Start();

        weapon = weapons.GetChild(weaponIndex).GetComponent<Weapon>();
        weapon.gameObject.SetActive(true);
    }

    override protected void Update()
    {
        base.Update();
        HandleInput();
    }

    void HandleInput()
    {
        if (state > EntityAnimator.AnimatorState.IDLE_COMBAT && lockAttackTill > Time.time)
        {
            return;
        }
        //Movement
        Vector3 h = Input.GetAxis("Horizontal") * gameController.cameraController.GetXZRight();
        Vector3 v = Input.GetAxis("Vertical") * gameController.cameraController.GetXZForward();
        Vector3 move = h + v;

        float effectiveSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            effectiveSpeed *= sprintModifier;
        }
        Move(move * effectiveSpeed * Time.deltaTime);

        Vector3 test = move;
        test.y = 0;
        if (test.magnitude > 0 && character.isGrounded && Input.GetKey(KeyCode.LeftShift))
        {
            state = EntityAnimator.AnimatorState.SPRINT;
        }
        else if (test.magnitude > 0 && character.isGrounded)
        {
            state = EntityAnimator.AnimatorState.RUN_FORWARD;
        }
        else if (character.isGrounded)
        {
            if (weaponIndex != 0)
            {
                state = EntityAnimator.AnimatorState.IDLE_COMBAT;
            }
            else
            {
                state = EntityAnimator.AnimatorState.IDLE;
            }
        }
        else
        {
            state = EntityAnimator.AnimatorState.FALLING_LOOP;
        }

        //Jump
        if (Input.GetAxis("Jump") > 0 && character.isGrounded)
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
        }

        //Attack
        if (lockAttackTill < Time.time)
        {
            weapon.attacking = false;
            if (Input.GetMouseButtonDown(0))
            {
                if (weaponIndex != 0)
                {
                    state = EntityAnimator.AnimatorState.MELEE_ATTACK_ONE_HANDED;
                }
                else
                {
                    state = EntityAnimator.AnimatorState.PUNCH_RIGHT;
                }
                weapon.attacking = true;
                lockAttackTill = Time.time + attackCooldown;
            }
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            weapon.gameObject.SetActive(false);
            weaponIndex = 0;
            weapon = weapons.GetChild(weaponIndex).GetComponent<Weapon>();
            weapon.gameObject.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            weapon.gameObject.SetActive(false);
            weaponIndex = 1;
            weapon = weapons.GetChild(weaponIndex).GetComponent<Weapon>();
            weapon.gameObject.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            weapon.gameObject.SetActive(false);
            weaponIndex = 2;
            weapon = weapons.GetChild(weaponIndex).GetComponent<Weapon>();
            weapon.gameObject.SetActive(true);
        }
    }
}
