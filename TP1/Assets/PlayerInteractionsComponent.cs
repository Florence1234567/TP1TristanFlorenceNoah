using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInteractionsComponent : MonoBehaviour
{
    [SerializeField] InputAction intactAction;
    [SerializeField] Object switchObj;
    Transform switchTransform;
    Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        switchTransform = switchObj.GetComponent<Transform>();
        renderer = GetComponent<Renderer>();
        renderer.material.color = Color.cyan;
    }

    private void OnEnable()
    {
        intactAction.Enable();
        intactAction.performed += (InputAction.CallbackContext context) => SwitchPlayerColor();
    }

    private void OnDisable()
    {
        intactAction.Disable();
        intactAction.performed -= (InputAction.CallbackContext context) => SwitchPlayerColor();
    }

    void SwitchPlayerColor()
    {
        if (Vector3.Distance(this.transform.position, switchTransform.position) <= 2)
        {
            if (renderer.material.color == Color.cyan)
                renderer.material.color = Color.red;
            else
                renderer.material.color = Color.cyan;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            SceneManager.LoadScene("Game Over");
        }

    }
}
