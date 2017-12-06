using UnityEngine;

namespace Arkanoid
{
	public class Bonus : MonoBehaviour
	{
		protected virtual void AddBonus(Object obj)
		{
			// nothing to add
		}
		
		protected virtual void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.CompareTag("Paddle"))
			{
				Paddle paddle = other.GetComponent<Paddle>();
				if (paddle != null)
				{
					AddBonus(paddle);
				}
				Destroy(gameObject);
			}
		}
	}
}