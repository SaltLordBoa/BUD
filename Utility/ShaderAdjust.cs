using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderAdjust : MonoBehaviour
{
    public Material shader;
    public float colourThreshold;

    public void Start()
    {
        shader.SetFloat("_ColorThreshold", colourThreshold);
    }

}
