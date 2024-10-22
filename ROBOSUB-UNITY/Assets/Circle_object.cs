using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle_object : MonoBehaviour
{
    public GameObject Target;
    public bool rotating = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(rotating){
            transform.LookAt(Target.transform);
            transform.Rotate(90* new Vector3(0,1,0));
            transform.RotateAround(Target.transform.position, Vector3.up, 10 * Time.deltaTime);
        }
    }
}
