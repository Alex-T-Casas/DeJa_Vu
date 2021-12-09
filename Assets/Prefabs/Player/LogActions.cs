using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogActions : MonoBehaviour
{
    [Header("Recorded Data")]
    [SerializeField] List<Vector3> positions;
    [SerializeField] List<Quaternion> rotations;
    [SerializeField] List<float> MoveingInputX;
    [SerializeField] List<float> MoveingInputY;
    public Transform player;
    [SerializeField] GameObject echo;
    public Transform echoTrans;

    private MovementControler movmentComp;
    public float MoveInputX;
    public float MoveInputY;
    private Animator animator;
    private int XIndex = 0;
    private int YIndex = 0;
    private int ReplayXIndex = 0;
    private int ReplayYIndex = 0;

    public bool isRecording;
    public bool isReplaying;
    bool hasRecorded = false;

    private int rotIndex = 0;
    private int posIndex = 0;
    private int ReplayPosIndex = 0;
    private int ReplayRotIndex = 0;
    private void Start()
    {
        rotations = new List<Quaternion>();
        positions = new List<Vector3>();
        MoveingInputX = new List<float>();
        MoveingInputY = new List<float>();
        echo.SetActive(false);

        movmentComp = GetComponent<MovementControler>();
        animator = echo.GetComponent<Animator>();

    }

    private void Update()
    {
        if (isRecording)
        {
            while (echo.activeInHierarchy)
            {
                echo.SetActive(false);
            }
            if(hasRecorded)
            {
                positions.Clear();
                rotations.Clear();
                MoveingInputX.Clear();
                MoveingInputY.Clear();
                hasRecorded = false;
            }

            Record();
            ReplayPosIndex = positions.Count - 1;
            ReplayRotIndex = rotations.Count - 1;
            ReplayXIndex = MoveingInputX.Count - 1;
            ReplayYIndex = MoveingInputY.Count - 1;
        }
        else if(isReplaying) // Add check if any record data is readable
        {
            while (!echo.activeInHierarchy)
            {
                echo.SetActive(true);
            }

            echoTrans = echo.transform;

            ReplayRotIndex--;
            ReplayPosIndex--;
            ReplayXIndex--;
            ReplayYIndex--;
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
        MoveingInputX.Insert(XIndex, movmentComp.MoveInputX);
        MoveingInputY.Insert(YIndex, movmentComp.MoveInputY);
        posIndex = posIndex++;
        rotIndex = rotIndex++;
        XIndex = XIndex++;
        YIndex = YIndex++;
    }

    void Replay()
    {
        Debug.Log("Replaying");
        echo.transform.position = positions[ReplayPosIndex];
        echo.transform.rotation = rotations[ReplayRotIndex];
        animator.SetFloat("Y", MoveingInputY[ReplayYIndex]);
        animator.SetFloat("X", MoveingInputX[ReplayXIndex]);
        Invoke("wait", 1f);
    }

    public void StartRecording()
    {
        isRecording = true;
    }

    public void EndRecording()
    {
        isRecording = false;
        hasRecorded = true;
    }

    public void StartReplay()
    {
        isReplaying = true;
    }

    public void EndReplay()
    {
        isReplaying = false;
        animator.SetFloat("Y", 0);
        animator.SetFloat("X", 0);
    }

    void wait()
    {
        return;
    }    
}
