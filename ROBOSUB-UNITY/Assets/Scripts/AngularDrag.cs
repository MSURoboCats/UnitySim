using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections.Specialized;
using System.Configuration;
//using System.Diagnostics;



public class AngularDrag : MonoBehaviour
{
    public GameObject meshObject;
    public GameObject parentObject;
    public float DragCooefficient;
    //public Gameobject Object;
    Mesh mesh;
    Rigidbody rb;
    new MeshCollider collider;
    private static Vector3 AngVelocity;
    private static Vector3 AngNormal;
    [HideInInspector()]
    public List<Vector3> meshCenters = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        rb = parentObject.GetComponent<Rigidbody>();
        mesh = meshObject.GetComponent<MeshFilter>().mesh;

        //collider = parentObject.GetComponent<MeshCollider>();
        List<Vector3> meshCenters = new List<Vector3>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector3[] normals = mesh.normals;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        //Debug.DrawLine(Vector3(200, 200, 200), Vector3.zero, Color.green, 2, false);

        for (int i = 0; i < triangles.Length; i += 3)
        {
            //Vector3 SideA = vertices[i] - vertices[i + 1];
            Vector3 V1 = transform.TransformPoint(vertices[triangles[i]]);
            Vector3 V2 = transform.TransformPoint(vertices[triangles[i + 1]]);
            Vector3 V3 = transform.TransformPoint(vertices[triangles[i + 2]]);
            Vector3 SideA = V1 - V2;
            //Vector3 SideB = vertices[i] - vertices[i + 2];
            Vector3 SideB = V1 - V3;
            Vector3 Normal = Vector3.Cross(SideA, SideB);
            Vector3 Center = (V1 + V2 + V3) / 3;
            Vector3 CenterInWorld = Center;//transform.TransformPoint(Center);
                                           //meshCenters.Add(CenterInWorld);
                                           //Debug.Log(rb.angularVelocity);
            Debug.Log(rb.angularVelocity);
            if (rb.angularVelocity != Vector3.zero){
                AngNormal = Quaternion.Euler(rb.angularVelocity * 180 / (float)Math.PI) * Normal;
                //AngCenter = 1;
                //AngVelocity = Normal - AngNormal;
                var NewCenter = Quaternion.Euler(rb.angularVelocity * 180 / (float)Math.PI) * Center;
                var AngVelocity = AngNormal + NewCenter - Normal + Center;
                float ProjectedArea = Vector3.Dot(Normal, AngVelocity);


                if (ProjectedArea < 0)
                {
                    //ProjectedArea = Math.Abs(ProjectedArea);
                    //float DragForce = ProjectedArea * Magnitude(Velocity) * Magnitude(Velocity) * DragCooefficient;
                    float DragForce = ProjectedArea * Vector3.Magnitude(AngVelocity) * Vector3.Magnitude(AngVelocity) * DragCooefficient;

                    Vector3 AppliedDrag = -DragForce * Vector3.Normalize(AngVelocity);//  Vector3.Magnitude(Velocity);//Vector3.Normalize(Velocity) * -1;
                                                                                      //AppliedDrag = Vector3.ClampMagnitude(AppliedDrag, 10000);
                                                                                      //Debug.Log(DragForce);
                    rb.AddForceAtPosition(AppliedDrag, CenterInWorld);
                    //Debug.DrawRay(CenterInWorld, AppliedDrag * 5, Color.yellow, 0.1f, false);

                } 
            }
        }
    }
}