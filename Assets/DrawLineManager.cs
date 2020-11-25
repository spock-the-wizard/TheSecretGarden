using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEditor;
public class DrawLineManager : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public Material lMat;
    public GameObject ptr;
    private InputDevice targetDevice;
    private LineMeshRenderer currLine;
    private int counter = 0;

    
    private GameObject flower;
    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
        flower = new GameObject("TestFlower");
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
            if(targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButton) && targetDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion quaternion))
            {
                if (triggerButton && counter == 0)
                {
                    Debug.Log("Adding Lines to Flower GameObject");
                    GameObject child = new GameObject("Line");
                    child.AddComponent<MeshFilter>();
                    child.AddComponent<MeshRenderer>();
                    currLine = child.AddComponent<LineMeshRenderer>();
                    currLine.setWidth(.03f);
                    currLine.lmat = new Material(lMat);
                    currLine.lmat.color = ColorManager.Instance.GetCurrentColor();

                    child.transform.parent = flower.transform;

                    counter++;
                }
                else if (triggerButton)
                {
                    Quaternion offset = new Quaternion(0.2f, 0.1f, -0.1f, 1.0f);
                    currLine.AddPoint(ptr.transform.position, offset * quaternion);
                    //currLine.AddPoint(this.transform.position);
                    counter++;
                }
                else if ( counter > 0)
                {
                    counter = 0;
                    currLine = null;
                }
            }

        }

    }
}
