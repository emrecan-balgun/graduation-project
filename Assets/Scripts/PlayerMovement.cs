using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private UnityEngine.CharacterController controller;

    public float rotateSpeed = 2f, speed=6f;
    public int myScore = 0;
    void Start()
    {
       controller= GetComponent<CharacterController>();
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,Input.GetAxis("Horizontal")*rotateSpeed,0);
        float curSpeed = speed * Input.GetAxis("Vertical");
        controller.SimpleMove( curSpeed * transform.TransformDirection(Vector3.forward) *10f );
        print("MyScore:"+myScore);
    }
    
    void OnCollisionEnter(Collision collision)
    {
      
        if (collision.gameObject.CompareTag("gem"))
        {
            Destroy(collision.gameObject);
            myScore++;
        }
        if (collision.gameObject.CompareTag("mutant"))
        {
            myScore--;
        }
    }
}
