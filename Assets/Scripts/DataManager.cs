using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    /* code to store persistence data */
    public static DataManager Instance;
    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // new data will be reached as DataManager.Instance.variable
    private int CharacterID = 0;
    private bool isMusic = true;
    private bool isSound = true;
    private int mode = 0;
    private string map = "Winter";

    public void SetCharacterID(int id)
    {
        CharacterID = id;
        Debug.Log("Set CharacterID to " + id);
    }
    public int GetCharacterID()
    {
        return CharacterID;
    }

    public void SetMusic(bool status)
    {
        isMusic = status;
        Debug.Log("Set Music to " + status);
    }
    public bool GetMusic()
    {
        return isMusic;
    }
    public void SetSound(bool status)
    {
        isSound = status;
        Debug.Log("Set Sound to " + status);
    }

    public bool GetSound()
    {
        return isSound;
    }

    public void SetMode(int level)
    {
        mode = level;
        Debug.Log("Set Mode to " + level);
    }

    public int GetMode()
    {
        return mode;
    }

    public void SetMap(string name)
    {
        map = name;
        Debug.Log("Set Map to " + name);
    }

    public string GetMap()
    {
        return map;
    }
}
