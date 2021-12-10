using UnityEngine;

public class Spike : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision.gameObject.name + " touched spikes");
	}
}
