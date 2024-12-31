using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f, shiftSpeed = 10f, stamina = 5f, currentSpeed;
    public float jumpForce = 5f;

    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        currentSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = Physics.CheckSphere(transform.position, 2f, LayerMask.GetMask("Ground"));

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (stamina > 0)
            {
                stamina -= Time.deltaTime;
                currentSpeed = shiftSpeed;
            }
            else
            {
                currentSpeed = moveSpeed;
            }
        }
        else
        {
            stamina += Time.deltaTime;
        }
        stamina = Mathf.Clamp(stamina, 0, shiftSpeed);

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        _controller.Move(move * currentSpeed * Time.deltaTime);

        // Zıplama
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }

        // Yerçekimi uygulama
        _velocity.y += Physics.gravity.y * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
