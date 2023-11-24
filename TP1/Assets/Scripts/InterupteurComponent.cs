using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InterupteurComponent : MonoBehaviour
{
    [SerializeField] InputAction interact;
    [SerializeField] GameObject player;
    Animator playerAnimator;
    float interactRange = 2;
    bool inRange;

    private void Awake()
    {
        playerAnimator = player.GetComponent<Animator>();
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
        if(inRange)
            playerAnimator.SetBool("dance", true);
    }
}
