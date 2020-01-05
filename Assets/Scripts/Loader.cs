using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour{

    void Awake(){
        
    }
    void Start(){
        StartCoroutine(Wait());
    }
    IEnumerator Wait(){
        yield return new WaitForSeconds(2);
    }
}