using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyperlinks : MonoBehaviour
{
    public string url;

    /*
    public void OpenMusinsa()
    {
        Application.OpenURL(s);
    }
    */

    public void OpenURL(string link)
    {
        Application.OpenURL(url);
    }
}