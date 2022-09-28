using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        MELEE_ATTACK_1H = 2,
        JUMP = 3
    }

    public List<AudioClip> clips;

    private List<AudioSource> sources;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if(clips == null || clips.Count == 0)
        {
            Debug.LogError("No Audio Clips were given");
        }
        LoadAudioSources();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetState(AnimatorState s)
    {
        if (_lockedTill > Time.time) { return; }

        if (s == _state) return;
        _lockedTill = Time.time + GetStateTransitionTime(s);
        _state = s;

        var c = clips.Find(clip => (clip.name == GetStateString(s)));
        if(c != null)
        {
            sources[0].clip = c;
            sources[0].Play();
        }

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
        if (s == AnimatorState.JUMP) { return "Jump"; }


        return "tpose";
    }

    public float GetStateTransitionTime(AnimatorState s)
    {
        if (s == AnimatorState.IDLE) { return 0f; }
        if (s == AnimatorState.RUN_FORWARD) { return 0f; }
        if (s == AnimatorState.MELEE_ATTACK_1H) { return 0.876f; }
        if (s == AnimatorState.JUMP) { return 0.7f; }

        return 0f;
    }

    private void LoadAudioSources()
    {
        sources = GetComponents<AudioSource>().ToList();
    }
}
