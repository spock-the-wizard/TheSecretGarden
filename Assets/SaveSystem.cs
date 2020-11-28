using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem: MonoBehaviour
{
    public Material material;
    public GameObject flowerObject;

    private void Start()
    {
        GameObject savedFlower = LoadFlower("testFlower11");
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger with save button!");
        Flower flowerToSave = flowerObject.GetComponent<Flower>();
        if (flowerToSave != null)
        {
            //Debug.Log("Attempting to save flower " + flowerToSave.flowerName);
            SaveFlower(flowerToSave);
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision with save button!");
    }
    public void SaveFlowerPart(FlowerPart flowerPart)
    {
        
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + flowerPart.ml.name + ".flwpt";
        FileStream stream = new FileStream(path, FileMode.Create);

        FlowerPartData data = new FlowerPartData(flowerPart);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Saving Part " + flowerPart.ml.name);
    }

    
    /*
    //reconstruct flower part from mesh 
    private GameObject createFlowerPart(Mesh m)
    {
        GameObject flwprt = new GameObject(m.name);
        flwprt.AddComponent<MeshFilter>().mesh = m;
        flwprt.AddComponent<MeshRenderer>();
        MeshCollider mshC = flwprt.AddComponent<MeshCollider>();
        FlowerPart pt = flwprt.AddComponent<FlowerPart>();
        pt.ml = m;
        pt.lmat = new Material(Shader.Find("Diffuse"));
        if (m.colors.Length != 0)
            pt.lmat.color = m.colors[0];

        return flwprt;
    }
    */
    public static GameObject LoadFlowerPart(string name, Material mat)
    {
        string path = Application.persistentDataPath + "/" + name + ".flwpt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            FlowerPartData data = (FlowerPartData)formatter.Deserialize(stream);
            FlowerPart part = data.getFlowerPart(mat);
            Debug.Log("Loading Part " + name);
            return part.gameObject;
        }
        else
        {
            Debug.LogError("save file not found");
            return null;
        }
        
    }

    /*
    public static GameObject LoadFlowerPartData(string name)
    {
        string path = Application.persistentDataPath + "/" + name + ".flwpt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            FlowerPartData data = (FlowerPartData)formatter.Deserialize(stream);
            Mesh mesh = data.getFlowerPart(material);
            Debug.Log("Loading Part " + name);
            return mesh;
        }
        else
        {
            Debug.LogError("save file not found");
            return null;
        }
    }
    */
    public void SaveFlower(Flower flower)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + flower.flowerName + ".flw";
        FileStream stream = new FileStream(path, FileMode.Create);

        FlowerData data = new FlowerData(flower);

        formatter.Serialize(stream, data);

        for(int i = 0; i < flower.parts.Count; i++)
        {
            SaveFlowerPart(flower.parts[i].GetComponent<FlowerPart>());
        }
        stream.Close();

        Debug.Log("Saving flower " + flower.flowerName);
    }

    public GameObject LoadFlower(string name)
    {
        string path = Application.persistentDataPath + "/" + name + ".flw";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            FlowerData data = (FlowerData)formatter.Deserialize(stream);           
            Debug.Log("Loading Flower " + name);
            return data.getFlower(material).gameObject;
        }
        else
        {
            Debug.LogError("save file not found");
            return null;
        }
    }

    /*
    public GameObject LoadFlowerObject(string name)
    {
        string path = Application.persistentDataPath + "/" + name + ".flw";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            FlowerData data = (FlowerData)formatter.Deserialize(stream);

            GameObject flower = data.getFlower(material);
            Debug.Log("Loading Flower " + name);
            return flower;
        }
        else
        {
            Debug.LogError("save file not found");
            return null;
        }
    }*/

}
