using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPopup : MonoBehaviour
{
    public float lifetime = 1f;

    
    void Update()
    {
        Destroy(gameObject, lifetime);
    }
}
