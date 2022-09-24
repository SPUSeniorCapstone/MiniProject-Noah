using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{

    public float modelHeight = 1;
    public Animator animator;
    public PlayerController controller;

    public AnimatorState animatorState
    {
        get { return GetState(); }
        set { SetState(value); }
    }

    private AnimatorState _state;

    private float _lockedTill;

    public enum AnimatorState { 
        IDLE = 0,
        RUN_FORWARD = 1,
        MELEE_ATTACK_1H = 2
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetState(AnimatorState s)
    {
        if(_lockedTill > Time.time) { return; }

        _lockedTill = Time.time + GetStateTransitionTime(s);
        animator.CrossFade(GetStateString(s), 0.01f, 0);
    }

    private AnimatorState GetState()
    {
        return _state;
    }

    private string GetStateString(AnimatorState s)
    {
        if (s == AnimatorState.IDLE) { return "Idle"; }
        if (s == AnimatorState.RUN_FORWARD) { return "RunForward"; }
        if (s == AnimatorState.MELEE_ATTACK_1H) { return "MeleeAttack_OneHanded"; }

        return "tpose";
    }

    public float GetStateTransitionTime(AnimatorState s)
    {
        if (s == AnimatorState.IDLE) { return 0f; }
        if (s == AnimatorState.RUN_FORWARD) { return 0f; }
        if (s == AnimatorState.MELEE_ATTACK_1H) { return 0.9f; }

        return 0f;
    }
}
