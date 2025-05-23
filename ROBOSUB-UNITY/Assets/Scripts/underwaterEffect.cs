﻿using UnityEngine;
using System.Collections;

public class underwaterEffect : MonoBehaviour
{
    public float waterHeight = 0.7f;
    public float waterFog = 0.05f;

    private bool isUnderwater;
    private Color normalColor;
    public Color underwaterColor = new Color(0.22f, 0.65f, 0.77f, 0.5f);

    // Use this for initialization
    void Start()
    {
        normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        //underwaterColor = new Color(0.22f, 0.65f, 0.77f, 0.5f);
        RenderSettings.fog = true;
    }

    // Update is called once per frame
    void Update(){
        if ((transform.position.y < waterHeight) != isUnderwater)
        {
            print("position: "+transform.position.y);
            print("Water height: "+waterHeight);
            isUnderwater = transform.position.y < waterHeight;
            if (isUnderwater) SetUnderwater();
            if (!isUnderwater) SetNormal();
        }
    }

    void SetNormal()
    {
        RenderSettings.fogColor = normalColor;
        RenderSettings.fogDensity = 0.01f;

    }

    void SetUnderwater()
    {
        Debug.Log("Under water");
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = waterFog;

    }
}