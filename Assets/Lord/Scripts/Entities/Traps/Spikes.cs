using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spikes : MonoBehaviour, IActionable
{
    [SerializeField] private GameObject spikeModel;
    [SerializeField] private Animator animator;

    [SerializeField] private int durationOff = 1; //how many beats the spikes are off
    [SerializeField] private int durationOn = 1; //how many beats the spikes are on
    [SerializeField] private int damage = 1;

    private int cooldown; //this is a counter to determine when the spike is toggled on/off
    private bool isOn = true;

    private Collider hitBox;
    private List<GameObject> objectsInTrigger = new List<GameObject>();

    private void Start()
    {
        hitBox = GetComponent<Collider>();
        ToggleSpike();
    }


    public void TakeAction()
    {
        CheckCooldown();
        CheckObjectsInTrigger();
    }

    private void CheckCooldown()
    {
        if(cooldown > 0)
        {
            cooldown -= 1;
        }
        else
        {
            ToggleSpike();
        }
    }

    private void ToggleSpike()
    {
        if (isOn == false)
        {   //activate the spike
            isOn = true;
            animator.SetBool("isOn", true);
            hitBox.enabled = true;
            cooldown = durationOn - 1;
        }
        else
        {   //deactivate the spike
            isOn = false;
            animator.SetBool("isOn", false);
            hitBox.enabled = false;
            cooldown = durationOff - 1;
            objectsInTrigger.Clear();
        }
    }

    private void CheckObjectsInTrigger()
    {
        for (int i = objectsInTrigger.Count - 1; i >= 0; i--)
        {
            GameObject obj = objectsInTrigger[i];
            if (obj == null)
            {
                objectsInTrigger.RemoveAt(i);
                continue;
            }

            IDamageable damageable = obj.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        objectsInTrigger.Add(collider.gameObject);
    }

    private void OnTriggerExit(Collider collider)
    {
        objectsInTrigger.Remove(collider.gameObject);
    }
}
