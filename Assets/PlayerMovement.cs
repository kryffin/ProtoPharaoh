using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{

    private CharacterController2D _controller;
    private float horizontalMovement;
    private bool _fastFall;
    private bool _jump;
    private bool _dash;

    public float Speed = 40f;
    public SandSoldierBehavior SandSoldier;

    [Space]
    [Header("Inputs")]

    public InputAction HorizontalInput;
    public InputAction FastFallInput;
    public InputAction JumpInput;
    public InputAction SummonInput;
    public InputAction DashInput;

    private void Start()
	{
        _controller = GetComponent<CharacterController2D>();
	}

	private void Update()
	{
        horizontalMovement = HorizontalInput.ReadValue<float>() * Speed;

        if (FastFallInput.triggered)
            _fastFall = true;

        if (JumpInput.triggered)
            _jump = true;

        if (SummonInput.triggered)
            SandSoldier.Summon();

        if (DashInput.triggered)
            _dash = true;
    }

	private void FixedUpdate()
	{
        _controller.Move(horizontalMovement * Time.fixedDeltaTime, _fastFall, _jump, _dash);

        _fastFall = false;
        _jump = false;
        _dash = false;
    }

	private void OnEnable()
    {
        HorizontalInput.Enable();
        FastFallInput.Enable();
        JumpInput.Enable();
        SummonInput.Enable();
        DashInput.Enable();
    }

    private void OnDisable()
    {
        HorizontalInput.Disable();
        FastFallInput.Disable();
        JumpInput.Disable();
        SummonInput.Disable();
        DashInput.Disable();
    }
}
