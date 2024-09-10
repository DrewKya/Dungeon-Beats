using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        CheckInput();
        Debug.Log(currentPositionTile.transform.position);
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) //move up
        {
            transform.position += new Vector3(0, 0, 1);
            CheckGround(groundCheck.position);
        }
        else if (Input.GetKeyDown(KeyCode.S)) //move down
        {
            transform.position += new Vector3(0, 0, -1);
            CheckGround(groundCheck.position);
        }
        else if (Input.GetKeyDown(KeyCode.D)) //move right
        {
            transform.position += new Vector3(1, 0, 0);
            CheckGround(groundCheck.position);
        }
        else if (Input.GetKeyDown(KeyCode.A)) //move left
        {
            transform.position += new Vector3(-1, 0, 0);
            CheckGround(groundCheck.position);
        }
    }

    private void CheckGround(Vector3 origin)
    {
        RaycastHit hit;

        if (Physics.Raycast(origin, Vector3.down, out hit, 1f, LayerMask.GetMask("Ground")))
        {
            //Debug.DrawRay(origin, Vector3.down * 1f, Color.yellow);

            GameObject obj = hit.collider.gameObject;

            currentPositionTile.GetComponentInChildren<Renderer>().material.color = Color.green;

            currentPositionTile = obj;

            if (obj.CompareTag("Ground"))
            {
                Renderer renderer = obj.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.red;
                }
            }

            
        }

    }
}
