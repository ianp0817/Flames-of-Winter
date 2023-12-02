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
    private PlayerInput.BothActions both;
    [SerializeField] private GameObject solaraObject;
    [SerializeField] private GameObject solaraCamera;
    [SerializeField] private GameObject solaraModel;
    [SerializeField] private GameObject bobObject;
    [SerializeField] private GameObject bobCamera;
    [SerializeField] private GameObject bobModel;
    private bool solaraExists;
    private bool bobExists;

    [SerializeField] private TransitionHandler transitionHandler;

    private enum Player { Solara, Bob }
    [SerializeField] private Player player = Player.Solara;
    [SerializeField] GameObject swapTooltip = null;

    private SolaraMotor solaraMotor;
    private SolaraLook solaraLook;
    private SolaraSwap solaraSwap;
    private SolaraInteract solaraInteract;
    private SolaraShoot solaraShoot;
    private Outline solaraOutline;
    private BobMotor bobMotor;
    private BobLook bobLook;
    private BobSwap bobSwap;
    private BobPathfind bobPathfind;
    private BobInteract bobInteract;
    private BobShoot bobShoot;
    private PickupController bobPickup;
    private Outline bobOutline;

    void Awake()
    {
        playerInput = new PlayerInput();
        solara = playerInput.Solara;
        bob = playerInput.Bob;
        both = playerInput.Both;

        solaraExists = solaraObject != null;
        bobExists = bobObject != null;

        both.Reset.performed += ctx => Reset();

        if (solaraExists)
        {
            solaraMotor = solaraObject.GetComponent<SolaraMotor>();
            solaraLook = solaraObject.GetComponent<SolaraLook>();
            solaraSwap = solaraObject.GetComponent<SolaraSwap>();
            solaraInteract = solaraObject.GetComponentInChildren<SolaraInteract>();
            solaraShoot = solaraObject.GetComponentInChildren<SolaraShoot>();
            solaraOutline = solaraObject.GetComponent<Outline>();

            solara.Jump.performed += ctx => solaraMotor.Jump();
            solara.Interact.performed += ctx => solaraInteract.Interact();
            solara.Interact.performed += ctx => solaraShoot.Pickup();
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
            bobOutline = bobObject.GetComponent<Outline>();

            bob.Interact.performed += ctx => bobInteract.Interact();
            bob.Interact.performed += ctx => bobShoot.Pickup();
            bob.Shoot.performed += ctx => bobShoot.Shoot();
            bob.Grab.performed += ctx => bobPickup.Grab();
        }

        if (solaraExists && bobExists)
        {
            both.Swap.performed += ctx => Swap();
            solara.Point.performed += ctx => Point();
        }

        if (player == Player.Solara)
        {
            if (bobExists)
                bobCamera.SetActive(false);
            if (solaraExists)
                solaraOutline.enabled = false;
            if (solaraModel)
                solaraModel.SetActive(false);
        }
        else
        {
            if (solaraExists)
                solaraCamera.SetActive(false);
            if (bobExists)
                bobOutline.enabled = false;
            if (bobModel)
                bobModel.SetActive(false);
        }

        if (swapTooltip)
            both.Swap.performed += Unlock;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        try
        {
            Persistent.LvlIdx = SceneManager.GetActiveScene().buildIndex;
        }
        catch (System.NullReferenceException) { }
        transitionHandler.TransitionIn(() =>
        {
            if (!swapTooltip)
            {
                if (solaraExists || bobExists)
                    Enable();
                else
                    Disable();
            }
            else
            {
                both.Enable();
            }
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
                bobObject.transform.rotation = Quaternion.LookRotation(new Vector3(
                    solaraObject.transform.position.x - bobObject.transform.position.x, 0,
                    solaraObject.transform.position.z - bobObject.transform.position.z).normalized, Vector3.up);
                bobCamera.SetActive(true);
                solara.Disable();
                bob.Enable();
                bobOutline.enabled = false;
                solaraOutline.enabled = true;
                if (bobModel)
                    bobModel.SetActive(false);
                if (solaraModel)
                    solaraModel.SetActive(true);
                player = Player.Bob;
            }
            /*else if (result == SwapResult.Follow)
            {
                if (bobPathfind.IsFollowing())
                    bobPathfind.StopPathing();
                else
                    bobPathfind.FollowTarget(solaraObject.transform);
            }*/
        }
        else
        {
            SwapResult result = bobSwap.Swap(solaraObject);
            if (result == SwapResult.Swap)
            {
                bobPickup.Drop();
                bobCamera.SetActive(false);
                solaraObject.transform.rotation = Quaternion.LookRotation(new Vector3(
                    bobObject.transform.position.x - solaraObject.transform.position.x, 0,
                    bobObject.transform.position.z - solaraObject.transform.position.z).normalized, Vector3.up);
                solaraCamera.SetActive(true);
                bob.Disable();
                solara.Enable();
                solaraOutline.enabled = false;
                bobOutline.enabled = true;
                if (solaraModel)
                    solaraModel.SetActive(false);
                if (bobModel)
                    bobModel.SetActive(true);
                player = Player.Solara;
            }
        }
    }

    public void Reset()
    {
        Disable();
        transitionHandler.TransitionOut(() =>
            SceneManager.LoadScene(SceneManager.GetActiveScene().name)
        );
    }

    private void Point()
    {
        bool success = solaraSwap.Point(out Vector3 ping);
        if (success)
            bobPathfind.PathTo(ping);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (solaraExists || bobExists)
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
    }

    private void LateUpdate()
    {
        if (solaraExists || bobExists)
        {
            if (player == Player.Solara)
                solaraLook.ProcessLook(solara.Look.ReadValue<Vector2>());
            else
            {
                bobLook.ProcessLook(bob.Look.ReadValue<Vector2>());
                bobPickup.ProcessHold();
            }
        }
    }

    public void Enable()
    {
        both.Enable();
        if (player == Player.Solara)
            solara.Enable();
        else
            bob.Enable();
    }

    public void Disable()
    {
        both.Disable();
        if (player == Player.Solara)
            solara.Disable();
        else
            bob.Disable();
    }

    private void Unlock(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            both.Swap.performed -= Unlock;
            both.Disable();
            Enable();
            Destroy(swapTooltip);
        }
    }
}
