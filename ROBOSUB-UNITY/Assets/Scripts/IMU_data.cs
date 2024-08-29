using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Interfaces;
using Control_ = RosMessageTypes.Interfaces.ControlDataMsg;

public class IMU_data : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "control_data";

    // The game object 
    private Rigidbody rb;
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 0.5f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;
    private Vector3 prev_lin_vel;
    private Vector3 lin_acc;
    public Control_ data;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<ControlDataMsg>(topicName);
        rb = GetComponent<Rigidbody>();
        prev_lin_vel = new Vector3(0,0,0);

    }

    void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            data.imu_data.orientation.x = rb.rotation.x;
            data.imu_data.orientation.y = rb.rotation.y;
            data.imu_data.orientation.z = rb.rotation.z;
            data.imu_data.orientation.w = rb.rotation.w;

            data.imu_data.angular_velocity.x = rb.angularVelocity.x;
            data.imu_data.angular_velocity.y = rb.angularVelocity.y;
            data.imu_data.angular_velocity.z = rb.angularVelocity.z;

            lin_acc = (rb.velocity-prev_lin_vel)/timeElapsed;
            data.imu_data.linear_acceleration.x = lin_acc.x;
            data.imu_data.linear_acceleration.y = lin_acc.y;
            data.imu_data.linear_acceleration.z = lin_acc.z;
            data.depth = -rb.position.y;
            //Debug.Log(ang_acc);
            prev_lin_vel = rb.velocity;


            // Finally send the message to server_endpoint.py running in ROS
            ros.Publish(topicName, data);
            //Debug.Log(-rb.position.y);

            timeElapsed = 0;
        }
    }
}
