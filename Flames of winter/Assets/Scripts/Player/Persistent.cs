using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Persistent : MonoBehaviour
{
    private static Persistent Instance;
    private const string savePath = "/save.json";
    private const string settingsPath = "/settings.json";

    [System.Serializable]
    private class SaveData
    {
        public int coreFragmentBitField = 0;
        public int continueLevelIndex = 0;
    }

    private SaveData data;

    public static int FCBits
    {
        get {
#if (UNITY_EDITOR)
            Init();
#endif
            return Instance.data.coreFragmentBitField; }
        set {
#if (UNITY_EDITOR)
            Init();
#endif
            Instance.data.coreFragmentBitField = value; }
    }
    public static int LvlIdx
    {
        get
        {
#if (UNITY_EDITOR)
            Init();
#endif
            return Instance.data.continueLevelIndex; }
        set
        {
#if (UNITY_EDITOR)
            Init();
#endif
            Instance.data.continueLevelIndex = value; }
    }

    [System.Serializable]
    private class Settings
    {
        public float masterVolume = 100f;
        public float mouseSensitivityX = 10f;
        public float mouseSensitivityY = 10f;
    }

    private Settings settings;

    public static float Volume {
        get
        {
#if (UNITY_EDITOR)
            Init();
#endif
            return Instance.settings.masterVolume; }
        set {
#if (UNITY_EDITOR)
            Init();
#endif
            Instance.settings.masterVolume = value < VolumeMin ? VolumeMin : value > VolumeMax ? VolumeMax : value; }
    }
    public static readonly float VolumeMin = 0f;
    public static readonly float VolumeMax = 100f;
    public static float SensitivityX {
        get
        {
#if (UNITY_EDITOR)
            Init();
#endif
            return Instance.settings.mouseSensitivityX; }
        set
        {
#if (UNITY_EDITOR)
            Init();
#endif
            Instance.settings.mouseSensitivityX = value < SensitivityMin ? SensitivityMin : value > SensitivityMax ? SensitivityMax : value; }
    }
    public static float SensitivityY {
        get
        {
#if (UNITY_EDITOR)
            Init();
#endif
            return Instance.settings.mouseSensitivityY; }
        set
        {
#if (UNITY_EDITOR)
            Init();
#endif
            Instance.settings.mouseSensitivityY = value < SensitivityMin ? SensitivityMin : value > SensitivityMax ? SensitivityMax : value; }
    }
    public static readonly float SensitivityMin = 1f;
    public static readonly float SensitivityMax = 50f;


    public static void ClearData()
    {
        Instance.data = new();
    }

    private static void Init()
    {
        if (Instance == null)
            new GameObject("Persistent").AddComponent<Persistent>();
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

        string path = Application.persistentDataPath + savePath;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            data = new();
        }

        path = Application.persistentDataPath + settingsPath;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            settings = JsonUtility.FromJson<Settings>(json);
        }
        else
        {
            settings = new();
        }
        AudioListener.volume = Volume / 100f;
    }

    private void OnApplicationQuit()
    {
        File.WriteAllText(Application.persistentDataPath + savePath, JsonUtility.ToJson(data));
        File.WriteAllText(Application.persistentDataPath + settingsPath, JsonUtility.ToJson(settings));
    }
}
