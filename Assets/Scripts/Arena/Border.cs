using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour{
    public Transform top,bottom,left,right;
    void Start(){
        Rect rect = GetComponent<RectTransform>().rect;
        top.localScale = bottom.localScale = new Vector3(rect.width, 6,1);
        left.localScale = right.localScale = new Vector3(6,rect.height,1);
    }
}