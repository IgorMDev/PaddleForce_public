using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
[RequireComponent(typeof(Button))]
public class OpenURLBtn : MonoBehaviour{
    public string url;
    void Awake(){
        GetComponent<Button>().onClick.AddListener(GoToURL);
    }
    void GoToURL(){
        Application.OpenURL(url);
    }
}