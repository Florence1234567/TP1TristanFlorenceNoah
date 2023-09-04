using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovmentComponent : MonoBehaviour
{
    [SerializeField] InputAction moveAction;
    [SerializeField] float speed = 5;

    Vector3 direction = Vector3.zero;

    void Awake()
    {

    }

    private void OnEnable()
    {
        moveAction.Enable();
        moveAction.performed += (InputAction.CallbackContext context) => direction = context.ReadValue<Vector3>();
        moveAction.canceled += _ => direction = Vector3.zero;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        moveAction.performed -= (InputAction.CallbackContext context) => direction = context.ReadValue<Vector3>();
        moveAction.canceled -= _ => direction = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
