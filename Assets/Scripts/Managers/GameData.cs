using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine.SceneManagement;

// must run before other scripts
public class GameData : MonoBehaviour
{
    private BinaryFormatter formatter;

    // this game data
    private static GameData Instance;
    private Data data;

    // constants
    const string saveFileName = "SubliminalGameData.dat";

    // actual variables
    bool reverting = false, saving = false;

    void Awake()
    {
        if(Instance == null)
        {
            formatter = new BinaryFormatter();
            SceneManager.sceneLoaded += OnSceneLoaded;

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

    public void SaveAfterSceneChange()
    {
        saving = true;
    }

    public void SaveCurrentData(bool useCurrentScene = true)
    {
        Debug.Log("useCurrentScene=" + useCurrentScene);
        if(ScenesManager.GetCurrentScene() == ScenesManager.AllScenes.Menu || (!useCurrentScene && data.scene == ScenesManager.AllScenes.Menu))
        {
            Debug.LogError("tried to save data while on menu");
            return;
        }

        if(useCurrentScene)
        {
            // store level variables
            data.scene = ScenesManager.GetCurrentScene();
            StoreManagerVariables();

            if(data.scene == ScenesManager.AllScenes.CentralHub)
            {
                data.playVals = false;
            }

            // store player variables
            else if(GameManager.OneGM != null)
            {
                GameManager gm = GameManager.OneGM.GetComponent<GameManager>();
                data.playerHealth = gm.GetPlayerHealth();
                data.playerWeapon = gm.GetPlayerWeapon();

                GameObject player = GeneralFunctions.GetPlayer();

                if(player == null)
                {
                    Debug.LogError("Player not found in GameData. Save aborted.");
                    return;
                }

                data.playerPosition = new VectorsAreDumb(player.transform.position);
                data.playerDirection = player.GetComponent<Player_Movement>().flipped;
                data.playVals = true;
            }

            else
            {
                Debug.LogWarning("GameManager not found, resetting player values to default");
                data.playVals = false;
            }
        }


        FileStream file = File.Create(Application.persistentDataPath  + "/" + saveFileName); 
        formatter.Serialize(file, data);

        file.Close();
        // if( GeneralFunctions.IsDebug() ) Debug.Log("Game data saved!");
        if(GeneralFunctions.IsDebug())
            PrintSaveData("Save Data");
    }

    // doesn't actually save data
    public void ResetData()
    {
        data = new Data();
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
                Debug.LogWarning("data file formatting was out of date or invalid. Resetting data...");
            }

            file.Close();

            Debug.Log("Game data loaded!");
        }
        else
        {
            Debug.LogWarning("There is no save data!");
        }
    }

    public void RevertToSave()
    {
        if(!HasLoadData())
        {
            Debug.LogError("trying to load data without any saved data");
            return;
        }

        if(GeneralFunctions.IsDebug())
        {
            PrintSaveData("Load Data");
        }

        // set general manager vars
        InventoryManager.SetInventory(data.log.inventory);
        InventoryManager.SetUsed(data.log.usedInventory);
        LevelManager.SetObjectStates(data.log.levelManagerObjectState);
        UIManager.SetStates(data.log.interactiveStates);

        // castle vars
        MazeManager.SetMaze(data.log.maze);
        MazeManager.SetPath(data.log.mazePath);

        reverting = true;
        ScenesManager.LoadScene(data.scene);

    }

    public void RevertAfterDeath()
    {
        // reset player health
        data.playerHealth = GameManager.OneGM.GetComponent<GameManager>().GetMaxHealth();

        // save current health
        SaveCurrentData(false);

        // revert
        RevertToSave();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if( GeneralFunctions.IsDebug() ) Debug.Log("OnSceneLoaded: " + scene.name + " | " + mode);

        if(saving && reverting)
        {
            Debug.LogError("saving and reverting GameData at the same time");
        }

        if(reverting)
        {
            if(data.playVals)
            {
                // set player
                GameManager gm = GameManager.OneGM.GetComponent<GameManager>();
                if(data.playerHealth > 0) gm.SetPlayerHealth(data.playerHealth);
                gm.SetPlayerWeapon(data.playerWeapon);
                
                GameObject player = GeneralFunctions.GetPlayer();
                player.transform.position = data.playerPosition.Get();
                player.GetComponent<Player_Movement>().flipped = data.playerDirection;
            }

            reverting = false;
        }

        if(saving)
        {
            SaveCurrentData();
            saving = false;
        }
    }

    public void IncrementLevel()
    {
        data.level ++;
        Debug.Log("level is now " + data.level);
    }

    public void MindLoad(ScenesManager.AllScenes scene)
    {
        SaveAfterSceneChange();
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
    }

    public List<InventoryManager.AllItems> GetInventory()
    {
        return new List<InventoryManager.AllItems>( data.log.inventory );
    }

    public List<InventoryManager.AllItems> GetUsed()
    {
        return new List<InventoryManager.AllItems>( data.log.usedInventory );
    }

    public void UpdateMaze(Maze m)
    {
        data.log.maze = m;
        SaveCurrentData(false);
    }

    public void UpdatePath(Stack<Maze> path)
    {
        data.log.mazePath = new Stack<Maze>(path);
    }

    // data classes
    [System.Serializable]
    class Data
    {
        public ScenesManager.AllScenes scene;
        public int level;
        public bool playVals;
        public int playerWeapon;
        public int playerHealth;
        public bool playerDirection;
        public VectorsAreDumb playerPosition;
        // public int weapon;
        // public int player_direction;
        public LogData log;

        public Data()
        {
            scene = ScenesManager.AllScenes.Menu;
            level = 0;
            playVals = false;
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

    [Serializable]
    class VectorsAreDumb
    {
        float x, y, z;
        public VectorsAreDumb(Vector3 vec)
        {
            if(vec == null) return;

            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public Vector3 Get()
        {
            return new Vector3(x, y, z);
        }

        public override string ToString()
        {
            return "(" + x + ", " + y  + ", " + z + ")";
        }
    }

    public void PrintSaveData(string label)
    {
        Debug.Log("==============" + label + "============");
        Debug.Log("Scene: " + data.scene);
        Debug.Log("level: " + data.level);
        Debug.Log("playVals: " + data.playVals);

        if(data.playVals)
        {
            Debug.Log("player health: " + data.playerHealth);
            Debug.Log("player weapon: " + data.playerWeapon);
            Debug.Log("player position: " + data.playerPosition.ToString());
            Debug.Log("player is flipped?: " + data.playerDirection);
        }

        Debug.Log("Inventory: " + data.log.inventory.ToString());

        Debug.Log("==================================");
    }
}

/*

Notes

Current Save Locations:
After entering & exiting hub
After dying (resets health)
Maze generation (sets maze)

After reaching special room in maze
*/