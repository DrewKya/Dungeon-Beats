using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] Transform groundCheck;

    GameObject currentPositionTile;

    MusicPlayer musicPlayer;
    TMP_Text timingText;

    int lastInputBeat = -1; //this is to store in which beat the player last inputted an action

    private void Start()
    {
        musicPlayer = MusicPlayer.Instance;
        timingText = musicPlayer.timingText;
        timingText.text = "";

        CheckGround(groundCheck.position);
    }

    private void Update()
    {
        CheckPauseInput();
        if (Time.timeScale == 0f) return;

        CheckMovementInput();
    }

    private void CheckPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale > 0)
            {
                PauseManager.instance.TogglePauseGame(true);
                musicPlayer.audioSource.Pause();
            }
            else
            {
                PauseManager.instance.TogglePauseGame(false);
                musicPlayer.audioSource.Play();
            }
        }
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

        if(positionIncrement.magnitude > 0) //if a movement input is detected and allowed
        {
            RotatePlayer(positionIncrement);

            Vector3 targetTilePosition = groundCheck.position + positionIncrement;
            if (CheckIfWalkable(targetTilePosition))
            {
                transform.position += positionIncrement;
                CheckGround(groundCheck.position);
            }
        }   
    }

    private void RotatePlayer(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        gameObject.transform.rotation = rotation;
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
        if (timeDifference <= 0.3f)
        {
            timingText.text = "Great!";
            timingText.color = Color.yellow;
        }
        else
        {
            timingText.text = "Miss";
            timingText.color = Color.gray;
        }

    }
}
