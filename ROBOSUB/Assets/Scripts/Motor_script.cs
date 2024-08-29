using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor_script : MonoBehaviour
{
    
    public Rigidbody sub_rb;
    private Rigidbody rb;
    public Transform fins;
    public float Angle;
    public float max_motor_force = 1;
    public int motor_number = 1;
    private float Power = 0;
    
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        GetComponent<FixedJoint>().connectedBody = sub_rb;

    }

    // Update is called once per frame
    public void SpinMotor(float[] motor){
        if(motor[0] == motor_number && motor[1] != 0){
        Power = motor[1];
        }
        
    }

     void FixedUpdate()
    {
        Vector3 direction = transform.TransformDirection(0,1,0);
        rb.AddForce(direction*Power*max_motor_force);
        fins.Rotate(0, Angle*Power, 0, Space.Self);
    }
}
