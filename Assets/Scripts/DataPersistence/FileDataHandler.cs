using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class FileDataHandler
{

    private string dataDirectory = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    public FileDataHandler(string dataDirectory, string dataFileName, bool useEncryption)
    {
        this.dataDirectory = dataDirectory;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirectory, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if (useEncryption)
                {
                    // Decrypt the data if required
                    dataToLoad = Decrypt(dataToLoad);
                }
                // deserialize the JSON data into a GameData object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load game data: {e.Message}");
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirectory, dataFileName);
        try
        {
            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the GameData object to JSON
            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption)
            {
                // Encrypt the data if required
                dataToStore = Encrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            Debug.Log("Game data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game data: {e.Message}");
        }
    }

    // BASE64 encoding for simplicity, replace with actual encryption if needed
    private string Encrypt(string data)
    {
        // Placeholder for encryption logic
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));
    }
    
    private string Decrypt(string data)
    {
        return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(data));
    }
}