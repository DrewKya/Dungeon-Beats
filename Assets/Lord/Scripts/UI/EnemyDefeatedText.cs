using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDefeatedText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;

    private int maxCount = 0;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        maxCount = MapManager.instance.enemyRequirements;
        UpdateCount(MapManager.instance.EnemiesDefeated);

        MapManager.instance.onDefeatEnemies.AddListener(UpdateCount);
    }

    private void OnDestroy()
    {
        MapManager.instance.onDefeatEnemies.RemoveListener(UpdateCount);
    }

    private void UpdateCount(int count)
    {
        countText.text = $"Enemies Defeated\n" +
                            $"{count}/{maxCount}";
    }
}
