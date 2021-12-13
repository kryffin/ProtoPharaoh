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

		grapplePoint = gps[index].gameObject.transform;

		_joint = transform.parent.gameObject.AddComponent<SpringJoint2D>();
		_joint.autoConfigureConnectedAnchor = false;
		_joint.connectedAnchor = grapplePoint.position;

		_joint.enableCollision = true;

		float distance = Vector2.Distance(transform.position, grapplePoint.position);
		_joint.distance = distance;

		_joint.dampingRatio = 1f;
		_joint.frequency = 0f;

		_lr.positionCount = 2;

		transform.parent.GetComponent<CharacterController2D>().IsGrappled = true;
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

		transform.parent.GetComponent<CharacterController2D>().IsGrappled = false;
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
		Gizmos.color = new Color(0.8941177f, 0.7882353f, 0.4823529f, 0.2f);
		Gizmos.DrawSphere(transform.position, Radius);
    }
}
