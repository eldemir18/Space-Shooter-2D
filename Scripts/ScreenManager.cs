using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    void Start()
    {
        Screen.SetResolution(Mathf.RoundToInt(Screen.height * (9.0f / 16.0f)), Screen.height, true);
    }
}
