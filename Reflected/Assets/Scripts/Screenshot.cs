using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Screenshot : MonoBehaviour
{
    public enum ImageFormat { JPG, PNG }

    [SerializeField] private ImageFormat imageFormat;
    [SerializeField] private string folderPath = "Screenshots/";

    private void Start()
    {
        folderPath = Application.dataPath;
        folderPath = folderPath.Remove(folderPath.LastIndexOf("Assets"));
        folderPath += "Screenshots/";
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
            {
                string fileName = folderPath + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.") + imageFormat.ToString().ToLower();
                Debug.Log("Screenshot taken: " + fileName);
                ScreenCapture.CaptureScreenshot(fileName);
            }
        }
    }
}
