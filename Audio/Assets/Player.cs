using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform objectivo;
    public float speed = 10.0f;


    //Normalized position
    public float normalizedPosX;
    public float normalizedPosY;
    //Normalized distance
    public float normalizedDistance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float translationY = Input.GetAxis("Vertical") * speed;
        float translationX = Input.GetAxis("Horizontal") * speed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translationX *= Time.deltaTime;
        translationY *= Time.deltaTime;

        // Move translation along the object's x-axis
        transform.Translate(translationX, 0, 0);
        transform.Translate(0,translationY, 0);

        normalizedPosX = (1.0f + 0.5f * transform.position.x / 5.0f)/2.0f;
        normalizedPosY = (1.0f + 0.5f * transform.position.y / 5.0f) / 2.0f;

        normalizedDistance = Vector3.Distance(objectivo.position, transform.position)/20.0f;
    }

}
