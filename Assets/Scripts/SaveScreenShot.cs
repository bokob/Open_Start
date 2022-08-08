using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveScreenShot : MonoBehaviour
{
    public string m_Path = @"D:\Capture\Images\";   // 폴더가 있어야만 한다. 만약 없으면 안됨 -> 핸드폰으로 하는 방법 알아봐야 함
    public string m_FilePrefix = "CoderZero";   // 파일명    ex) CodeZero??? ???에 아무 숫자 들어간다.
    private string m_FilePath;

    public void Capture()
    {
        m_FilePath = m_Path + m_FilePrefix + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
        StartCoroutine(SaveScreeJpg(m_FilePath));
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.P))
        {
            m_FilePath = m_Path + m_FilePrefix + DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
            ScreenCapture.CaptureScreenshot(m_FilePath);
        }
        */

        /*
        if (Input.GetMouseButtonDown(0))
        {
            m_FilePath = m_Path + m_FilePrefix + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
            StartCoroutine(SaveScreeJpg(m_FilePath));
        }
        */

        /*
        if (Input.GetKeyDown(KeyCode.T))
        {
            m_FilePath = m_Path + m_FilePrefix + DateTime.Now.ToString("yyyyMMddhhmmss") + ".tga";
            StartCoroutine(SaveScreenTga(m_FilePath));
        }
        */

        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_FilePath = m_Path + m_FilePrefix + DateTime.Now.ToString("yyyyMMddhhmmss") + ".exr";
            StartCoroutine(SaveScreeExr(m_FilePath));
        }
        */
    }

    IEnumerator SaveScreeJpg(string filePath)
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(Screen.width, Screen.height / 2);
        texture.ReadPixels(new Rect(0, 750, Screen.width, Screen.height / 2), 0, 0);    // Rect(x축 시작 위치, y축 시작 위치)   // ReadPixels(원본, x축 어디, y축 어디)
        texture.Apply();
        byte[] bytes = texture.EncodeToJPG();   // jpg로 변환
        File.WriteAllBytes(filePath, bytes);
        DestroyImmediate(texture);
    }

    /*
    IEnumerator SaveScreenTga(string filePath)
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();
        byte[] bytes = texture.EncodeToTGA();   // tga로 변환
        File.WriteAllBytes(filePath, bytes);    
        DestroyImmediate(texture);
    }
    */

    /*
    IEnumerator SaveScreeExr(string filePath)
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBAFloat, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();
        byte[] bytes = texture.EncodeToEXR(Texture2D.EXRFlags.CompressZIP);
        File.WriteAllBytes(filePath, bytes);
        DestroyImmediate(texture);
    }
    */
}
