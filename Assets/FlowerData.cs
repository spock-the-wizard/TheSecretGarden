using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlowerData
{
    [SerializeField]
    public string flowerName;
    [SerializeField]
    public int partCount;
    // [SerializeField]
    //public float[] normals;

    public FlowerData(Flower flower)
    {
        flowerName = flower.flowerName;
        partCount = flower.parts.Count;
    }

    public Flower getFlower(Material mat)
    {
        GameObject obj = new GameObject(flowerName);
        Flower flower = obj.AddComponent<Flower>();
        flower.flowerName = flowerName;
        for(int i=0;i<partCount;i++)
        {
            string name = flowerName + i.ToString();
            flower.parts.Add(SaveSystem.LoadFlowerPart(name, mat));
            Debug.Log(flower.parts[i].GetComponent<FlowerPart>().ml.vertexCount);
            flower.parts[i].transform.parent = obj.transform;
        }

        return flower;
    }
    
    /*
    public GameObject getFlower(Material mat)
    {
        GameObject newFlower = new GameObject("Saved Flower");

        Flower flower = newFlower.AddComponent<Flower>();// new Flower();
        flower.flowerName = flowerName;


        flower.parts = new List<GameObject>();
        for (int i=0;i<partCount;i++)
        {
            Mesh m = SaveSystem.LoadFlowerPartData(flowerName + i.ToString());
            flower.parts.Add(FlowerPart.createFlowerPart(m, mat));
            flower.parts[i].transform.parent = newFlower.transform;
        }
        
        return newFlower;
    }
    */

}

