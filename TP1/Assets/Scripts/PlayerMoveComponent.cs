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
    [SerializeField] float speed = 4;
    [SerializeField] float rotationSpeed = 5;

    Animator animator;

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
        animator = GetComponent<Animator>();
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

        JumpAction.performed += _ => Jump();
       // JumpAction.canceled += _ => isJumping = false;
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
        if (direction != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("dance", false);
            toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            translation = Vector3.forward * Time.deltaTime * speed;

            if (isRunning)
            {
                translation *= 2;
                animator.SetBool("isRunning", true);
            }
            else
                animator.SetBool("isRunning", false);



            transform.Translate(translation);

        }
        else
            animator.SetBool("isMoving", false);

        
        if (isJumping)
        {

            //Jump();
        }
            
    }

    public bool IsGrounded()
    {
        // Cast a ray downward to check for ground
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, .2f))
        {
            return true;
        }

        return false;
    }

    private void Jump()
    {
        if (!isJumping && IsGrounded())
        {
            animator.SetTrigger("jump");
            rigidbody.AddRelativeForce(new Vector3(0, 5, 0), ForceMode.Impulse);
        }
        //rigidbody.AddForce(Vector3.up * Time.deltaTime, ForceMode.Impulse);
    }
}
