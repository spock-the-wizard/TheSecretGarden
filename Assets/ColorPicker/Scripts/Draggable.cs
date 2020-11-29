using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class Draggable : MonoBehaviour
{

    public InputDeviceCharacteristics controllerCharacteristics;

    private InputDevice targetDevice;

    public GameObject controller;
    public GameObject hand;
    private int counter=0;

    public bool fixX;
	public bool fixY;
	public Transform thumb;	
	bool dragging;


    private void Start()
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
    void FixedUpdate()
	{
        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {

            if (targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButton))
            {
                LineRenderer controllerRot = controller.GetComponent<LineRenderer>();
                
                if (triggerButton && counter == 0)
                {
                    dragging = false;
                    Ray ray = new Ray(controller.transform.position, controllerRot.GetPosition(1)-controllerRot.GetPosition(0));

                    if (GetComponent<Collider>().Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                        dragging = true;
                    counter = 1;


                }
                else if (triggerButton)
                {
                    counter++;
                    
                    Ray ray = new Ray(controller.transform.position, controllerRot.GetPosition(1) - controllerRot.GetPosition(0));
                   
                    if (GetComponent<Collider>().Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                    {
                        var point = hit.point;
                        point = GetComponent<Collider>().ClosestPointOnBounds(point);
                        SetThumbPosition(point);
                        SendMessage("OnDrag", Vector3.one - (thumb.position - GetComponent<Collider>().bounds.min) / GetComponent<Collider>().bounds.size.x);

                    }
                }
                else if (!triggerButton && counter > 0) //last hit\
                {
                    dragging = false;
                    counter = 0;
                }
            }
        }
        

        /*
		if (Input.GetMouseButtonDown(0)) {
			dragging = false;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (collider.Raycast(ray, out hit, 100)) {
				dragging = true;
			}
		}
		if (Input.GetMouseButtonUp(0)) dragging = false;
		if (dragging && Input.GetMouseButton(0)) {
			var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			point = collider.ClosestPointOnBounds(point);
			SetThumbPosition(point);
			SendMessage("OnDrag", Vector3.one - (thumb.position - collider.bounds.min) / collider.bounds.size.x);
		}*/
    }

    
	void SetDragPoint(Vector3 point)
	{
		point = (Vector3.one - point) * GetComponent<Collider>().bounds.size.x + GetComponent<Collider>().bounds.min;
		SetThumbPosition(point);
	}

	void SetThumbPosition(Vector3 point)
	{
		thumb.position = new Vector3(fixX ? thumb.position.x : point.x, fixY ? thumb.position.y : point.y, thumb.position.z);
	}
}
