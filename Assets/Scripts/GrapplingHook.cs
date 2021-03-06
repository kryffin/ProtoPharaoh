using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingHook : MonoBehaviour
{
    private LineRenderer _lr;
	private bool _started;
	private bool _canceled;
	private bool _upward;
	private bool _downward;
	private SpringJoint2D _joint;
	private Transform _grapplePoint;
	private float _distance;
	[SerializeField] private LayerMask _layerMask;

	public LayerMask whatIsGrappleable;
	public float Radius;
	public float UpDownSpeed = 1f;

	[Header("Input")]
	[Space]

	public InputAction GrappleInput;

	private void Start()
	{
		_lr = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		if (_started)
			StartGrapple();

		if (_canceled)
			StopGrapple();

		if (_joint != null)
        {
			if (_upward)
				GoUp();

			if (_downward)
				GoDown();

		}

		_started = false;
		_canceled = false;
	}

    private void FixedUpdate()
    {
        if (_joint != null)
        {
			// Check if the rope should break
			RaycastHit2D hit = Physics2D.Raycast(transform.position, (_grapplePoint.position - transform.position).normalized, _distance, _layerMask);
			Debug.DrawRay(transform.position, (_grapplePoint.position - transform.position).normalized * _distance, Color.red);
			if (hit.collider != null)
				StopGrapple();
		}
	}

    private void LateUpdate()
	{
		DrawRope();
	}

	public void StartGrapple()
	{
		Collider2D[] gps = Physics2D.OverlapCircleAll(transform.position, Radius, whatIsGrappleable);
		if (gps.Length == 0) return;

		float minDist = float.MaxValue;
		int index = -1;

		int i = 0;
		foreach(Collider2D gp in gps)
        {
			float tmpDist = Vector2.Distance(transform.position, gp.transform.position);
			if (tmpDist < minDist)
            {
				minDist = tmpDist;
				index = i;
            }
			i++;
        }

		_grapplePoint = gps[index].gameObject.transform;

		_joint = transform.parent.gameObject.AddComponent<SpringJoint2D>();
		_joint.autoConfigureConnectedAnchor = false;
		_joint.connectedAnchor = _grapplePoint.position;

		_joint.enableCollision = true;

		float distance = Vector2.Distance(transform.position, _grapplePoint.position);
		_joint.distance = distance;
		_distance = _joint.distance;

		_joint.dampingRatio = 1f;
		_joint.frequency = 0f;

		_lr.positionCount = 2;

		transform.parent.GetComponent<CharacterController2D>().IsGrappled = true;
	}

	private void DrawRope()
	{
		if (!_joint) return;
		_lr.SetPosition(0, transform.position);
		_lr.SetPosition(1, _grapplePoint.position);
	}

	public void GoUp()
    {
		_joint.distance -= UpDownSpeed * Time.deltaTime;
		_distance = _joint.distance;
    }

	public void GoDown()
	{
		_joint.distance += UpDownSpeed * Time.deltaTime;
		_distance = _joint.distance;
	}

	public void StopGrapple()
	{
		_lr.positionCount = 0;
		Destroy(_joint);

		transform.parent.GetComponent<CharacterController2D>().IsGrappled = false;
	}

	private void OnEnable()
	{
		GrappleInput.Enable();
		GrappleInput.started += ctx => _started = true;
		GrappleInput.canceled += ctx => _canceled = true;

		transform.parent.GetComponent<PlayerMovement>().JumpInput.started += ctx => _upward = true;
		transform.parent.GetComponent<PlayerMovement>().JumpInput.canceled += ctx => _upward = false;
		transform.parent.GetComponent<PlayerMovement>().FastFallInput.started += ctx => _downward = true;
		transform.parent.GetComponent<PlayerMovement>().FastFallInput.canceled += ctx => _downward = false;
	}

	private void OnDisable()
	{
		GrappleInput.Disable();
	}

    private void OnDrawGizmosSelected()
    {
		Gizmos.color = new Color(0.8941177f, 0.7882353f, 0.4823529f, 0.2f);
		Gizmos.DrawSphere(transform.position, Radius);
    }
}
