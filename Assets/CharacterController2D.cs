using UnityEngine;
using UnityEngine.Events;

// Hacked version of Brackeys' CharacterController2D
public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float _jumpForce = 400f;                          // Amount of force added when the player jumps.
	[SerializeField] private float _dashForce = 400f;                          // Amount of force added when the player dashes.
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool _airControl;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask _whatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform _groundCheck;                           // A position marking where to check if the player is grounded.
	private const float _groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool _grounded;            // Whether or not the player is grounded.
	private Rigidbody2D _rigidbody2D;
	private bool _facingRight = true;  // For determining which way the player is currently facing.
	private Vector3 _velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

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

		if (dash && move != 0f)
        {
			Vector2 dir = _facingRight ? Vector2.right : Vector2.left;
			_rigidbody2D.AddForce(dir * _dashForce, ForceMode2D.Impulse);
		}

		if (!_grounded && fastFall)
        {

        }
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
}
