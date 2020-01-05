using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneBtn : MonoBehaviour{
    public string sceneName;
    public LoadSceneMode mode = LoadSceneMode.Single;
    void Awake(){
        if(sceneName.Length > 0){
            GetComponent<Button>().onClick.AddListener(LoadScene);
        }
    }
    void LoadScene(){
        Scene s = SceneManager.GetSceneByName(sceneName);
        if(s.IsValid() && s.isLoaded){
            SceneManager.SetActiveScene(s);
        }else{
            SceneManager.LoadScene(sceneName, mode);
        }
    }
    public void Click(){
        LoadScene();
    }
}