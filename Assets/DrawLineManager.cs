using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEditor;
public class DrawLineManager : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public Material lMat;
    private InputDevice targetDevice;
    private DrawingToMesh currLine;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            Debug.Log(targetDevice.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else {
            bool success = targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButton);
            if (success && triggerButton && counter == 0)
            {
                Debug.Log("create new object");
                GameObject go = new GameObject();
                go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>();
                currLine = go.AddComponent<DrawingToMesh>();
                currLine.setWidth(.05f);
                currLine.lmat = new Material(lMat);
                currLine.lmat.color = ColorManager.Instance.GetCurrentColor();

                counter++;
            }
            else if (success && triggerButton)
            {
                currLine.AddPoint(this.transform.position);
                counter++;
            }
            else if (counter > 0)
            {
                counter = 0;
            }

        }

    }
}
