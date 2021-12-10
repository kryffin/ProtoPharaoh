using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{

    private CharacterController2D _controller;
    private float horizontalMovement;
    private bool _jump;
    private bool _grapple;

    public float Speed = 40f;
    public Transform GrapplePoint;

    [Space]
    [Header("Inputs")]

    public InputAction HorizontalInput;
    public InputAction JumpInput;
    public InputAction SummonInput;
    public InputAction GrappleInput;

    private void Start()
	{
        _controller = GetComponent<CharacterController2D>();
	}

	private void Update()
	{
        horizontalMovement = HorizontalInput.ReadValue<float>() * Speed;

        if (JumpInput.triggered)
            _jump = true;

        if (GrappleInput.triggered)
            _grapple = true;
    }

	private void FixedUpdate()
	{
        _controller.Move(horizontalMovement * Time.fixedDeltaTime, _grapple, GrapplePoint.position, _jump);
        
        _jump = false;
        _grapple = false;
    }

	private void OnEnable()
    {
        HorizontalInput.Enable();
        JumpInput.Enable();
        SummonInput.Enable();
        GrappleInput.Enable();
    }

    private void OnDisable()
    {
        HorizontalInput.Disable();
        JumpInput.Disable();
        SummonInput.Disable();
        GrappleInput.Disable();
    }

	private void OnDrawGizmos()
	{
        Gizmos.DrawRay(new Ray(transform.position, GrapplePoint.position - transform.position)); ;
	}

}
