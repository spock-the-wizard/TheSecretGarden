using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadSystem : MonoBehaviour
{
    public Material material;
    //public InputField nameField;
    //public InputField journalField;
    private Flower flower;
    private void Start()
    {
        int flwNum = getFlowerNum();
        for(int i = 0; i < flwNum; i++)
        {
            GameObject flw = LoadFlower("test" + i.ToString());
            if (flw != null)
            {
                Debug.Log("translate" + flw.GetComponent<Flower>().position);
                flw.transform.Translate(flw.GetComponent<Flower>().position);
                
            }
        }

        flower = GameObject.Find("test1").GetComponent<Flower>() ;
      
    }
    
    public static GameObject LoadFlowerPart(string name, Material mat)
    {
        string path = Application.persistentDataPath + "/" + name + ".flwpt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            FlowerPartData data = (FlowerPartData)formatter.Deserialize(stream);
            stream.Close(); 
            FlowerPart part = data.getFlowerPart(mat);
            Debug.Log("Loading Part " + name);
            return part.gameObject;
        }
        else
        {
            Debug.Log("save file " + path + " not found");
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
            stream.Close();
            Debug.Log("Loading Flower " + name);
            return data.getFlower(material).gameObject;
        }
        else
        {
            Debug.Log("save file "+path+" not found");
            return null;
        }
    }

    public int getFlowerNum()
    {
        string path = Application.persistentDataPath + "/State.dat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            StateData data = (StateData)formatter.Deserialize(stream);
            stream.Close();
            int cnt = data.GetFlowerNum();

            return cnt;
        }
        else
        {
            return 0;
        }
    }
}
