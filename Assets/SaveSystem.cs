using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
  public static void SaveFlowerPart(FlowerPart flowerPart)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + flowerPart.ml.name + ".flwpt";
        FileStream stream = new FileStream(path, FileMode.Create);

        FlowerPartData data = new FlowerPartData(flowerPart);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Mesh LoadFlowerPartData(string name)
    {
        string path = Application.persistentDataPath + "/" + name + ".flwpt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            FlowerPartData data = (FlowerPartData)formatter.Deserialize(stream);
            Mesh mesh = data.getFlowerPartMesh();
            return mesh;
        }
        else
        {
            Debug.LogError("save file not found");
            return null;
        }
    }
}
