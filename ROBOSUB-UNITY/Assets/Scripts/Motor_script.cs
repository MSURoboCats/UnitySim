using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Motor_script : MonoBehaviour
{
    
    public Rigidbody sub_rb;
    private Rigidbody rb;
    public Transform fins;
    public float Angle;
    //public float max_motor_force = 1;
    public int motor_number = 1;
    private float Power = 0;
    private float X;
    
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        GetComponent<FixedJoint>().connectedBody = sub_rb;

    }

    // Update is called once per frame
    public void SpinMotor(float[] motor){
        if(motor[0] == motor_number && motor[1] != 0){
        X = motor[1];
        Power = (float)(0.0*Math.Pow(X,0) + 4.118907246276761*Math.Pow(X,1) + 8.821459800501257*Math.Pow(X,2) + 300.2409405269243*Math.Pow(X,3) + -12.828203731295387*Math.Pow(X,4) + -1835.0570460432466*Math.Pow(X,5) + -8.556796512341581*Math.Pow(X,6) + 6402.263737480421*Math.Pow(X,7) + 132.71963983709227*Math.Pow(X,8) + -12462.130805052753*Math.Pow(X,9) + -283.56130147752305*Math.Pow(X,10) + 13533.217409561114*Math.Pow(X,11) + 248.18171360272333*Math.Pow(X,12) + -7659.510998714309*Math.Pow(X,13) + -79.75205265552889*Math.Pow(X,14) + 1756.4475492089628*Math.Pow(X,15)); 
        //Power = motor[1];
        }
        
    }

     void FixedUpdate()
    {
        Vector3 direction = transform.TransformDirection(0,1,0);
        rb.AddForce(direction*Power);
        fins.Rotate(0, Angle*Power, 0, Space.Self);
    }
}
