using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
public class SaveSystem: MonoBehaviour
{
    // public GameObject flowerObject;
    public DeleteSystem deleteSystem;
    
    public void saveNewFlower()
    {
        GameObject obj = GameObject.Find("New Flower");
        if (obj != null && obj.GetComponent<Flower>() != null)
        {
            SaveFlower(obj.GetComponent<Flower>());
        }
    }

    public void returnToGarden()
    {
        if(SceneManager.GetActiveScene().name=="DrawScene")
            SceneManager.LoadScene("GardenScene");
    }

    public void saveFlowerChanges()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Flower");
        for (int i = 0; i < list.Length; i++)
        {
            Flower current = list[i].GetComponent<Flower>();
            Debug.Log(current.gameObject.transform.position);
            current.position = current.gameObject.transform.position;
            Debug.Log(current.position);
            SaveFlower(current);
        }

        deleteSystem.deleteFlowerFilesInBin();
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
        stream.Close();

        for (int i = 0; i < flower.parts.Count; i++)
        {
            SaveFlowerPart(flower.parts[i].GetComponent<FlowerPart>());
        }

        Debug.Log("Saving flower " + flower.flowerName);
    }
    
    public static int getValidIndex()
    {
        string path = Application.persistentDataPath + "/State.dat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            StateData data = (StateData)formatter.Deserialize(stream);
            stream.Close();
            int cnt = data.GetFlowerNum();

            for(int i = 0; i < cnt; i++)
            {
                string pth = Application.persistentDataPath + "/test" + i.ToString() + ".flw";
                if (!File.Exists(pth))
                    return i;
            }
            saveFlowerCnt(cnt+1);
            return cnt;
        }
        else
        {
            saveFlowerCnt(1);
            return 0;
        }
    }

    public static void saveFlowerCnt(int cnt)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/State.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        StateData data = new StateData(cnt);
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Saved State.dat to value " + cnt);
    }

}