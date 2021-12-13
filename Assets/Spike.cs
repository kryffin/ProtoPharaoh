using UnityEngine;

public class Spike : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Dashing"))
			GameManager.GetInstance().PlayerHitSpike();
	}
}
