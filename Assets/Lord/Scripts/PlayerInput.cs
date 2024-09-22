using TMPro;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] Transform groundCheck;

    GameObject currentPositionTile;

    MusicPlayer musicPlayer;
    TMP_Text timingText;

    private void Start()
    {
        musicPlayer = MusicPlayer.Instance;
        timingText = musicPlayer.timingText;

        CheckGround(groundCheck.position);
    }

    private void Update()
    {
        CheckMovementInput();
    }

    private void CheckMovementInput()
    {
        Vector3 positionIncrement = new Vector3(0, 0, 0);
        if (Input.GetKeyDown(KeyCode.W)) //move up
        {
            CheckTiming(musicPlayer.songPositionInBeats);
            positionIncrement = new Vector3(0, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S)) //move down
        {
            CheckTiming(musicPlayer.songPositionInBeats);
            positionIncrement = new Vector3(0, 0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D)) //move right
        {
            CheckTiming(musicPlayer.songPositionInBeats);
            positionIncrement = new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A)) //move left
        {
            CheckTiming(musicPlayer.songPositionInBeats);
            positionIncrement = new Vector3(-1, 0, 0);        
        }

        if(positionIncrement.magnitude > 0) //if a movement input is detected
        {
            Vector3 targetTilePosition = groundCheck.position + positionIncrement;
            if (CheckIfWalkable(targetTilePosition))
            {
                transform.position += positionIncrement;
                CheckGround(groundCheck.position);
            }
        }   
    }

    private bool CheckIfWalkable(Vector3 target)
    {
        RaycastHit hit;
        return (Physics.Raycast(target, Vector3.down, out hit, 1f, LayerMask.GetMask("Ground"))) ? true : false;
    }

    private void CheckGround(Vector3 origin)
    {
        RaycastHit hit;

        if (Physics.Raycast(origin, Vector3.down, out hit, 1f, LayerMask.GetMask("Ground")))
        {
            GameObject obj = hit.collider.gameObject;
            currentPositionTile = obj;
        }

    }
    public void CheckTiming(float inputTime)
    {
        float closestBeat = Mathf.Round(inputTime);

        float timeDifference = Mathf.Abs(closestBeat - inputTime); //time difference in beats

        //Debug.Log(timeDifference);
        if (timeDifference < 0.3f)
        {
            timingText.text = "Perfect";
        }
        else
        {
            timingText.text = "Miss";
        }
    }
}
