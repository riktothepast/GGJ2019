using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField]
    CharacterController characterController;
    [SerializeField]
    float gravity;
    [SerializeField]
    float movementSpeed;
    Vector3 movementVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movementVector.z = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        movementVector.x = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        if (!characterController.isGrounded)
        {
            movementVector.y -= gravity * Time.deltaTime;
        }
        characterController.Move(movementVector);
    }
}
