using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance {  get; private set; }

    MusicPlayer musicPlayer;

    [SerializeField] private GameObject entityContainer;
    [SerializeField] private List<IActionable> actionableEntities;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.LogWarning("Make sure every entity is a child of entities container");

        musicPlayer = MusicPlayer.Instance;
        actionableEntities = new List<IActionable>();
        GetAllActionableEntities();
        musicPlayer.interval.trigger.AddListener(NotifyAllEntityToTakeAction);
    }

    private void GetAllActionableEntities()
    {
        foreach (Transform child in entityContainer.transform)
        {
            if(child.gameObject.GetComponent<IActionable>() != null)
            {
                actionableEntities.Add(child.gameObject.GetComponent<IActionable>());
            }
        }
    }

    public void NotifyAllEntityToTakeAction()
    {
        foreach(IActionable entity in actionableEntities)
        {
            entity.TakeAction();
        }
    }

}
