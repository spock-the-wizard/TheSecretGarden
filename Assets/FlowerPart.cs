using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class FlowerPart : MonoBehaviour
{
    public Material lmat;

    public Mesh ml;
    
    private Vector3 s;
    private float lineSize = .1f;
    private bool firstQuad = true;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("caught something");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered");
        Debug.Log(other.gameObject.name);
    }
    void Start()
    {
        ml =GetComponent<MeshFilter>().mesh;
        GetComponent<MeshCollider>().sharedMesh = ml;
        GetComponent<MeshRenderer>().material = lmat;
    }

    public void setWidth(float width)
    {
        lineSize = width;
    }


    Vector3[] MakeQuad(Vector3 s, Quaternion rot)
    {
        Vector3[] q = new Vector3[2];
        float w = lineSize / 2;
        // get right vector for controller
        Vector3 right = rot * Vector3.forward;
        right.Normalize();
        q[0] = transform.InverseTransformPoint(s + right * w);
        q[1] = transform.InverseTransformPoint(s + right * -w);

        return q;
    }
    
   //Quaternion needs to be quat of device controller
    public void AddPoint(Vector3 point, Quaternion rot)
    {
        AddLine(MakeQuad(point, rot));
        if (firstQuad)
            firstQuad = false;
    }

    public void AddPoint(Vector3 point)
    {

        if (s != Vector3.zero)
        {
            AddLine(ml, MakeQuad(s, point, lineSize, firstQuad));
            firstQuad = false;
        }

        s = point;
    }

    Vector3[] MakeQuad(Vector3 s, Vector3 e, float w, bool all)
    {
        w = w / 2;

        Vector3[] q;
        if (all)
        {
            q = new Vector3[4];
        }
        else
        {
            q = new Vector3[2];
        }

        Vector3 n = Vector3.Cross(s, e);
        Vector3 l = Vector3.Cross(n, e - s);
        l.Normalize();

        if (all)
        {
            q[0] = transform.InverseTransformPoint(s + l * w);
            q[1] = transform.InverseTransformPoint(s + l * -w);
            q[2] = transform.InverseTransformPoint(e + l * w);
            q[3] = transform.InverseTransformPoint(e + l * -w);
        }
        else
        {
            q[0] = transform.InverseTransformPoint(s + l * w);
            q[1] = transform.InverseTransformPoint(s + l * -w);
        }
        return q;
    }

    void AddLine(Vector3[] quad)
    {
        int vertexN = ml.vertices.Length;
        if (quad.Length != 2)
        {
            Debug.Log("wierd quad input");
            return;
        }

        Vector3[] newVertices = ml.vertices;
        newVertices = resizeVertices(newVertices, 4);
        
        for(int i = 0; i < 4; i ++)
            newVertices[vertexN + i] = quad[i / 2];

        Vector2[] newUVs = ml.uv;
        newUVs = resizeUVs(newUVs, 4);
        
        if(vertexN %8==0)
        {
            newUVs[vertexN] = Vector2.zero;
            newUVs[vertexN + 1] = Vector2.zero;
            newUVs[vertexN + 2] = Vector2.right;
            newUVs[vertexN + 3] = Vector2.right;
        }else if(vertexN % 4 == 0)
        {
            newUVs[vertexN] = Vector2.up;
            newUVs[vertexN + 1] = Vector2.up;
            newUVs[vertexN + 2] = Vector2.one;
            newUVs[vertexN + 3] = Vector2.one;
        }

        ml.vertices = newVertices;
        ml.uv = newUVs;

        if (!firstQuad)
        {
            int triN = ml.triangles.Length;
            int[] newTri = ml.triangles;
            newTri = resizeTriangles(newTri, 12);
            int v = vertexN - 4;
            //front-facing quad
            newTri[triN] = v;
            newTri[triN+1] = v+2;
            newTri[triN+2] = v+4;

            newTri[triN + 3] = v + 2;
            newTri[triN + 4] = v + 6;
            newTri[triN + 5] = v + 4;

            //back-facing quad
            newTri[triN + 6] = v + 5;
            newTri[triN + 7] = v + 3;
            newTri[triN + 8] = v + 1;

            newTri[triN + 9] = v + 5;
            newTri[triN + 10] = v + 7;
            newTri[triN + 11] = v + 3;

            ml.triangles = newTri;
        }
       

       
        
        ml.RecalculateBounds();
        ml.RecalculateNormals();

    }
    void AddLine(Mesh m, Vector3[] quad)
    {
        int vl = m.vertices.Length;

        Vector3[] vs = m.vertices;
        vs = resizeVertices(vs, 2 * quad.Length);

        for (int i = 0; i < 2 * quad.Length; i += 2)
        {
            vs[vl + i] = quad[i / 2];
            vs[vl + i + 1] = quad[i / 2];
        }

        Vector2[] uvs = m.uv;
        uvs = resizeUVs(uvs, 2 * quad.Length);

        if (quad.Length == 4)
        {
            uvs[vl] = Vector2.zero;
            uvs[vl + 1] = Vector2.zero;
            uvs[vl + 2] = Vector2.right;
            uvs[vl + 3] = Vector2.right;
            uvs[vl + 4] = Vector2.up;
            uvs[vl + 5] = Vector2.up;
            uvs[vl + 6] = Vector2.one;
            uvs[vl + 7] = Vector2.one;
        }
        else
        {
            if (vl % 8 == 0)
            {
                uvs[vl] = Vector2.zero;
                uvs[vl + 1] = Vector2.zero;
                uvs[vl + 2] = Vector2.right;
                uvs[vl + 3] = Vector2.right;

            }
            else
            {
                uvs[vl] = Vector2.up;
                uvs[vl + 1] = Vector2.up;
                uvs[vl + 2] = Vector2.one;
                uvs[vl + 3] = Vector2.one;
            }
        }

        int tl = m.triangles.Length;

        int[] ts = m.triangles;
        ts = resizeTriangles(ts, 12);

        if (quad.Length == 2)
        {
            vl -= 4;
        }

        // front-facing quad
        ts[tl] = vl;
        ts[tl + 1] = vl + 2;
        ts[tl + 2] = vl + 4;

        ts[tl + 3] = vl + 2;
        ts[tl + 4] = vl + 6;
        ts[tl + 5] = vl + 4;

        // back-facing quad
        ts[tl + 6] = vl + 5;
        ts[tl + 7] = vl + 3;
        ts[tl + 8] = vl + 1;

        ts[tl + 9] = vl + 5;
        ts[tl + 10] = vl + 7;
        ts[tl + 11] = vl + 3;

        m.vertices = vs;
        m.uv = uvs;
        m.triangles = ts;
        m.RecalculateBounds();
        m.RecalculateNormals();
    }

    Vector3[] resizeVertices(Vector3[] ovs, int ns)
    {
        Vector3[] nvs = new Vector3[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++)
        {
            nvs[i] = ovs[i];
        }

        return nvs;
    }

    Vector2[] resizeUVs(Vector2[] uvs, int ns)
    {
        Vector2[] nvs = new Vector2[uvs.Length + ns];
        for (int i = 0; i < uvs.Length; i++)
        {
            nvs[i] = uvs[i];
        }

        return nvs;
    }

    int[] resizeTriangles(int[] ovs, int ns)
    {
        int[] nvs = new int[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++)
        {
            nvs[i] = ovs[i];
        }

        return nvs;
    }

    //reconstruct flower part from mesh 
    public static GameObject createFlowerPart(Mesh m, Material mat)
    {
        GameObject flwprt = new GameObject(m.name);
        flwprt.AddComponent<MeshFilter>().mesh = m;
        flwprt.AddComponent<MeshRenderer>();
        MeshCollider mshC = flwprt.AddComponent<MeshCollider>();
        FlowerPart pt = flwprt.AddComponent<FlowerPart>();
        pt.ml = m;
        pt.lmat = new Material(mat);
        Debug.Log(m.colors[0]);
        if (m.colors.Length != 0)
            pt.lmat.color = m.colors[0];

        return flwprt;
    }
}
