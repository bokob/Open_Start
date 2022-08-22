using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    [SerializeField]
    private GameObject Target;

    private bool is_active;
    void Start()
    {
        is_active = false;
        Target.SetActive(is_active);
    }

    public void ToggleSwitch()
    {
        is_active = !is_active;
        Target.SetActive(is_active);
    }
}
