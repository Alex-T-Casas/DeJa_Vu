using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGateScript : MonoBehaviour
{
    LogActions log;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            log = other.gameObject.GetComponent<LogActions>();
            log.RemoveEcho();
            log.ClearData();
        }
    }
}
