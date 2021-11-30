using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogActions : MonoBehaviour
{
    [Header("Recorded Data")]
    [SerializeField] List<Vector3> positions;
    public Transform player;
    [SerializeField] GameObject echo;
    public Transform echoTrans;

    public bool isRecording;
    public bool isReplaying;

    private int posIndex = 0;
    private int ReplayIndex = 0;
    private void Start()
    {
        positions = new List<Vector3>();
        echo.SetActive(false);

    }

    private void Update()
    {
        if(isRecording)
        {
            while(echo.activeInHierarchy)
            {
                echo.SetActive(false);
                positions.Clear();
            }
            //positions.Clear();
            Record();
            ReplayIndex = positions.Count - 1;
        }
        else if(isReplaying)
        {
            while(!echo.activeInHierarchy)
            {
                echo.SetActive(true);
            }

            echoTrans = echo.transform;

            ReplayIndex--;
            Replay();
            if (ReplayIndex == 0)
            {
                isReplaying = false;
            }
        }
    }

    void Record()
    {
        positions.Insert(posIndex, player.position);
        posIndex = posIndex++;
    }

    void Replay()
    {
        Debug.Log("Replaying");
        echo.transform.position = positions[ReplayIndex];
        Invoke("wait", 1f);
    }

    public void StartRecording()
    {
        isRecording = true;
    }

    public void EndRecording()
    {
        isRecording = false;
    }

    public void StartReplay()
    {
        isReplaying = true;
    }

    public void EndReplay()
    {
        isReplaying = false;
    }

    void wait()
    {
        return;
    }    
}
