using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Interfaces;
using ROS_img = RosMessageTypes.Sensor.ImageMsg;




public class Camera_message : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "video_frames";

    // The game object 
    private Rigidbody rb;
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 1.0f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;
    public int width = 0;
    public int height = 0;
    public Camera cam;
    public ROS_img Frame;
    
    
    private Texture2D renderedTexture;
    private Texture2D renderedTextureFlipped;
    private RenderTexture screenTexture;
    private Rect screenView;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<RosMessageTypes.Sensor.ImageMsg>(topicName);
        rb = GetComponent<Rigidbody>();
        
        Frame.encoding = "rgb8";
        Frame.width = (uint)width;
        Frame.height = (uint)height;
        Frame.step = (uint)width*3;
        Frame.is_bigendian = 0;
        screenTexture = new RenderTexture(width, height, 16,RenderTextureFormat.BGRA32);
        cam.targetTexture = screenTexture;
        screenView = new Rect(0, 0, width, height);

        renderedTexture = new Texture2D(width, height,TextureFormat.RGB24, false);
        renderedTextureFlipped = new Texture2D(width, height,TextureFormat.RGB24, false);
        //renderedTexture.transform.x = -1;

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
            for (int i = 0; i < width; i++){
                for (int j = 0; j < height; j++){
                    renderedTextureFlipped.SetPixel(width - i - 1, j, renderedTexture.GetPixel(i, j));
                }
            }
            renderedTextureFlipped.Apply();

            byte[] byteArray = renderedTextureFlipped.GetRawTextureData();
            Array.Reverse(byteArray);
            Frame.data = byteArray;

            ros.Publish(topicName, Frame);
            timeElapsed = 0;
        }
    }
}
