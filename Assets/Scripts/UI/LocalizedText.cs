using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

using System.Xml.Linq;
[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour {
    public string before, key, after;
    public bool capitalizeFirst = true;
    void Start(){
        /* string res = matchString;
        if(res.Length > 0){
            res = "<root>"+res+"</root>";
            XElement xs = XElement.Parse(res);
            res = "";
            foreach(var el in xs.DescendantsAndSelf().Elements()){
                if(el.Name == "u"){
                    el.Value = el.Value.ToUpper();
                }
            }
            Debug.Log("xml is "+xs.Value); 
        }*/
        try{
            if(LocalizationManager.instance.GetIsReady()){
                string[] str = LocalizationManager.instance.GetLocalizedValue(key);
                if(str != null){
                    for(int i = 0; i < str.Length; i++){
                        str[i] = Concat(str[i]);
                    }
                    GetComponent<Text>().text = string.Join("\n", str);
                }
            }
        }catch{
            Debug.Log("Localization Manager not found");
        }
    }
    string Concat(string s){
        char[] cht = s.ToCharArray();
        
        if(capitalizeFirst && cht.Length > 0) cht[0] = char.ToUpper(cht[0]);
        return before + new string(cht) + after;
    }
}