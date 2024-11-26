using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string playerName;

    public SaveData(PlayerManager playerManager)
    {
        playerName = playerManager.playerName;
    }
}
