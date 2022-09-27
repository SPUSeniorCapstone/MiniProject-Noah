using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    [Serializable]
    public class GameSound
    {
        public string name;
        public List<AudioClip> clip;
        public float length = 0;
        public bool random = false;
        int index = 0;

        public AudioClip GetSound()
        {
            if (random)
            {
                var i = UnityEngine.Random.Range(0, clip.Count);
                return clip[i];
            }
            else
            {
                var c = clip[index];
                index++;
                if (index >= clip.Count) index = 0;
                return c;
            }
        }
    }

    [SerializeField]
    public List<GameSound> clips;
    private AudioSource audioSource;

    string state = "none";

    private float stoppedTill = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(clips == null || clips.Count == 0)
        {
            Debug.LogError("No clips were given");
        }
    }

    public void PlaySound(string soundName)
    {
        if (soundName == state && Time.time < stoppedTill) return;

        state = soundName;

        var sound = clips.Find(clip => clip.name == soundName);
        var c = sound.GetSound();
        stoppedTill = Time.time + sound.length;
        audioSource.PlayOneShot(c);
    }

    IEnumerator PlaySound(float waitFor)
    {


        yield return new WaitForSeconds(waitFor);
    }

}
