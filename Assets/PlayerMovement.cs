using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController _cc;
    private Transform _sandSoldierPivot;
    private SandSoldierBehavior _sandSoldierBehavior;
    private float _mvt;
    private float yVelocity;

    public GameObject SandSoldier;
    public InputAction MvtInput;
    public InputAction JumpInput;
    public InputAction SummonInput;
    public float Speed;
    public float JumpForce;
    public float GravityForce;

    private void Start()
    {
        _cc = GetComponent<CharacterController>();
        _sandSoldierPivot = transform.Find("SandSoldier Pivot");
        _sandSoldierBehavior = SandSoldier.GetComponent<SandSoldierBehavior>();
    }

    private void Update()
    {
        _mvt = 0f;

        _mvt += MvtInput.ReadValue<float>() * Speed;

        if (JumpInput.triggered && _cc.isGrounded)
            yVelocity += JumpForce;

        if (SummonInput.triggered)
        {
            if (!_sandSoldierBehavior.Summoned)
                _sandSoldierBehavior.Summon(_sandSoldierPivot.position);
            else
                _sandSoldierBehavior.SwitchMode();
        }

        //gravity
        if (!_cc.isGrounded)
            yVelocity -= GravityForce * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        _cc.Move(((Vector2.right * _mvt) + (Vector2.up * yVelocity)) * Time.fixedDeltaTime);
    }

    private void OnEnable()
    {
        MvtInput.Enable();
        JumpInput.Enable();
        SummonInput.Enable();
    }

    private void OnDisable()
    {
        MvtInput.Disable();
        JumpInput.Disable();
        SummonInput.Disable();
    }

}
