﻿using UnityEngine;
using System.Collections;
using DG.Tweening;


public class ShowFPS : MonoBehaviour
{

    public float f_UpdateInterval = 0.5F;

    private float f_LastInterval;

    private int i_Frames = 0;

    private float f_Fps;

    void Start()
    {
        //Application.targetFrameRate=60;

        f_LastInterval = Time.realtimeSinceStartup;
        
        i_Frames = 0;

        transform.DORotate(new Vector3(180,180,180), 2).SetEase(Ease.InElastic).OnComplete(() => transform.DOMoveY(5, 3).SetEase(Ease.InOutBack).SetDelay(2));
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 100, 200, 200), "FPS:" + f_Fps.ToString("f2"));
    }

    void Update()
    {
        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }
    }
}