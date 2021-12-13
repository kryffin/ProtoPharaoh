using UnityEngine;

public class ShieldTrigger : MonoBehaviour
{
	private SandSoldierBehavior _sandSoldierBehavior;

	private void Start()
	{
		_sandSoldierBehavior = transform.parent.GetComponent<SandSoldierBehavior>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		_sandSoldierBehavior.PlatformMode();
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		_sandSoldierBehavior.DefenseMode();
	}
}
