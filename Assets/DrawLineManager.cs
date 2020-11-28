using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEditor;
public class DrawLineManager : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public Material material;
    public GameObject ptr;
    public GameObject pen;
    private InputDevice targetDevice;
    private FlowerPart currentPart;
    private int counter = 0;
    private int gripCount = 0;
    private static int count = 0;
    //private DestroyFlower destroyFlowerScript;
    private Flower flower;
    public int flowerCnt;
    public bool drawMode = true;


    private float lowest_y = 100;

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


    }

   
    /*
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
    }*/
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
                    //destroyFlowerScript.enabled = false;
                    if (triggerButton && counter == 0)
                    {
                        if(flower==null)
                            flower = Flower.createFlowerObject(count).GetComponent<Flower>();
                        int index = flower.parts.Count;
                        GameObject partObj =  FlowerPart.createPartObject(flower.flowerName+"."+index.ToString(), material);//FlowerPart(flower.flowerName+index.ToString());
                        partObj.transform.parent = flower.gameObject.transform;
                        currentPart = partObj.GetComponent<FlowerPart>();
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

