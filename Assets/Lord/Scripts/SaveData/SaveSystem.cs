using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(int slot, PlayerManager playerManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetDataPath(slot);
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(playerManager);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Game saved at " + path);
    }


    public static SaveData LoadPlayer(int slot)
    {
        string path = GetDataPath(slot);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log("Game loaded at " + path);
            return data;
        }
        else
        {
            Debug.LogError("Save File not found in " + path);
            return null;
        }
    }

    public static void ResetData(int slot)
    {
        string path = GetDataPath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted at " + path);
        }
    }

    private static string GetDataPath(int slot)
    {
        string fileName = $"saveData_{slot}.bin";
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}