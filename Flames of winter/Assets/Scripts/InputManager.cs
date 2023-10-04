using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.SolaraActions solara;

    private PlayerMotor motor;
    private PlayerLook look;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        solara = playerInput.Solara;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        solara.Jump.performed += ctx => motor.Jump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Tell the player to move using the value from our movement action.
        motor.ProcessMove(solara.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(solara.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        solara.Enable();
    }

    private void OnDisable()
    {
        solara.Disable();
    }
}
