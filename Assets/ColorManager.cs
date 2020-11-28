using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    private Color color;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    void OnColorChange(HSBColor color)
    {
        //Debug.Log("color is " + color);
        this.color = color.ToColor();
    }
    public Color GetCurrentColor()
    {
        return this.color;
    }
}
