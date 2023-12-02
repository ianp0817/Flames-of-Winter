using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Persistent : MonoBehaviour
{
    private static Persistent Instance;
    private const string filepath = "/save.json";

    [System.Serializable]
    private class SaveData
    {
        public int coreFragmentBitField = 0;
        public int continueLevelIndex = 0;
    }

    private SaveData data;

    public static int CFBits { get { return Instance.data.coreFragmentBitField; } set { Instance.data.coreFragmentBitField = value; } }
    public static int LvlIdx { get { return Instance.data.continueLevelIndex; } set { Instance.data.continueLevelIndex = value; } }

    public static void ClearData()
    {
        Instance.data = new();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        string path = Application.persistentDataPath + filepath;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            data = new();
        }
    }

    private void OnApplicationQuit()
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + filepath, json);
    }
}
