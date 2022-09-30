using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactString = "Talk";
    public string message = "Welcome to my world, traveler...";

    private GameController gc;

    private void Start()
    {
        gc = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        if(Vector3.Distance(gc.playerController.model.transform.position, transform.position) < 2)
        {
            return;
        }
    }

    public void PerformAction()
    {
        gc.PlayMessage(message);
    }
}
