using System.Collections;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameController : MonoBehaviour{
    public static GameController instance;
    AudioSource audioSrc;
    //string gameId = "3177514";
    void Awake(){
        if(instance == null){
            instance = this;
        }else if (instance != this){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Prefs.Load();
        CheckDate();
        AchievementUnlocker.CheckConsecutiveDay();
        audioSrc = GetComponent<AudioSource>();
        DOTween.Init(true, true);
        if(Prefs.options.musicOn){
            audioSrc.Play();
        }
        if(Prefs.options.soundOn){
            AudioListener.volume = 1;
        }
    }
    void Start(){
        
    }
    public void MusicOn(float d = 0.5f){
        if(audioSrc != null){
            DOTween.To(() => audioSrc.volume, (x) => audioSrc.volume = x, 1, d);
            audioSrc.UnPause();
            if(!audioSrc.isPlaying){
                audioSrc.Play();
            }
        }
    }
    public void MusicOff(float d = 0.5f){
        if(audioSrc != null){
            DOTween.To(() => audioSrc.volume, (x) => audioSrc.volume = x, 0, d);
            audioSrc.Pause();
        }
    }
    public void CheckDate(){
        DateTime today = DateTime.Today;
        TimeSpan span = today.Subtract(DateTime.Parse(Prefs.arenaData.lastDate));
        if(span.Days == 1){
            Prefs.arenaData.consecutiveDay++;
        }else if(span.Days > 1){
            Prefs.arenaData.consecutiveDay = 0;
        }
        Prefs.arenaData.lastDate = today.ToString();
    }
    void OnDestroy(){
        if(instance == this)
            Prefs.Save();
    }

}
public class Prefs{
    public static float maxPrecise = 0.001f;
    public static ArenaData arenaData;
    public static Options options;
    static bool loaded = false;
    static string encKey = "yhfnvjgo", arenaDat;
    static Prefs(){
        Load();
    }
    public static void Load(){
        if(!loaded){
            LoadArenaData("Base");
            LoadOptions();
            loaded = true;
        }
    }
    public static void LoadArenaData(string loc){
        string spd = PlayerPrefs.GetString(loc);
        if(spd.Length > 0){
            string ard = DecryptData(spd, encKey);
            arenaData = JsonUtility.FromJson<ArenaData>(ard);
        }else{
            arenaData = new ArenaData{location = loc};
        }
    }
    public static void LoadOptions(){
        string spd = PlayerPrefs.GetString("options");
        if(spd.Length > 0){
            options = JsonUtility.FromJson<Options>(spd);
        }else{
            options = new Options();
        }
    }
    public static void Save(){
        string arenaDat = JsonUtility.ToJson(arenaData);
        arenaDat = EncryptData(arenaDat,encKey);
        PlayerPrefs.SetString(arenaData.location, arenaDat);
        string optDat = JsonUtility.ToJson(options);
        PlayerPrefs.SetString("options", optDat);
        PlayerPrefs.Save();
        Debug.Log("savedData\n"+JsonUtility.ToJson(arenaData,true));
        Debug.Log("savedOptions\n"+JsonUtility.ToJson(options,true));
    }
    public static void ConsolePrefs(){
        arenaDat = JsonUtility.ToJson(arenaData);
        arenaDat = EncryptData(arenaDat,encKey);
        Debug.Log("esavedData\n"+arenaDat);
    }
    public static void DConsolePrefs(){
        arenaDat = JsonUtility.ToJson(arenaData,true);
        Debug.Log("dsavedData\n"+arenaDat);
    }
    public static string EncryptData(string strData, string strEncDcKey){
        byte[] key = { };
        byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
        byte[] inputByteArray;
        try{
            key = Encoding.UTF8.GetBytes(strEncDcKey);
            DESCryptoServiceProvider ObjDES = new DESCryptoServiceProvider();
            inputByteArray = Encoding.UTF8.GetBytes(strData);
            MemoryStream Objmst = new MemoryStream();
            CryptoStream Objcs = new CryptoStream(Objmst, ObjDES.CreateEncryptor(key, IV), CryptoStreamMode.Write);
            Objcs.Write(inputByteArray, 0, inputByteArray.Length);
            Objcs.FlushFinalBlock();
            return Convert.ToBase64String(Objmst.ToArray());
        }
        catch{
            return null;
        }
    }
    public static string DecryptData(string strData, string strEndDcKey){
        byte[] key = { };
        byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
        byte[] inputByteArray = new byte[strData.Length];
        try{
            key = Encoding.UTF8.GetBytes(strEndDcKey);
            DESCryptoServiceProvider ObjDES = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(strData);
            MemoryStream Objmst = new MemoryStream();
            CryptoStream Objcs = new CryptoStream(Objmst, ObjDES.CreateDecryptor(key, IV), CryptoStreamMode.Write);
            Objcs.Write(inputByteArray, 0, inputByteArray.Length);
            Objcs.FlushFinalBlock();
            Encoding encoding = Encoding.UTF8;
            return encoding.GetString(Objmst.ToArray());
        }
        catch{
            return null;
        }
    }

}
[System.Serializable]
public class Options{
    public bool soundOn = true;
    public bool musicOn = true;

}
[System.Serializable]
public class ArenaData{
    public string location = "";
    public string lastDate = DateTime.Today.ToString();
    public int consecutiveDay = 0;
    public int playedMatches = 0;
    public int wonMatches = 0;
    public string ballName = "";
    public List<AchievementData> achievements = new List<AchievementData>();
    public ActorData actor1Data = new ActorData();
    public ActorData actor2Data = new ActorData();
}
[System.Serializable]
public class ActorData{
    public string brickName = "";
    public string colorName = "";
    public ControlType controls = ControlType.None;
}
[System.Serializable]
public class AchievementMenuData{
    public List<AchievementData> items = new List<AchievementData>();
}
[System.Serializable]
public class AchievementData{
    public AchievementType type = AchievementType.None;
    public bool unlocked;
}

public class ColorPalette{
    public static Color32[] palette1 = {
        new Color32(255, 159, 243, 255),
        new Color32(254, 202, 87, 255),
        new Color32(255, 107, 107, 255),
        new Color32(84, 160, 255, 255),
        new Color32(29, 209, 161, 255),
        new Color32(0, 210, 211, 255),
        new Color32(95, 39, 205, 255),
        new Color32(131, 149, 167,255),
    };
    public static string[] colorsNames = {
        "pink",
        "orange",
        "red",
        "blue",
        "green",
        "blueGreen",
        "purple",
        "gray",
    };
    public static Color GetColorByName(string cname){
        int i = Array.IndexOf(colorsNames, cname);
        return palette1[i];
    }
    public static string GetColorName(Color32 c){
        int i = Array.FindIndex(palette1, (Color32 cc) => {
            return c.r == cc.r && c.g == cc.g && c.b == cc.b;
        });
        return colorsNames[i];
    }
    public static Color32 ToColor(string cname){
        string[] cs = cname.Split(',');
        return new Color32(Byte.Parse(cs[0]),Byte.Parse(cs[1]),Byte.Parse(cs[2]),Byte.Parse(cs[3]));
    }
    public static string ToString(Color32 col){
        return col.r+","+col.g+","+col.b+","+col.a;
    }
    public static Color GetNextColor(string cname){
        int i = Array.IndexOf(colorsNames, cname);
        if(i == -1){
            return GetRandomColor();
        }
        if(i == colorsNames.Length-1){
            i = -1;
        }
        return palette1[++i];
    }
    public static string GetNextColorName(string cname){
        int i = Array.IndexOf(colorsNames, cname);
        if(i == -1){
            return GetRandomColorName();
        }
        if(i == colorsNames.Length-1){
            i = 0;
        }
        return colorsNames[i];
    }
    public static string GetNextColorNameExcept(string cname, string ex){
        int i = Array.IndexOf(colorsNames, cname);
        if(i == -1){
            return GetRandomColorNameExcept(ex);
        }
        string c;
        do{
            if(i == colorsNames.Length-1){
                i = -1;
            }
            c = colorsNames[++i];
        }while(c == ex);
        return c;
    }
    public static Color GetRandomColor(){
        return palette1[UnityEngine.Random.Range(0, palette1.Length)];
    }
    public static string GetRandomColorName(){
        return colorsNames[UnityEngine.Random.Range(0, colorsNames.Length)];
    }
    public static string GetRandomColorNameExcept(string cname){
        string c;
        do{
            c = colorsNames[UnityEngine.Random.Range(0, colorsNames.Length)];
        }while(c == cname);
        return c;
    }
}

public enum Alighment : sbyte{
    Left = -1, None = 0, Right = 1
}
public enum ControlType{
    None,
    Manual,
    Bot,
    Multiplayer
}
