using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
public class LoadSystem : MonoBehaviour
{
    public Material material;
    public int flowerCnt;
    private void Start()
    {
       GameObject flw = LoadFlower("test0");
        flw.transform.Translate(-7, 3, -19);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<Flower>())
        {
            Debug.Log("Redirecting to Draw Room");
            SceneManager.LoadScene("DrawScene");
        }
    }
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
}
