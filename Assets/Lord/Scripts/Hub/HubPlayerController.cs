using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HubPlayerController : MonoBehaviour
{
    [SerializeField] Transform groundCheck;

    GameObject currentPositionTile;

    private void Start()
    {
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
            }
            else
            {
                PauseManager.instance.TogglePauseGame(false);
            }
        }
    }

    private void CheckMovementInput()
    {
        Vector3 positionIncrement = new Vector3(0, 0, 0);
        if (Input.GetKeyDown(KeyCode.W)) //move up
        {
            positionIncrement = new Vector3(0, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S)) //move down
        {
            positionIncrement = new Vector3(0, 0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D)) //move right
        {
            positionIncrement = new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A)) //move left
        {
            positionIncrement = new Vector3(-1, 0, 0);
        }

        if (positionIncrement.magnitude > 0) //if a movement input is detected and allowed
        {
            RotatePlayer(positionIncrement);

            Vector3 targetTilePosition = groundCheck.position + positionIncrement;
            if (CheckIfWalkable(targetTilePosition, positionIncrement))
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

    private bool CheckIfWalkable(Vector3 target, Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheck.position, direction, out hit, 1f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) //check if there is a collider in that direction
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
}
