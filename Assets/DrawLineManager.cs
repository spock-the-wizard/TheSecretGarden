using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEditor;
using UnityEngine.UI;
public class DrawLineManager : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public Material material;
    public GameObject ptr;
    public GameObject pen;
    public Slider slider;
    private InputDevice targetDevice;
    private FlowerPart currentPart;
    private int counter = 0;
    private Flower flower;
    private int nextFlowerIndex;

   // public GameObject flowerObject; //current flower you are drawing
    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();

        
        //flower = flowerObject.AddComponent<Flower>();
        //flower.flowerName = "testFlower11";
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

        nextFlowerIndex = SaveSystem.getValidIndex();
    }

  
   
    // Update is called once per frame
    void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            if (targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButton))
            {
                if (triggerButton && counter == 0)
                {
                    if (flower == null)
                    {
                        flower = Flower.createFlowerObject(nextFlowerIndex).GetComponent<Flower>();
                    }
                    int index = flower.parts.Count;
                    GameObject partObj = FlowerPart.createPartObject(flower.flowerName + "." + index.ToString(), material);//FlowerPart(flower.flowerName+index.ToString());
                    partObj.transform.parent = flower.gameObject.transform;
                    currentPart = partObj.GetComponent<FlowerPart>();
                    currentPart.setWidth(.01f+slider.value*.1f);
                    counter++;
                }
                else if (triggerButton)
                {
                    currentPart.AddPoint(ptr.transform.position, pen.transform.rotation);
                    counter++;
                }
                else if (counter > 0)
                {
                    flower.parts.Add(currentPart.gameObject);
                    counter = 0;
                    currentPart = null;
                }

            }
        }

        }
    public void undo()
    {
        if (flower != null)
        {
            flower.undoPart();
        }
    }

    }

