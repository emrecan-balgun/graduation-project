using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private UnityEngine.CharacterController controller;
    public VariableJoystick variableJoystick;
    public RectTransform handle;
    public float rotateSpeed = 2f, speed=6f;
    public int myScore = 0;
    private Rigidbody rb;
    void Start()
    {
       //controller= GetComponent<CharacterController>();
       rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,variableJoystick.Horizontal*rotateSpeed,0);
        transform.Translate(0,0,speed * variableJoystick.Vertical*Time.deltaTime);


        Vector2 dir = variableJoystick.Direction;
        if (dir.x > 0)
        {
            handle.Rotate(0,0,dir.x*5) ;
        }
        else
        {
            handle.Rotate(0,0,dir.x*5) ;
        }
        
        /* transform.Rotate(0,Input.GetAxis("Horizontal")*rotateSpeed,0);
         float curSpeed = speed * Input.GetAxis("Vertical");
         controller.SimpleMove( curSpeed * transform.TransformDirection(Vector3.forward) *10f );*/

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

        if (collision.gameObject.CompareTag("chest"))
        {
            print("ups");
           
            collision.gameObject.transform.position = ChestManager.SetRandomPosition();
            myScore = myScore + collision.gameObject.GetComponent<Chest>().feature;
        }
    }
}
