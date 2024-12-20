using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] Transform groundCheck;

    GameObject currentPositionTile;

    MusicPlayer musicPlayer;
    TMP_Text timingText;

    PlayerManager playerManager;
    PlayerEntity playerEntity;
    PlayerAnimation playerAnimation;

    bool ultimateEnabled = false;
    bool inputEnabled = true;

    int lastInputBeat = -1; //this is to store in which beat the player last inputted an action

    private void Start()
    {
        playerManager = PlayerManager.instance;
        playerEntity = GetComponent<PlayerEntity>();
        playerAnimation = GetComponent<PlayerAnimation>();

        musicPlayer = MusicPlayer.Instance;
        timingText = musicPlayer.timingText;
        timingText.text = "";

        EnableUltimate(false);

        CheckGround(groundCheck.position);
    }

    private void Update()
    {
        CheckPauseInput();
        if (Time.timeScale == 0f || inputEnabled == false) return;

        CheckMovementInput();
        CheckUseItemInput();
        CheckAttackInput();
        CheckUltimateInput();
    }

    public void EnableInput(bool boolean)
    {
        inputEnabled = boolean;
    }

    public void EnableUltimate(bool boolean)
    {
        ultimateEnabled = boolean;
        IngameParametersUI.instance.SetUltimateIcon(boolean);
    }

    private void CheckPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameStateManager.instance.currentState == GameStateManager.GameState.gameOver)
            {
                return;
            }

            if (GameStateManager.instance.ToggleGameState(GameStateManager.GameState.inMenu))
            {
                inputEnabled = false;
                //musicPlayer.audioSource.Pause();
            }
            else
            {
                inputEnabled = true;
                //musicPlayer.audioSource.Play();
            }
        }
    }

    private void CheckMovementInput()
    {
        Vector3 positionIncrement = new Vector3(0, 0, 0);
        if (Input.GetKeyDown(KeyCode.W)) //move up
        {
            if (CheckTiming(musicPlayer.songPositionInBeats))
            {
                positionIncrement = new Vector3(0, 0, 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S)) //move down
        {
            if (CheckTiming(musicPlayer.songPositionInBeats))
            {
                positionIncrement = new Vector3(0, 0, -1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D)) //move right
        {
            if (CheckTiming(musicPlayer.songPositionInBeats))
            {
                positionIncrement = new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A)) //move left
        {
            if (CheckTiming(musicPlayer.songPositionInBeats))
            {
                positionIncrement = new Vector3(-1, 0, 0);
            }
        }

        if (positionIncrement.magnitude > 0) //if a movement input is detected and allowed
        {
            playerAnimation.RotatePlayer(positionIncrement);
            Vector3 targetTilePosition = groundCheck.position + positionIncrement;
            if (CheckIfWalkable(targetTilePosition, positionIncrement))
            {
                StartCoroutine(playerAnimation.HopAnimation(transform.position, transform.position + positionIncrement));

                transform.position += positionIncrement;
                CheckGround(groundCheck.position);
            }
        }
    }

    private bool CheckIfWalkable(Vector3 target, Vector3 direction)
    {
        RaycastHit hit;
        if(Physics.Raycast(groundCheck.position, direction, out hit, 1f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) //check if there is a collider in that direction
        {
            return false;
        }

        return (Physics.Raycast(target, Vector3.down, out hit, 1f, LayerMask.GetMask("Ground"))) ? true : false; //check if ground exist in that direction
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

    private void CheckAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            playerEntity.HoldAttack();
        }
        if (Input.GetKey(KeyCode.Mouse0) && playerEntity.isCharging)
        {
            playerAnimation.RotatePlayer(CheckPlayerDirectionByMouse());
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            playerEntity.Attack();
        }
    }

    private void CheckUltimateInput()
    {
        //if (!ultimateEnabled) return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            playerEntity.PreviewUltimate();
        }
        if (Input.GetKey(KeyCode.Mouse1) && playerEntity.isCharging)
        {
            playerAnimation.RotatePlayer(CheckPlayerDirectionByMouse());
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            playerEntity.UltimateAttack();
        }
    }

    private void CheckUseItemInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerEntity.UseItem();
        }
    }

    private Vector3 CheckPlayerDirectionByMouse()
    {
        Vector2 max = new Vector2(Screen.width, Screen.height);

        Vector2 position = Input.mousePosition / max;
        if (position.y > position.x && position.y > 1 - position.x)
        {
            return Vector3.forward;
        }
        else if (position.y > position.x && position.y < 1 - position.x)
        {
            return Vector3.left;
        }
        else if (position.y < position.x && position.y < 1 - position.x)
        {
            return Vector3.back;
        }
        else if (position.y < position.x && position.y > 1 - position.x)
        {
            return Vector3.right;
        }
        else
        {
            return Vector3.forward;
        }
    }

    public bool CheckTiming(float inputTime)
    {
        float closestBeat = Mathf.Round(inputTime);
        //Debug.Log(closestBeat);

        float timeDifference = Mathf.Abs(closestBeat - inputTime); //time difference in beats

        //Debug.Log(timeDifference);
        if (timeDifference <= 0.33f && closestBeat != lastInputBeat)
        {
            timingText.text = "Great!";
            timingText.color = Color.yellow;
            lastInputBeat = Mathf.RoundToInt(closestBeat);
            return true;
        }
        else
        {
            timingText.text = "Miss";
            timingText.color = Color.gray;
            playerEntity.TakeDamage(Mathf.RoundToInt(playerEntity.stats.healthPoint * 0.02f));
            lastInputBeat = Mathf.RoundToInt(closestBeat);
            return false;
        }

    }
}
