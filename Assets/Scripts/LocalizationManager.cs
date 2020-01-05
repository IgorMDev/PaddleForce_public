using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Xml;

public class LocalizationManager : MonoBehaviour {
    public static LocalizationManager instance;
    public SystemLanguage lang;
    Dictionary<string, string[]> localizedText;
    bool isReady = false;
    void Awake (){
        if(instance == null){
            instance = this;
        }else if(instance != this){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        if(lang == SystemLanguage.Unknown){
            lang = Application.systemLanguage;
        }
        switch(lang){
            case SystemLanguage.Ukrainian:
                LoadLocalizedText("uk");
                break;
            case SystemLanguage.Russian:
                LoadLocalizedText("ru");
                break;
            /* case SystemLanguage.German:
                LoadLocalizedText("de");
                break;
            case SystemLanguage.Spanish:
                LoadLocalizedText("es");
                break;
            case SystemLanguage.Chinese:
            case SystemLanguage.ChineseSimplified:
                LoadLocalizedText("zhch");
                break;
            case SystemLanguage.Arabic:
                LoadLocalizedText("ar");
                break;
            case SystemLanguage.Indonesian:
                LoadLocalizedText("id");
                break;
            case SystemLanguage.Portuguese:
                LoadLocalizedText("pt");
                break;
            case SystemLanguage.French:
                LoadLocalizedText("fr");
                break; */
        }
    }
    /* public void LoadLocalizedText(string fileName){
        
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        Debug.Log(filePath);
        if (File.Exists(filePath)) {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
            for (int i = 0; i < loadedData.items.Length; i++){
                localizedText.Add (loadedData.items[i].key, loadedData.items[i].value);   
            }
            Debug.Log ("Data loaded, dictionary contains: " + localizedText.Count + " entries");
            isReady = true;
        } else {
            Debug.LogError ("Cannot find file!");
        }
    } */
    public void LoadLocalizedText(string fileName){
        localizedText = new Dictionary<string, string[]>();
        TextAsset textAsset = Resources.Load<TextAsset>("i18n/"+fileName);
        if(textAsset != null){
            string dataAsJson = textAsset.text;
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
            for (int i = 0; i < loadedData.items.Length; i++){
                localizedText.Add (loadedData.items[i].key, loadedData.items[i].value);   
            }
            Debug.Log ("Data loaded, dictionary contains: " + localizedText.Count + " entries");
            isReady = true;
        }else{
            Debug.Log("File not found");
        }
    }
    public string[] GetLocalizedValue(string key){
        if (localizedText.ContainsKey(key)){
            return localizedText[key];
        }
        Debug.Log("Localized text not found");
        return null;
    }
    public bool GetIsReady(){
        return isReady;
    }
}
[System.Serializable]
public class LocalizationData{
    public LocalizationItem[] items;
}
[System.Serializable]
public class LocalizationItem{
    public string key;
    public string[] value;
}