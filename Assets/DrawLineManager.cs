using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class DrawLineManager : MonoBehaviour
{

    public InputDeviceCharacteristics controllerCharacteristics;
    

    private InputDevice targetDevice;
    private LineRenderer currLine;

    private int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("starting DrawLineManager.cs");
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];

            Debug.Log(targetDevice.name);
        }
        else
            Debug.Log("no devices found");
    }

    // Update is called once per frame
    void Update()
    {
        bool success = targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButton);
        if (success && triggerButton && counter == 0)
        {
            Debug.Log("create new object");
            GameObject go = new GameObject();
            currLine = go.AddComponent<LineRenderer>();
            currLine.startWidth = .1f;
            currLine.endWidth = .1f;
            counter++;
        }
        else if (success && triggerButton)
        {
            currLine.positionCount = counter;
           

            currLine.SetPosition(counter-1, this.transform.position);
            counter++;
        }
        else
            counter = 0;
    }
}
