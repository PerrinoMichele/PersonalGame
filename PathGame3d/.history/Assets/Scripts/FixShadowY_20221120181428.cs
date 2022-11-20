using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixShadowY : MonoBehaviour
{
    public GameObject shadowCaster;
    public float shadowY = 0f;
    
    void Update() {
        Vector3 pos = shadowCaster.transform.position;
        pos.y = shadowY;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = pos;
    }
}