using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool Paused
    {
        get { return paused; }
        private set { paused = value; }
    }
    [SerializeField]
    private bool paused = false;

    public GameObject messageBox;
    public PlayerController playerController;

    private float unloadMessageBoxAt;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            paused = !paused;
            if (paused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
            
        }

        if(messageBox.activeSelf && Time.time > unloadMessageBoxAt)
        {
            messageBox.SetActive(false);
        }
    }

    public void PlayMessage(string message)
    {
        messageBox.SetActive(true);
        var m = messageBox.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        m.text = message;
        unloadMessageBoxAt = Time.time + 15;
    }
}
