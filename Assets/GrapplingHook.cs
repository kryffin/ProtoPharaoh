using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingHook : MonoBehaviour
{
    private LineRenderer _lr;
	private bool _started;
	private bool _canceled;
	private SpringJoint2D _joint;

	public LayerMask whatIsGrappleable;
	public Transform GrapplePoint;

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
		if (GrapplePoint == null) return;

		_joint = transform.parent.gameObject.AddComponent<SpringJoint2D>();
		_joint.autoConfigureConnectedAnchor = false;
		_joint.connectedAnchor = GrapplePoint.position;

		_joint.enableCollision = true;

		float distance = Vector2.Distance(transform.position, GrapplePoint.position);
		_joint.distance = distance;

		_joint.dampingRatio = 1f;
		_joint.frequency = 0f;

		_lr.positionCount = 2;
	}

	private void DrawRope()
	{
		if (!_joint) return;
		_lr.SetPosition(0, transform.position);
		_lr.SetPosition(1, GrapplePoint.position);
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
}
