using UnityEngine;

namespace Arkanoid
{
	public class Block : MonoBehaviour
	{
		public delegate void OnBlockDestroyed(Block block);
		public static event OnBlockDestroyed OnBlockDestroyedEvent;
		
		[Header("Block")]
		public bool IsDestroyable = true;
		public bool CanAffectGame = true;
		public int HitsMax = 1;
		private int hitsCurrent;
		public int ScoreRewardFromKill = 10;

		private void Start()
		{
			hitsCurrent = HitsMax;
		}

		protected virtual void Hit()
		{
			hitsCurrent -= 1;
			if (hitsCurrent <= 0)
			{
				Remove();
			}
		}

		protected virtual void Remove()
		{
			if (OnBlockDestroyedEvent != null)
			{
				OnBlockDestroyedEvent(this);
			}
			gameObject.SetActive(false);
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			if(other.gameObject.CompareTag("Ball"))
			{
				Hit();
			}
		}
	}
}