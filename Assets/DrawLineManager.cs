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
    private FlowerPart currLine;
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

        Mesh m = SaveSystem.LoadFlowerPartData("haha1");
        GameObject newobj = createFlowerPart(m);

    }

    //reconstruct flower part from mesh 
    GameObject createFlowerPart(Mesh m)
    {
        GameObject flwprt = new GameObject(m.name);
        flwprt.AddComponent<MeshFilter>().mesh = m;
        flwprt.AddComponent<MeshRenderer>();
        FlowerPart pt = flwprt.AddComponent<FlowerPart>();
        pt.ml = m;
        pt.lmat = new Material(lMat);
        if(m.colors.Length!=0)
            pt.lmat.color = pt.ml.colors[0];

        return flwprt;
    }

    // create new flower part from scratch
    GameObject createFlowerPart(string name)
    {
        GameObject flwprt = new GameObject(name);
        flwprt.AddComponent<MeshFilter>();
        flwprt.AddComponent<MeshRenderer>();
        FlowerPart pt = flwprt.AddComponent<FlowerPart>();
        pt.setWidth(.3f);
        pt.ml.name = name;
        pt.lmat = new Material(lMat);
     
        pt.lmat.color = ColorManager.Instance.GetCurrentColor();
        
        return flwprt;
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
                    currLine = child.AddComponent<FlowerPart>();
                    currLine.setWidth(.03f);
                    currLine.name = "haha1";
                    currLine.lmat = new Material(lMat);
                    currLine.lmat.color = ColorManager.Instance.GetCurrentColor();

                    child.transform.parent = flower.transform;

                    counter++;
                }
                else if (triggerButton)
                {
                    Quaternion offset = new Quaternion(0.2f, 0.1f, -0.1f, 1.0f);
                    currLine.AddPoint(ptr.transform.position, offset * quaternion);
                    counter++;
                }
                else if (counter > 0)
                {
                   //SaveSystem.SaveFlowerPart(currLine);
                    //Debug.Log(currLine.ml.name);
                    counter = 0;
                    currLine = null;
                }

            }

        }

    }
}
