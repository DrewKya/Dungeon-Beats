using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class WeightedItem
{
    public Item item;
    public float probability;
}


[CreateAssetMenu(fileName = "New Loot Table", menuName = "Loot Table")]
[System.Serializable]
public class LootTable : ScriptableObject
{
    [SerializeField] List<WeightedItem> loots = new List<WeightedItem>();

    private float totalWeight;

    public void CalculateTotalWeight()
    {
        totalWeight = loots.Sum(loot => loot.probability);
    }

    public Item GetRandomItem()
    {
        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float currentWeight = 0f;

        foreach (WeightedItem loot in loots)
        {
            currentWeight += loot.probability;
            if(randomValue < currentWeight)
            {
                return loot.item;
            }
        }

        //if no valid loot is found
        return loots.LastOrDefault().item;
    }

    private void OnValidate()
    {
        //make sure no item has negative probability
        foreach(WeightedItem loot in loots)
        {
            loot.probability = MathF.Max(0, loot.probability);
        }
    }
}
