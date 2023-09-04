using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] InputAction restart;

    private void OnEnable()
    {
        restart.Enable();
        restart.performed += (InputAction.CallbackContext context) => SceneManager.LoadScene("SampleScene");
    }

    private void OnDisable()
    {
        restart.Disable();
        restart.performed -= (InputAction.CallbackContext context) => SceneManager.LoadScene("SampleScene");
    }

}
