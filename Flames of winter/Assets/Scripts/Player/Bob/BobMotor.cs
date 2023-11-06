using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float gravity = -9.8f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    // Receives inputs from InputManager and apply to the character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(Time.deltaTime * speed * transform.TransformDirection(moveDirection));
        playerVelocity.y += Time.deltaTime * gravity;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -1f;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
