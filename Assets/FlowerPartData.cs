using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlowerPartData
{
    [SerializeField]
    public string name;
    [SerializeField]
    public float[] color;
    [SerializeField]
    public float[] vertices;
    [SerializeField]
    public int[] triangles;
    [SerializeField]
    public float[] uv;
   // [SerializeField]
    //public float[] normals;

    public FlowerPartData(FlowerPart flowerPart)
    {
        Mesh m = flowerPart.ml;

        name = m.name;

        vertices = new float[m.vertexCount * 3];
        for(int i=0;i<m.vertexCount;i++)
        {
            vertices[i * 3] = m.vertices[i].x;
            vertices[i * 3 + 1] = m.vertices[i].y;
            vertices[i * 3 + 2] = m.vertices[i].z;
        }

        color = new float[3];
        color[0] = flowerPart.lmat.color.r;
        color[1] = flowerPart.lmat.color.g;
        color[2] = flowerPart.lmat.color.b;

        triangles = new int[m.triangles.Length]; // initialize triangles array
        for (int i = 0; i < m.triangles.Length; i++) // Mesh's triangles is an array that stores the indices, sequentially, of the vertices that form one face
        {
            triangles[i] = m.triangles[i];
        }
        uv = new float[m.uv.Length * 2]; // initialize uvs array
        for (int i = 0; i < m.uv.Length; i++) // uv's Vector2 values are serialized similarly to vertices' Vector3
        {
            uv[i * 2] = m.uv[i].x;
            uv[i * 2 + 1] = m.uv[i].y;
        }

   

    }

    public Mesh getFlowerPartMesh()
    {
        Mesh m = new Mesh();

        m.name = name;

        List<Vector3> verticesList = new List<Vector3>();
        for (int i = 0; i < vertices.Length / 3; i++)
        {
            verticesList.Add(new Vector3(
                    vertices[i * 3], vertices[i * 3 + 1], vertices[i * 3 + 2]
                ));
        }
        m.SetVertices(verticesList);

        m.colors = new Color[verticesList.Count];
        m.colors.SetValue(new Color(color[0], color[1], color[2]), 0);

        m.triangles = triangles;
        List<Vector2> uvList = new List<Vector2>();
        for (int i = 0; i < uv.Length / 2; i++)
        {
            uvList.Add(new Vector2(
                    uv[i * 2], uv[i * 2 + 1]
                ));
        }
        m.SetUVs(0, uvList);



        m.RecalculateNormals();
        return m;
    }
}

