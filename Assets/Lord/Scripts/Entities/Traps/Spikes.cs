using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spikes : MonoBehaviour, IActionable
{
    public GameObject spikeModel;

    [SerializeField] private int durationOff = 1; //how many beats the spikes are off
    [SerializeField] private int durationOn = 1; //how many beats the spikes are on
    [SerializeField] private int damage = 1;

    public int cooldown; //this is a counter to determine when the spike is toggled on/off
    private bool isOn = true;

    private Collider hitBox;

    private void Start()
    {
        hitBox = GetComponent<Collider>();
        ToggleSpike();
    }


    public void TakeAction()
    {
        CheckCooldown();
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
            spikeModel.SetActive(true);
            hitBox.enabled = true;
            cooldown = durationOn - 1;
        }
        else
        {   //deactivate the spike
            isOn = false;
            spikeModel.SetActive(false);
            hitBox.enabled = false;
            cooldown = durationOff - 1;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerEntity>().TakeDamage(damage);
        }
    }
}
