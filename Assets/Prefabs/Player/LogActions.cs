using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogActions : MonoBehaviour
{
    [Header("Recorded Data")]
    [SerializeField] List<Vector3> positions;
    [SerializeField] List<Quaternion> rotations;
    public Transform player;
    [SerializeField] GameObject echo;
    public Transform echoTrans;

    public bool isRecording;
    public bool isReplaying;

    private int rotIndex = 0;
    private int posIndex = 0;
    private int ReplayPosIndex = 0;
    private int ReplayRotIndex = 0;
    private void Start()
    {
        rotations = new List<Quaternion>();
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
                rotations.Clear();
            }

            Record();
            ReplayPosIndex = positions.Count - 1;
            ReplayRotIndex = rotations.Count - 1;
        }
        else if(isReplaying) // Add check if any record data is readable
        {
            while(!echo.activeInHierarchy)
            {
                echo.SetActive(true);
            }

            echoTrans = echo.transform;

            ReplayRotIndex--;
            ReplayPosIndex--;
            Replay();
            if (ReplayPosIndex == 0 || ReplayRotIndex == 0)
            {
                isReplaying = false;
            }
        }
    }

    void Record()
    {
        Debug.Log("Recording");
        positions.Insert(posIndex, player.position);
        rotations.Insert(rotIndex, player.rotation);
        posIndex = posIndex++;
        rotIndex = rotIndex++;
    }

    void Replay()
    {
        Debug.Log("Replaying");
        echo.transform.position = positions[ReplayPosIndex];
        echo.transform.rotation = rotations[ReplayRotIndex];
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
