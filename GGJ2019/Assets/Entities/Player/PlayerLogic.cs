﻿using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField]
    CharacterController characterController;
    [SerializeField]
    GameObject playerChildModel;
    [SerializeField]
    float gravity;
    [SerializeField]
    float movementSpeed;
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    Animator animator;
    Vector3 movementVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float verticalSpeed = Input.GetAxis("Vertical") > 0 ? Input.GetAxis("Vertical") : 0;
        float horizontalSpeed = Input.GetAxis("Horizontal");

        if (horizontalSpeed != 0 )
        {
            transform.Rotate(0, horizontalSpeed * rotationSpeed * Time.deltaTime, 0);
            if (verticalSpeed == 0)
            {
                animator.Play("Rotate");
            }
        }

        if (characterController.isGrounded)
        {
            if (verticalSpeed > 0)
            {
                animator.Play("Walk");
            }
            movementVector = Vector3.forward * verticalSpeed;
            movementVector = transform.TransformDirection(movementVector);
            movementVector *= movementSpeed;
        }
        movementVector.y -= gravity * Time.deltaTime;
        characterController.Move(movementVector * Time.deltaTime);
    }
}
