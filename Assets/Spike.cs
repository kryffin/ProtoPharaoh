using UnityEngine;

public class Spike : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
			GameManager.GetInstance().PlayerHitSpike();
	}
}
