using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// must run before other scripts
public class GameData : MonoBehaviour
{
    private BinaryFormatter formatter;

    // this game data
    private static GameData Instance;
    private Data data;

    // constants
    const string saveFileName = "SubliminalGameData.dat";

    void Awake()
    {
        if(Instance == null)
        {
            formatter = new BinaryFormatter();

            Instance = this;
            DontDestroyOnLoad(this);

            // try to load the init
            if(SaveFileExists())
            {
                LoadData();
            }

            else
            {
                data = new Data();
            }
        }

        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void UpdateData()
    {
        
    }

    public void SaveCurrentData(bool useCurrentScene = true)
    {
        if(data.scene == ScenesManager.AllScenes.Menu)
        {
            Debug.LogError("tried to save data while on menu");
            return;
        }

        if(useCurrentScene)
        {
            data.scene = ScenesManager.GetCurrentScene();
            StoreManagerVariables();
        }

        FileStream file = File.Create(Application.persistentDataPath  + "/" + saveFileName); 
        formatter.Serialize(file, data);

        file.Close();
        Debug.Log("Game data saved!");
    }

    // doesn't actually save data
    public void ResetData()
    {
        data = new Data();
        // SaveCurrentData();
    }

    public void LoadData()
    {
        if (SaveFileExists())
        {
            FileStream file =  File.Open(Application.persistentDataPath  + "/" + saveFileName, FileMode.Open);

            try
            {
                data = (Data)formatter.Deserialize(file);
            }
            catch
            {
                data = new Data();
                Debug.LogError("data file formatting was out of date or invalid. Resetting data...");
            }

            file.Close();

            Debug.Log("Game data loaded!");
        }
        else
        {
            Debug.LogError("There is no save data!");
        }
    }

    public void RevertToSave()
    {
        if(!HasLoadData())
        {
            Debug.LogError("trying to load data without any saved data");
            return;
        }

        // set general manager vars
        InventoryManager._inventoryItems = data.log.inventory;
        InventoryManager._usedItems = data.log.usedInventory;
        LevelManager.SetObjectStates(data.log.levelManagerObjectState);
        UIManager.SetStates(data.log.interactiveStates);

        ScenesManager.LoadScene(data.scene);

        // set specific managers
        if(data.level == (int) HubManager.PhaseTag.Castle)
        {
            MazeManager.SetMaze(data.log.maze);
            MazeManager.SetPath(data.log.mazePath);
        }
    }

    public void IncrementLevel()
    {
        data.level ++;
        Debug.Log("level is now " + data.level);
    }

    public void MindLoad(ScenesManager.AllScenes scene)
    {
        data.scene = scene;
        SaveCurrentData(false);
    }

    public void SetLevel(int val)
    {
        data.level = val;
    }

    public int GetLevel()
    {
        return data.level;
    }

    public bool SaveFileExists()
    {
        return File.Exists(Application.persistentDataPath  + "/" + saveFileName);
    }

    public bool HasLoadData()
    {
        return data != null && data.scene != ScenesManager.AllScenes.Menu;
    }

    public static GameData GetInstance()
    {
        return Instance;
    }

    // assumption: PhaseTag integer is equal to level integer
    // assumption: maze will only need last room of maze
    public void StoreManagerVariables()
    {
        data.log.inventory = InventoryManager.GetInventoryItems();
        data.log.usedInventory = InventoryManager.GetUsedItems();
        data.log.levelManagerObjectState = LevelManager.GetObjectStates();
        data.log.interactiveStates = UIManager.GetStates();

        // castle level data
        if(ScenesManager.GetCurrentScene() == ScenesManager.AllScenes.CastleMaze)
        {
            data.log.maze = MazeManager.GetMaze();
            data.log.mazePath = MazeManager.GetPath();
        }

    }

    public List<InventoryManager.AllItems> GetInventory()
    {
        return data.log.inventory;
    }

    public List<InventoryManager.AllItems> GetUsed()
    {
        return data.log.usedInventory;
    }

    public void UpdateMaze(Maze m)
    {
        data.log.maze = m;
        SaveCurrentData(false);
    }

    // data classes
    [System.Serializable]
    class Data
    {
        public ScenesManager.AllScenes scene;
        public int level;
        // public int weapon;
        // public int player_direction;
        public LogData log;

        public Data()
        {
            scene = ScenesManager.AllScenes.Menu;
            level = 0;
            // weapon = 0;
            // player_direction = 1;
            log = new LogData();
        }

    }

    [Serializable]
    class LogData
    {
        public List<InventoryManager.AllItems> inventory;
        public List<InventoryManager.AllItems> usedInventory;
        public Dictionary<string, bool> levelManagerObjectState;
        public Dictionary<string, int> interactiveStates;

        // Castle Level
        public Maze maze;
        public Stack<Maze> mazePath;

        public LogData()
        {
            inventory = new List<InventoryManager.AllItems>();
            usedInventory = new List<InventoryManager.AllItems>();
            levelManagerObjectState = new Dictionary<string, bool>();
            interactiveStates = new Dictionary<string, int>();
            
            mazePath = new Stack<Maze>();
        }
    }
}