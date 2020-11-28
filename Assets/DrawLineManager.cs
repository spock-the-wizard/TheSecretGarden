using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEditor;
public class DrawLineManager : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public Material lMat;
    public GameObject ptr;
    public GameObject pen;
    private InputDevice targetDevice;
    private FlowerPart curPart;
    private int counter = 0;
    private int gripCount = 0;
    private static int count = 0;
    private DestroyFlower destroyFlowerScript;
    private Flower flower;

    public bool drawMode = true;
    public GameObject flowerObject; //current flower you are drawing
    private GameObject currentPartObject;
    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
        flower = flowerObject.AddComponent<Flower>();
        flower.flowerName = "testFlower11";
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

        destroyFlowerScript = ptr.GetComponent<DestroyFlower>();
        destroyFlowerScript.enabled = false;

        //Mesh m = SaveSystem.LoadFlowerPartData("haha1");
        //GameObject newobj = createFlowerPart(m);

    }

   
    // create new flower part from scratch
    GameObject createFlowerPart(string name)
    {
        GameObject flwprt = new GameObject(name);
        flwprt.AddComponent<MeshFilter>().mesh.name = name ;
       // flwprt.GetComponent<MeshFilter>().mesh.name = name;
        flwprt.AddComponent<MeshRenderer>();
        //flwprt.AddComponent<BoxCollider>().isTrigger = true ;
        MeshCollider mshC = flwprt.AddComponent<MeshCollider>();
        mshC.convex = true ;
        mshC.isTrigger = false;
        //flwprt.AddComponent<Rigidbody>().useGravity = true;
       // flwprt.AddComponent<XRGrabInteractable>();
       // flwprt.GetComponent<Rigidbody>().useGravity = false;
       // mshC.convex = true;
        curPart = flwprt.AddComponent<FlowerPart>();
        //Debug.Log("createFlowerPart");
        curPart.setWidth(.03f);
    
        curPart.lmat = new Material(lMat);
        curPart.lmat.color = ColorManager.Instance.GetCurrentColor();
        
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
            if(targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButton) && targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool toggle))
            {
                if (toggle)
                {
                    if (gripCount == 0)
                    {
                        drawMode = !drawMode;
                        count = 0;
                        gripCount++;
                    }
                    
                }
                else if (drawMode)
                    {
                    gripCount = 0;
                    //Debug.Log("Entering DRAW mode");
                        destroyFlowerScript.enabled = false;
                        if (triggerButton && counter == 0)
                        {
                        int index = flower.parts.Count;
                            currentPartObject = createFlowerPart(flower.flowerName+index.ToString());

                            currentPartObject.transform.parent = flowerObject.transform;

                            counter++;
                        }
                        else if (triggerButton)
                        {
                            Quaternion offset = pen.transform.rotation;
                            curPart.AddPoint(ptr.transform.position, offset);
                            counter++;
                        }
                        else if (counter > 0)
                        {
                            //Debug.Log("Adding to flower object part " + curPart.ml.name);
                            flower.parts.Add(currentPartObject);
                            // add part to current flower
                            
                            //SaveSystem.SaveFlowerPart(currLine);
                            //Debug.Log(currLine.ml.name);
                            counter = 0;
                            curPart = null;
                        }

                    }
                    else //eraseMode
                    {
                    gripCount = 0;
                    //Debug.Log("entering erase Mode");
                    ptr.GetComponent<DestroyFlower>().enabled = true;
                    /*
                        if (triggerButton && counter == 0)
                        {
                            destroyFlowerScript.enabled = true;

                            Debug.Log(destroyFlowerScript.enabled);// "Destroy flower script enabled");
                            counter++;
                        }
                        else if (triggerButton)
                            counter++;
                        else if (!triggerButton)
                        {
                            destroyFlowerScript.enabled = false;

                            Debug.Log(destroyFlowerScript.enabled);
                            counter = 0;
                        }*/
                }
                }

            }
            
            

        }

    }

