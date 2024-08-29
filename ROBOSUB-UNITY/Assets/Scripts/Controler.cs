using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Robotics.ROSTCPConnector;
using Motor_power = RosMessageTypes.Interfaces.MotorPowersMsg;


public class Controler : MonoBehaviour
{
    private float[] Motor_var = new float[2]; // 0 index = motor number; 1 index = power
    private double[] powers = new double[8];
    private float[] prev_powers = new float[8];
    private Motor_script Script; 
    // Start is called before the first frame update
        void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<Motor_power>("motor_powers", PowerChange);
        Script = gameObject.GetComponentInChildren<Motor_script>();


    }

    void PowerChange(Motor_power Powers_)
    {
        powers[0] = Powers_.motor1;
        powers[1] = Powers_.motor2;
        powers[2] = Powers_.motor3;
        powers[3] = Powers_.motor4;
        powers[4] = Powers_.motor5;
        powers[5] = Powers_.motor6;
        powers[6] = Powers_.motor7;
        powers[7] = Powers_.motor8;

        for (int i = 0; i < 8; ++i){
            Motor_var[0] = i+1;
            Motor_var[1] = (float)powers[i];
            gameObject.BroadcastMessage("SpinMotor", Motor_var);
        }
    }

    // Update is called once per frame

}
