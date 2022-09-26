using System.Collections;
using System.Collections.Generic;
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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            paused = !paused;
        }
    }
}
