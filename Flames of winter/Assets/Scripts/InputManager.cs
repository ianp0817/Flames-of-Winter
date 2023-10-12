using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.SolaraActions solara;
    private PlayerInput.BobActions bob;
    private PlayerInput.GlobalActions global;
    [SerializeField] private GameObject solaraObject;
    [SerializeField] private GameObject solaraCamera;
    [SerializeField] private GameObject bobObject;
    [SerializeField] private GameObject bobCamera;
    private bool solaraExists;
    private bool bobExists;

    [SerializeField] private TransitionHandler transitionHandler;

    private enum Player { Solara, Bob }
    [SerializeField] private Player player = Player.Solara;

    private SolaraMotor solaraMotor;
    private SolaraLook solaraLook;
    private SolaraSwap solaraSwap;
    private BobMotor bobMotor;
    private BobLook bobLook;
    private BobSwap bobSwap;

    void Awake()
    {
        playerInput = new PlayerInput();
        solara = playerInput.Solara;
        bob = playerInput.Bob;
        global = playerInput.Global;

        solaraExists = solaraObject != null;
        bobExists = bobObject != null;

        if (solaraExists)
        {
            solaraMotor = solaraObject.GetComponent<SolaraMotor>();
            solaraLook = solaraObject.GetComponent<SolaraLook>();
            solaraSwap = solaraObject.GetComponent<SolaraSwap>();
        }
        if (bobExists)
        {
            bobMotor = bobObject.GetComponent<BobMotor>();
            bobLook = bobObject.GetComponent<BobLook>();
            bobSwap = bobObject.GetComponent<BobSwap>();
        }

        if (solaraExists && bobExists)
            global.Swap.performed += ctx => Swap();

        if (player == Player.Solara)
        {
            solara.Jump.performed += ctx => solaraMotor.Jump();
            if (bobExists)
                bobCamera.SetActive(false);
        }
        else
        {
            if (solaraExists)
                solaraCamera.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        transitionHandler.TransitionIn(() =>
        {
            global.Enable();
            if (player == Player.Solara)
                solara.Enable();
            else
                bob.Enable();
        });
    }

    private void Swap()
    {
        if (!bobExists || !solaraExists)
            return;

        if (player == Player.Solara)
        {
            SwapResult result = solaraSwap.Swap(bobObject);
            if (result == SwapResult.Swap)
            {
                solaraCamera.SetActive(false);
                bobCamera.SetActive(true);
                solara.Disable();
                bob.Enable();
                player = Player.Bob;
            }
        }
        else
        {
            SwapResult result = bobSwap.Swap(solaraObject);
            if (result == SwapResult.Swap)
            {
                bobCamera.SetActive(false);
                solaraCamera.SetActive(true);
                bob.Disable();
                solara.Enable();
                player = Player.Solara;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Tell the player to move using the value from our movement action.
        if (player == Player.Solara)
        {
            solaraMotor.ProcessMove(solara.Movement.ReadValue<Vector2>());
            if (bobExists)
                bobMotor.ProcessMove(Vector2.zero);
        }
        else
        {
            bobMotor.ProcessMove(bob.Movement.ReadValue<Vector2>());
            if (solaraExists)
                solaraMotor.ProcessMove(Vector2.zero);
        }
    }

    private void LateUpdate()
    {
        if (player == Player.Solara)
            solaraLook.ProcessLook(solara.Look.ReadValue<Vector2>());
        else
            bobLook.ProcessLook(bob.Look.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        global.Disable();
        if (player == Player.Solara)
            solara.Disable();
        else
            bob.Disable();
    }
}
