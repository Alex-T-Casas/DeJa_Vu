using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogActions : MonoBehaviour
{
    [Header("Recorded Data")]
    [SerializeField] List<Vector3> positions;
    public Transform player;
    public Transform echo;
    public bool isRecording;
    public bool isReplaying;

    private void Start()
    {
        positions = new List<Vector3>();
    }

    private void Update()
    {
        if(isRecording)
        {
            Record();
        }
        else if(isReplaying)
        {
            Replay();
        }
    }

    void Record()
    {
        positions.Insert(0, player.position);
    }

    void Replay()
    {
        for(int i = 0; i < positions.Count; i++)
        {
            echo.position = positions[i];
        }
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
}
