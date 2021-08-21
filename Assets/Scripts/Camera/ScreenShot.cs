using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class ScreenShot : MonoBehaviour
{
    public Camera camera;// 보여지는 카메라

    private int resWidth;
    private int resHeight;
    string path;
    public GameObject curUI;

    void Start()
    {
        resWidth = Screen.width;
        resHeight = Screen.height;
        path = Application.dataPath + "/ScreenShot/";
        Debug.Log(path);
    }


    public void ClickScreenShot()
    {

        path += DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
        StartCoroutine(SaveScreenPng(path));
        path = Application.dataPath + "/ScreenShot/";
        
    }

    IEnumerator SaveScreenPng(string filePath)
    {
        curUI.SetActive(false);
        
        ScreenCapture.CaptureScreenshot(path);
        Debug.Log(path);
        yield return null;
        curUI.SetActive(true);
    }

    public void TakeScreenShot()
    {

        path += "CoderZero" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
        
        StartCoroutine(SaveScreenJpg(path));
        
        Debug.Log(path);
        path = Application.dataPath + "/ScreenShot/";
    }

    IEnumerator SaveScreenJpg(string filePath)
    {
        curUI.SetActive(false);
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(Screen.width, Screen.height);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();
        byte[] bytes = texture.EncodeToJPG();
        File.WriteAllBytes(filePath, bytes);
        DestroyImmediate(texture);
        curUI.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
