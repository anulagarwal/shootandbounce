using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class UpgradeData
{
    public string upgradeName;
    public int level;
    // Add more fields as needed
}

[System.Serializable]
public class GunData
{
    public int gunLevel;
    public Vector3 position;
    // Add more fields as needed
}

[System.Serializable]
public class GameData
{
    public List<UpgradeData> upgrades;
    public List<GunData> guns;
}



public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance { get { return _instance; } }

    private string savePath;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }


    public void Start()
    {
        LoadGame();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            DeleteSaveFile();
        }
    }
    public void DoSaveGame()
    {
        // Populate the data
        GameData gameData = new GameData();
        gameData.upgrades = GetUpgradeData(); // Implement this function to get upgrade data
        gameData.guns = GetGunData(); // Implement this function to get gun data
        SaveGame(gameData);
    }

    public List<UpgradeData> GetUpgradeData()
    {
        List<UpgradeData> ud = new List<UpgradeData>();

        UpgradeData fire = new UpgradeData();
        fire.upgradeName = "FireRate";
        fire.level = UpgradeManager.instance.currentFireRateLevel;

        UpgradeData dropSpeed = new UpgradeData();
        dropSpeed.upgradeName = "DropSpeed";
        dropSpeed.level = UpgradeManager.instance.currentDropSpeedLevel;


        UpgradeData dropValue = new UpgradeData();
        dropValue.upgradeName = "DropValue";
        dropValue.level = UpgradeManager.instance.currentDropValueLevel;


        UpgradeData buyGun = new UpgradeData();
        buyGun.upgradeName = "BuyGun";
        buyGun.level = GunSelectionGridManager.Instance.currentGunIndex;

        ud.Add(fire);
        ud.Add(dropSpeed);
        ud.Add(dropValue);
        ud.Add(buyGun);
        return ud;
    }

    public void DeleteSaveFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.json");

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.Log("No save file to delete.");
        }
    }

    public List<GunData> GetGunData()
    {
        List<GunData> ud = new List<GunData>();

        foreach (Gun g in GunSelectionGridManager.Instance.guns)
        {
            if (g.isPlaced)
            {
                GunData gd = new GunData();
                gd.gunLevel = g.level;
                gd.position = g.transform.position;
                ud.Add(gd);
            }
        }
        return ud;
    }

    public void SaveGame(GameData gameData)
    {
        // Convert to JSON
        string json = JsonUtility.ToJson(gameData);

        // Write to a file
        File.WriteAllText(savePath, json);
    }

    public void RestoreUpgradeData(List <UpgradeData> ud)
    {
        UpgradeManager.instance.RestoreUpgrades(ud);
    }

    public void RestoreGunData(List<GunData> gd)
    {
        GunSelectionGridManager.Instance.RestoreGuns(gd);       
    }


    public GameData LoadGame()
    {
        if (File.Exists(savePath))
        {
            // Read from a file
            string json = File.ReadAllText(savePath);

            // Convert from JSON
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            // Restore the data
            RestoreUpgradeData(gameData.upgrades); // Implement this function to restore upgrade data
            RestoreGunData(gameData.guns); // Implement this function to restore gun data
            return gameData;
            
        }
        else
        {
            Debug.Log("No save file found, starting a new game.");
            return null; // or return a new GameData object with default values
        }
    }
}
