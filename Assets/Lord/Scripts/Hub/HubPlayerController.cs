using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HubPlayerController : MonoBehaviour
{
    private GameStateManager gameStateManager;

    private PlayerManager playerManager;
    private PlayerAnimation playerAnimation;

    [SerializeField] GameObject playerModel;

    public Transform weaponAttachPoint;
    public Transform offhandAttachPoint;

    [SerializeField] Transform groundCheck;

    GameObject currentPositionTile;

    public bool inputEnabled = false;

    private void Start()
    {
        gameStateManager = GameStateManager.instance;

        playerManager = PlayerManager.instance;
        playerAnimation = GetComponent<PlayerAnimation>();

        SetWeaponModel();

        CheckGround(groundCheck.position);
    }

    private void Update()
    {
        CheckPauseInput();
        if (gameStateManager.currentState != GameStateManager.GameState.inGame)
        {
            return;
        }
        
        if (inputEnabled) CheckMovementInput();
    }

    public void SetInputBool(bool value)
    {
        inputEnabled = value;
    }

    private void CheckPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStateManager.instance.ToggleGameState(GameStateManager.GameState.inMenu);
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

    public void SetWeaponModel()
    {
        //remove existing weapon model
        if (weaponAttachPoint.childCount > 0)
        {
            foreach (Transform child in weaponAttachPoint.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in offhandAttachPoint.transform)
            {
                Destroy(child.gameObject);
            }
        }

        //Set animation type based on equipped weapon
        if (playerManager.currentWeapon != null)
        {
            playerAnimation.SetAnimationType(playerManager.currentWeapon.animationType);
        }
        else
        {
            playerAnimation.SetAnimationType(0);
        }

        //initialize weapon based on its type
        if (playerManager.currentWeapon is MeleeWeapon)
        {
            MeleeWeapon meleeWeapon = (MeleeWeapon)playerManager.currentWeapon;
            meleeWeapon.Initialize(weaponAttachPoint, offhandAttachPoint, null);
        }
        else if (playerManager.currentWeapon is RangedWeapon)
        {
            RangedWeapon rangedWeapon = (RangedWeapon)playerManager.currentWeapon;
            rangedWeapon.Initialize(weaponAttachPoint, offhandAttachPoint);
        }
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
