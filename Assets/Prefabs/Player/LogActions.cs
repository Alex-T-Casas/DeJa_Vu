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

    private int posIndex = 0;
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
        positions.Insert(posIndex, player.position);
        posIndex = posIndex++;
    }

    void Replay()
    {
        Debug.Log("Replaying");
        for(int i = positions.Count - 1; i > 0; i--)
        {
            Debug.Log($"Currently on pos {i}");
            echo.transform.position = positions[i];
            
        }
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
