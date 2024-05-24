using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public Resolution[] GetAvailableResolutions()
    {
        return Screen.resolutions;
    }

    public void ChangeResolution(int width, int height, bool fullscreen)
    {
        Screen.SetResolution(width, height, fullscreen);
    }
}
