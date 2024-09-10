using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{


    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) //move up
        {
            transform.position += new Vector3(0, 0, 1);
            
        }
        else if (Input.GetKeyDown(KeyCode.S)) //move down
        {
            transform.position += new Vector3(0, 0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D)) //move right
        {
            transform.position += new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A)) //move left
        {
            transform.position += new Vector3(-1, 0, 0);
        }
    }

    private void CheckGround(Vector3 origin)
    {
        RaycastHit hit;
        if(Physics.Raycast(origin, Vector3.down, out hit))
        {
            if (hit.collider == null) //note : bikin buat check ground, bikin merah
            {

            }
        }
    }
}
