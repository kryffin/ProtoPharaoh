using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Hacked version of Brackeys' CharacterController2D
public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float _jumpForce = 400f;                          // Amount of force added when the player jumps.
	[SerializeField] private float _dashForce = 400f;                          // Amount of force added when the player dashes.
	[SerializeField] private float _dashDuration = .5f;                          // Amount of seconds the player dashes.
	[SerializeField] private float _fastFallForce = 1.2f;                       // Amount of force added when the player fast falls.
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool _airControl;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask _whatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform _groundCheck;                           // A position marking where to check if the player is grounded.
	private const float _groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool _grounded;            // Whether or not the player is grounded.
	private Rigidbody2D _rigidbody2D;

	private bool _facingRight = true;  // For determining which way the player is currently facing.
	public bool FacingRight { get => _facingRight; }

	private Vector3 _velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
	public bool IsGrappled;
	public Material PlayerMaterial;
	public Material DashingMaterial;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = _grounded;
		_grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, _whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				_grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool fastFall, bool jump, bool dash)
	{
		//only control the player if grounded or airControl is turned on
		if (_grounded || _airControl)
		{
			// Move the character by finding the target velocity
			Vector2 targetVelocity = new Vector2(move * 10f, _rigidbody2D.velocity.y);

			if (IsGrappled) targetVelocity *= 1.2f;

			// TAKE 2
			if (!_grounded && fastFall)
            {
				targetVelocity += Vector2.down * _fastFallForce;
            }

			// And then smoothing it out and applying it to the character
			_rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !_facingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && _facingRight)
			{
				// ... flip the player.
				Flip();
			}
		}

		// If the player should jump...
		if (_grounded && jump)
		{
			// Add a vertical force to the player.
			_grounded = false;
			_rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
		}

		if (dash)
        {
			transform.Find("GrapplingHook").GetComponent<GrapplingHook>().StopGrapple();

			StartCoroutine(Dash());

			Vector2 dir = _facingRight ? Vector2.right : Vector2.left;
			_rigidbody2D.AddForce(dir * _dashForce, ForceMode2D.Impulse);
			_rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
		}
	}

	// Enables the player to dash through Dashable objects for a given duration
	private IEnumerator Dash()
    {
		gameObject.layer = LayerMask.NameToLayer("Dashing");
		GetComponent<MeshRenderer>().material = DashingMaterial;
		yield return new WaitForSeconds(_dashDuration);
		gameObject.layer = LayerMask.NameToLayer("Player");
		GetComponent<MeshRenderer>().material = PlayerMaterial;
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		_facingRight = !_facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    private void OnDrawGizmos()
    {
		if (_rigidbody2D == null) return;
		Gizmos.DrawRay(new Ray(_rigidbody2D.position, _rigidbody2D.velocity));
    }
}
