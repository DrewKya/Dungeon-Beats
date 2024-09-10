using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] Transform groundCheck;

    GameObject currentPositionTile;

    private void Start()
    {
        CheckGround(groundCheck.position);
    }

    private void Update()
    {
        CheckMovementInput();
        Debug.Log(currentPositionTile.transform.position);
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
            //Debug.DrawRay(origin, Vector3.down * 1f, Color.yellow);

            GameObject obj = hit.collider.gameObject;
            /*
            if (currentPositionTile != null)
            {
                currentPositionTile.GetComponentInChildren<Renderer>().material.color = Color.white;
            }
            */
            currentPositionTile = obj;
            /*
            if (obj.CompareTag("Ground"))
            {
                Renderer renderer = obj.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.red;
                }
            }
            */
            
        }

    }
}
