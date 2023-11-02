using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private SolaraInteract solaraInteract;
    private SolaraShoot solaraShoot;
    private BobMotor bobMotor;
    private BobLook bobLook;
    private BobSwap bobSwap;
    private BobPathfind bobPathfind;
    private BobInteract bobInteract;
    private BobShoot bobShoot;
    private PickupController bobPickup;

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
            solaraInteract = solaraObject.GetComponentInChildren<SolaraInteract>();
            solaraShoot = solaraObject.GetComponentInChildren<SolaraShoot>();

            solara.Jump.performed += ctx => solaraMotor.Jump();
            solara.Interact.performed += ctx => solaraInteract.Interact();
            solara.Shoot.performed += ctx => solaraShoot.Shoot();
        }
        if (bobExists)
        {
            bobMotor = bobObject.GetComponent<BobMotor>();
            bobLook = bobObject.GetComponent<BobLook>();
            bobSwap = bobObject.GetComponent<BobSwap>();
            bobPathfind = bobObject.GetComponent<BobPathfind>();
            bobInteract = bobObject.GetComponentInChildren<BobInteract>();
            bobShoot = bobObject.GetComponentInChildren<BobShoot>();
            bobPickup = bobObject.GetComponentInChildren<PickupController>();

            bob.Interact.performed += ctx => bobInteract.Interact();
            bob.Shoot.performed += ctx => bobShoot.Shoot();
            bob.Grab.performed += ctx => bobPickup.Grab();
        }

        if (solaraExists && bobExists)
        {
            global.Swap.performed += ctx => Swap();
            global.Reset.performed += ctx => Reset();
            solara.Point.performed += ctx => Point();
        }

        if (player == Player.Solara)
        {
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
                bobPathfind.StopPathing();
                solaraCamera.SetActive(false);
                bobCamera.SetActive(true);
                solara.Disable();
                bob.Enable();
                player = Player.Bob;
            }
            else if (result == SwapResult.Follow)
            {
                if (bobPathfind.IsFollowing())
                    bobPathfind.StopPathing();
                else
                    bobPathfind.FollowTarget(solaraObject.transform);
            }
        }
        else
        {
            SwapResult result = bobSwap.Swap(solaraObject);
            if (result == SwapResult.Swap)
            {
                bobPickup.Drop();
                bobCamera.SetActive(false);
                solaraCamera.SetActive(true);
                bob.Disable();
                solara.Enable();
                player = Player.Solara;
            }
        }
    }

    private void Reset()
    {
        transitionHandler.TransitionOut(() =>
            SceneManager.LoadScene(SceneManager.GetActiveScene().name)
        );
    }

    private void Point()
    {
        bool success = solaraSwap.Point(out Vector3 location);
        if (success)
            bobPathfind.PathTo(location);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Tell the player to move using the value from our movement action.
        if (player == Player.Solara)
        {
            solaraMotor.ProcessMove(solara.Movement.ReadValue<Vector2>());
            if (bobExists && !bobPathfind.IsPathing())
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
        {
            bobLook.ProcessLook(bob.Look.ReadValue<Vector2>());
            bobPickup.ProcessHold();
        }
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
