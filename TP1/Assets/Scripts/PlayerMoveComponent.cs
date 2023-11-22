using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMoveComponent : MonoBehaviour
{
    [SerializeField] InputAction MoveAction;
    [SerializeField] InputAction RunAction;
    [SerializeField] InputAction JumpAction;
    [SerializeField] float speed = 3;
    [SerializeField] float rotationSpeed = 5;

    Rigidbody rigidbody;
    Vector3 translation;
    Vector3 direction = Vector3.zero;
    Quaternion toRotation;

    bool isRunning = false;
    bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        MoveAction.Enable();
        RunAction.Enable();
        JumpAction.Enable();

        MoveAction.performed += (InputAction.CallbackContext context) => direction = context.ReadValue<Vector3>();
        MoveAction.canceled += _ => direction = Vector3.zero;

        RunAction.performed += _ => isRunning = true;
        RunAction.canceled += _ => isRunning = false;

        JumpAction.performed += _ => isJumping = true;
        JumpAction.canceled += _ => isJumping = false;
    }

    private void OnDisable()
    {
        RunAction.Disable();
        JumpAction.Disable();

        RunAction.performed -= _ => isRunning = true;
        RunAction.canceled -= _ => isRunning = false;

        JumpAction.performed -= _ => isJumping = true;
        JumpAction.canceled -= _ => isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(direction != Vector3.zero)
        {
            toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            translation = Vector3.forward * Time.deltaTime * speed;

            if (isRunning)
                translation *= 2;

            transform.Translate(translation);
            
        }
        
        if (isJumping)
            Jump();
    }

    private void Jump()
    {
        rigidbody.AddForce(Vector3.up, ForceMode.Impulse);
    }
}
