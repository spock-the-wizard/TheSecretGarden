using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
public class SaveSystem: MonoBehaviour
{
   // public GameObject flowerObject;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger with save button!");

        GameObject obj = GameObject.Find("New Flower");
        if (obj != null && obj.GetComponent<Flower>() != null)
        {
            SaveFlower(obj.GetComponent<Flower>());
        }
        else
        {
            Debug.Log("Nothing to save, redirecting to Garden");
            SceneManager.LoadScene("GardenScene");
        }
    }
    
    public void SaveFlowerPart(FlowerPart flowerPart)
    {
        
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + flowerPart.ml.name + ".flwpt";
        FileStream stream = new FileStream(path, FileMode.Create);

        FlowerPartData data = new FlowerPartData(flowerPart);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Saving Part " + flowerPart.partName);
    }
 
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

}