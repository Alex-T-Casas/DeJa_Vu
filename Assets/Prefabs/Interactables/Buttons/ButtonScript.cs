using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] GameObject ToggelingObject;


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Box")
        {
            Debug.Log("Button Unpressed");
            OnObjectLeft();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Box")
        {
            Debug.Log("Button Pressed");
            OnObjectPlaced();
        }
    }

    public void OnObjectLeft()
    {
        gameObject.GetComponent<Toggleable>().ToggleOff();
        ToggelingObject.GetComponent<Toggleable>().ToggleOff();
    }

    public void OnObjectPlaced()
    {
        gameObject.GetComponent<Toggleable>().ToggleOn();
        ToggelingObject.GetComponent<Toggleable>().ToggleOn();
    }
}
