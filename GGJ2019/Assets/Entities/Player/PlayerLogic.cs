using UnityEngine;

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
    Vector3 movementVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, 0);
        if (characterController.isGrounded)
        {
            float verticalSpeed = Input.GetAxis("Vertical") > 0 ? Input.GetAxis("Vertical") : 0;
            movementVector = Vector3.forward * verticalSpeed;
            movementVector = transform.TransformDirection(movementVector);
            movementVector *= movementSpeed;
        }
        movementVector.y -= gravity * Time.deltaTime;
        characterController.Move(movementVector * Time.deltaTime);
    }
}
