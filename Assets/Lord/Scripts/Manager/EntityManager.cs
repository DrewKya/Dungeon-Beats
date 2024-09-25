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
        if(instance != null)
        {
            Debug.LogWarning($"More than one instance of {instance.GetType()} found!");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
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

    public void RemoveEntity(IActionable entity)
    {
        actionableEntities.Remove(entity);
    }

    public void NotifyAllEntityToTakeAction()
    {
        for (int i = actionableEntities.Count - 1; i >= 0; i--)
        {
            if(actionableEntities[i] != null)
            {
                actionableEntities[i].TakeAction();
            }
        }
    }


}
