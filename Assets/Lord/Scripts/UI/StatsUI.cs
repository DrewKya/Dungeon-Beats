using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    PlayerManager playerManager;

    [SerializeField] TMP_Text coinText;
    [SerializeField] List<TMP_Text> statTexts = new List<TMP_Text>();

    private void Start()
    {
        playerManager = PlayerManager.instance;
        UpdateStatsUI();
    }

    private void OnEnable()
    {
        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        if (playerManager == null)
        {
            playerManager = PlayerManager.instance;
        }
        coinText.text = $"Coin : {playerManager.coin}";

        var stats = playerManager.playerStats;
        statTexts[0].text = stats.healthPoint.ToString();
        statTexts[1].text = stats.attack.ToString();
        statTexts[2].text = stats.defense.ToString();
        statTexts[3].text = stats.critRate.ToString();
    }
}
