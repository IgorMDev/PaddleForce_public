using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ActorEnergy : MonoBehaviour{
    
    Animator anim;
    public int energies;
    public bool isEmpty{
        get{return energies <= 0;}
    }
    public bool isFull{
        get{return energies >= transform.childCount;}
    }
    int currChild;
    void Awake(){
        anim = GetComponent<Animator>();
    }
    public void ShowEnergy(){
        anim.Play("showEnergy");
    }
    public void ShowUp(){
        anim.Play("showUp");
    }
    public void ShowDown(){
        anim.Play("showDown");
    }
    public void AddEnergy(){
        if(currChild < transform.childCount-1){
            currChild++;
            energies++;
            SetAlpha(transform.GetChild(currChild).gameObject, 1f);
        }
        anim.Play("showEnergy");
        
    }
    public void PopEnergy(){
        if(currChild >= 0){
            SetAlpha(transform.GetChild(currChild).gameObject, 0.5f);
            energies--;
            currChild--;
        }
    }
    void SetAlpha(GameObject go, float a){
        Color c = go.GetComponent<Image>().color;
        c.a = a;
        go.GetComponent<Image>().color = c;
    }
    public void Reset(){
        energies = transform.childCount;
        currChild = transform.childCount-1;
        for(byte i = 0; i < transform.childCount; i++){
            SetAlpha(transform.GetChild(i).gameObject, 1f);
        }
    }
}