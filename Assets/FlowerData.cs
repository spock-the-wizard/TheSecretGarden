using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
[System.Serializable]
public class FlowerData
{
    [SerializeField]
    public string flowerName;
    public string userName;
    [SerializeField]
    public int partCount;
    public float[] position;
    public string text;
    // [SerializeField]
    //public float[] normals;

    public FlowerData(Flower flower)
    {
        flowerName = flower.flowerName;
        userName = flower.userName;
        partCount = flower.parts.Count;
        text = flower.text;
        position = new float[3] { flower.position[0], flower.position[1], flower.position[2] };
    }

    
    public Flower getFlower(Material mat)
    {
        GameObject obj = new GameObject(flowerName);
        obj.tag = "Flower";
        BoxCollider boxC = obj.AddComponent<BoxCollider>();
        obj.AddComponent<XRGrabInteractable>();
        Flower flower = obj.AddComponent<Flower>();
        flower.flowerName = flowerName;
        flower.userName = userName;
        flower.position = new Vector3(position[0], position[1],position[2]);
        flower.text = text;
        for(int i=0;i<partCount;i++)
        {
            string NName = flowerName + "."+i.ToString();
            flower.parts.Add(LoadSystem.LoadFlowerPart(NName, mat));
            flower.parts[i].transform.parent = obj.transform;
        }


        // Sets box collider to region contain all and only the flower part meshes
        // doesn't change the gameobject's position, so use collider information to set coordinates
        Bounds bounds = new Bounds(flower.parts[0].GetComponent<FlowerPart>().ml.bounds.center, Vector3.zero);// Vector3.zero, Vector3.zero);\
        for(int i = 0; i < partCount; i++)
        {
            bounds.Encapsulate(flower.parts[i].GetComponent<FlowerPart>().ml.bounds);
        }
        //Vector3 lowest = new Vector3(0, bounds.center[1]- bounds.size[1], 0);
        //bounds.center -= lowest;
        for(int i=0;i<partCount;i++)
        {
            flower.parts[i].transform.Translate(-bounds.center);
        }
        boxC.size = bounds.size;
        //boxC.center = bounds.center;
        Rigidbody rigidBody = obj.GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        //rigidBody.useGravity = false;
        rigidBody.mass = 2.0f;
        return flower;
    }

}

