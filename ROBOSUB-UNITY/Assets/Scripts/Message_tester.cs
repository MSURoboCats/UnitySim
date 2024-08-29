using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Interfaces;
using ROS_img = RosMessageTypes.Sensor.ImageMsg;




public class Message_tester : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "image_data";

    // The game object 
    private Rigidbody rb;
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 1.0f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;
    private Vector3 prev_lin_vel;
    private Vector3 lin_acc;
    public ROS_img Frame;
    public Camera cam;
    public int width = 0;
    public int height = 0;
    private Texture2D renderedTexture;
    private RenderTexture screenTexture;
    private Rect screenView;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<RosMessageTypes.Sensor.ImageMsg>(topicName);
        rb = GetComponent<Rigidbody>();
        prev_lin_vel = new Vector3(0,0,0);
        
        Frame.encoding = "rgb8";
        Frame.width = (uint)width;
        Frame.height = (uint)height;
        Frame.step = (uint)width*3;
        screenTexture = new RenderTexture(width, height, 16);
        cam.targetTexture = screenTexture;
        screenView = new Rect(0, 0, width, height);

        renderedTexture = new Texture2D(width, height,TextureFormat.RGB24, false);

    }

    void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            


            // Finally send the message to server_endpoint.py running in ROS
            RenderTexture.active = screenTexture;
            
            cam.Render();
            renderedTexture.ReadPixels(screenView, 0, 0,false);
            RenderTexture.active = null;

            byte[] byteArray = renderedTexture.GetRawTextureData();

            Frame.data = byteArray;

            ros.Publish(topicName, Frame);
            timeElapsed = 0;
        }
    }
}
