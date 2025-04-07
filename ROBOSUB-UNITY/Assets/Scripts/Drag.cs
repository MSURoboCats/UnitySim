using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections.Specialized;
using System.Configuration;
//using System.Diagnostics;



public class Drag : MonoBehaviour
{
    public GameObject meshObject;
    public GameObject rigidBodyObject;
    public float DragCooefficient;
    //public Gameobject Object;
    Mesh mesh;
    Rigidbody rb;
    new MeshCollider collider;
    private static Vector3 PrevLocation;
    private static Vector3 Velocity;
    [HideInInspector()]
    public List<Vector3> meshCenters = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        rb = rigidBodyObject.GetComponent<Rigidbody>();
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

            // Get the 3 points of the triangle as global vector position
            Vector3 V1 = transform.TransformPoint(vertices[triangles[i]]);
            Vector3 V2 = transform.TransformPoint(vertices[triangles[i + 1]]);
            Vector3 V3 = transform.TransformPoint(vertices[triangles[i + 2]]);

            // Calculate the relative vectors V2->V1 and V3->V1, SideA and SideB respectively
            Vector3 SideA = V1 - V2;
            Vector3 SideB = V1 - V3;

            // Cross SideA with SideB
            // The length of this resultant vector is twice the area of the mesh triangle
            // The direction is the normal of the surface of the triangle
            Vector3 Normal = Vector3.Cross(SideA, SideB);

            // Center of the triangle (in global coordinate)
            Vector3 Center = (V1 + V2 + V3) / 3;
            Vector3 CenterInWorld = Center;//transform.TransformPoint(Center);
                                            //meshCenters.Add(CenterInWorld);

            // Get the velocity at the center point (velocity of this mesh trianlge)
            Vector3 Velocity = rb.GetPointVelocity(CenterInWorld);
            //Debug.DrawRay(CenterInWorld, Normal, Color.green, 1, false);
            //if (collider.bounds.Contains(CenterInWorld + Normal))      //Flip normals to point outwards
            //{
            //    Normal *= -1;
            //}
            //Vector3 OffCenter = CenterInWorld + Normal.normalized;
            //Vector3 Normal = Vector3.zero;
            //Normal[0] = SideA[1]*SideB[2] - SideA[2]*SideB[1];
            //Normal[1] = SideA[2] * SideB[0] - SideA[0] * SideB[2];
            //Normal[2] = SideA[0] * SideB[1] - SideA[1] * SideB[0];
            //Normal = normals[i] + normals[i + 1] + normals[i + 2];
            //dTheta = Vector3.Angle(Normal, Velocity);

            //float ProjectedArea = Magnitude(Vector3.Cross(Normal, Velocity));
            //if (i == 0) Debug.Log(CenterInWorld);

            // Calculate the projection of the triangle surface in the direction of velocity
            float ProjectedArea = Vector3.Dot(Normal, Velocity); // Division by Velocity.normalized may be necessary
            //Debug.DrawRay(CenterInWorld, Normal.normalized, Color.green, 1, false);

            // case where the triangle is heading approximately forward, going sideways and backwards won't have drag
            if (ProjectedArea > 0)
            {
                //float DragForce = ProjectedArea * Magnitude(Velocity) * Magnitude(Velocity) * DragCooefficient;

                // Apply the drag formula to get the size of drag
                float DragForce = ProjectedArea * Vector3.Magnitude(Velocity) * Vector3.Magnitude(Velocity) * DragCooefficient;

                // Drag is a opposite force to velocity
                Vector3 AppliedDrag = -DragForce * Vector3.Normalize(Velocity);//  Vector3.Magnitude(Velocity);//Vector3.Normalize(Velocity) * -1;
                                                                                //AppliedDrag = Vector3.ClampMagnitude(AppliedDrag, 10000);
                                                                                //Debug.Log(DragForce);
                
                // Apply the drag force to center of triangle
                rb.AddForceAtPosition(AppliedDrag, CenterInWorld);
                Debug.DrawRay(CenterInWorld, AppliedDrag * 5, Color.green, 0.1f, false);
                
                //Debug.DrawRay(transform.TransformPoint(V1), SideB, Color.green, 1, false);
            }
        }
    }
}
