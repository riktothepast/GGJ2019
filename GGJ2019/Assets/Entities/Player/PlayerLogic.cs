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
        movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        movementVector *= movementSpeed * Time.deltaTime;

        if (movementVector != Vector3.zero)
        {
            playerChildModel.transform.rotation = Quaternion.Lerp(playerChildModel.transform.rotation, Quaternion.LookRotation(movementVector), rotationSpeed * Time.deltaTime);
        }

        if (!characterController.isGrounded)
        {
            movementVector.y -= gravity * Time.deltaTime;
        }

        characterController.Move(movementVector);
    }
}
