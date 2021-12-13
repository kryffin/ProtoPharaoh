using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingHook : MonoBehaviour
{
    private LineRenderer _lr;
	private bool _started;
	private bool _canceled;
	private SpringJoint2D _joint;
	private Transform grapplePoint;

	public LayerMask whatIsGrappleable;
	public float Radius;

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

		_started = false;
		_canceled = false;
	}

	private void LateUpdate()
	{
		DrawRope();
	}

	public void StartGrapple()
	{
		var gp = Physics2D.OverlapCircle(transform.position, Radius, whatIsGrappleable);
		if (gp == null) return;

		grapplePoint = gp.gameObject.transform;

		_joint = transform.parent.gameObject.AddComponent<SpringJoint2D>();
		_joint.autoConfigureConnectedAnchor = false;
		_joint.connectedAnchor = grapplePoint.position;

		_joint.enableCollision = true;

		float distance = Vector2.Distance(transform.position, grapplePoint.position);
		_joint.distance = distance;

		_joint.dampingRatio = 1f;
		_joint.frequency = 0f;

		_lr.positionCount = 2;
	}

	private void DrawRope()
	{
		if (!_joint) return;
		_lr.SetPosition(0, transform.position);
		_lr.SetPosition(1, grapplePoint.position);
	}

	public void StopGrapple()
	{
		_lr.positionCount = 0;
		Destroy(_joint);
	}

	private void OnEnable()
	{
		GrappleInput.Enable();
		GrappleInput.started += ctx => _started = true;
		GrappleInput.canceled += ctx => _canceled = true;
	}

	private void OnDisable()
	{
		GrappleInput.Disable();
	}

    private void OnDrawGizmosSelected()
    {
		Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
		Gizmos.DrawSphere(transform.position, Radius);
    }
}
