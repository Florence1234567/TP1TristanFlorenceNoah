using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InterupteurPorte : MonoBehaviour
{
    [SerializeField] InputAction interact;
    [SerializeField] GameObject player;
    [SerializeField] GameObject door;
    Animator doorAnimator;
    float interactRange = 2;
    bool inRange;
    bool isOpen = false;

    private void Awake()
    {
        doorAnimator = door.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        interact.Enable();

        interact.performed += _ => SwitchUsed();
    }

    private void OnDisable()
    {
        interact.Disable();

        interact.performed -= _ => SwitchUsed();
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= interactRange)
            inRange = true;
        else
            inRange = false;
    }

    void SwitchUsed()
    {
        if (inRange && !isOpen)
        {
            doorAnimator.SetBool("DoorOpen", true);
            isOpen = true;
        }
    }
}
