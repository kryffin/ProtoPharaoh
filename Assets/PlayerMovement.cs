using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{

    private CharacterController2D _controller;
    private float horizontalMovement;
    private bool _jump;

    public float Speed = 40f;
    public SandSoldierBehavior SandSoldier;

    [Space]
    [Header("Inputs")]

    public InputAction HorizontalInput;
    public InputAction JumpInput;
    public InputAction SummonInput;

    private void Start()
	{
        _controller = GetComponent<CharacterController2D>();
	}

	private void Update()
	{
        horizontalMovement = HorizontalInput.ReadValue<float>() * Speed;

        if (JumpInput.triggered)
            _jump = true;

        if (SummonInput.triggered)
            SandSoldier.Summon();
    }

	private void FixedUpdate()
	{
        _controller.Move(horizontalMovement * Time.fixedDeltaTime, _jump);
        
        _jump = false;
    }

	private void OnEnable()
    {
        HorizontalInput.Enable();
        JumpInput.Enable();
        SummonInput.Enable();
    }

    private void OnDisable()
    {
        HorizontalInput.Disable();
        JumpInput.Disable();
        SummonInput.Disable();
    }
}
