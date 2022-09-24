using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 1;
    public float jumpHeight = 5;
    public float sprintModifier = 2;

    public Camera cam;
    public PlayerModel model;
    public Terrain terrain;

    [SerializeField]
    public bool useRigidbody
    {
        get { return _useRigidBody; }
        set { _useRigidBody = value; model.GetComponent<Collider>().enabled = value; }
    }
    private bool _useRigidBody = true;

    private float _stoppedTill = 0;


    private float _jump = 0;
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        if (model == null)
        {
            Debug.LogError("PlayerController: Missing Player Model");
        }
        else
        {
            model.controller = this;
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

        if (Input.GetMouseButtonDown(0))
        {
            model.animatorState = PlayerModel.AnimatorState.MELEE_ATTACK_1H;
            _stoppedTill = Time.time + model.GetStateTransitionTime(PlayerModel.AnimatorState.MELEE_ATTACK_1H);
        }
        if (_stoppedTill > Time.time)
        {
            return;
        }

        Vector3 move = Vector3.zero;
        if(Input.GetAxis("Jump") != 0 && model.GetComponent<CharacterController>().isGrounded)
        {
            _jump = Input.GetAxis("Jump") * jumpHeight;

        }

        move += Vector3.up * _jump * Time.deltaTime;
        _jump = _jump - 0.1f;
        if (_jump < 0)
        {
            _jump = 0;
        }


        if (Input.GetAxis("Horizontal") != 0)
        {
            Vector3 moveDir = cam.transform.right;
            moveDir.y = 0;
            moveDir = moveDir.normalized;

            float moveHorizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            Vector3 temp = model.transform.position + moveDir * moveHorizontal;
            move += moveDir * moveHorizontal;
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            Vector3 moveDir = cam.transform.forward;
            moveDir.y = 0;
            moveDir = moveDir.normalized;

            float moveVertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            Vector3 temp = model.transform.position + moveDir * moveVertical;
            move += moveDir * moveVertical;

        }


        var h = move;
        h.y = 0;
        if (h.magnitude > 0.01 && model.GetComponent<CharacterController>().isGrounded)
        {
            model.animatorState = PlayerModel.AnimatorState.RUN_FORWARD;
        }
        else if(model.GetComponent<CharacterController>().isGrounded)
        {
            model.animatorState = PlayerModel.AnimatorState.IDLE;
        }

        RotateTowardsMovement(model.transform.position + move);

        model.GetComponent<CharacterController>().Move(move);
    }

    bool CheckAngle(Vector3 newPos)
    {
        float height = terrain.SampleHeight(newPos);
        float slope = Mathf.Abs((height - model.transform.position.y) / Vector3.Distance(newPos, model.transform.position));
        if (slope > 1)
        {
            return false;
        }

        return true;
    }

    void RotateTowardsMovement(Vector3 newPos)
    {
        if (newPos.x == model.transform.position.x && newPos.z == model.transform.position.z)
        {
            newPos = cam.transform.position;
        }
        newPos.y = model.transform.position.y;
        Quaternion rotation = model.transform.rotation;

        Vector3 direction = newPos - model.transform.position;
        var lookRotation = Quaternion.LookRotation(direction);


        rotation = Quaternion.Slerp(rotation, lookRotation, Time.deltaTime * moveSpeed);
        model.transform.rotation = rotation;
    }
}
