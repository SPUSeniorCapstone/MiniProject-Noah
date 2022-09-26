using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    GameController gc;
   
    void Start()
    {
        gc = FindObjectOfType<GameController>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (gc.Paused)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
