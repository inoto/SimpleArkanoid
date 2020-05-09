using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SimpleArkanoid
{
	public class Block : MonoBehaviour
    {
        public static event Action<Block> BlockDestroyedEvent;

		public int HitsMax = 1;
        [SerializeField] bool RandomHitsCount = true;

        int hitsCurrent;
        BonusGiver bonusGiver;
        BoxCollider2D collider;
        public BoxCollider2D Collider => collider;

        void Awake()
        {
            collider = GetComponent<BoxCollider2D>();
            bonusGiver = GetComponent<BonusGiver>();
        }

		void Start()
		{
			hitsCurrent = HitsMax;
            if (RandomHitsCount && HitsMax > 1)
            {
                hitsCurrent = Random.Range(1, HitsMax);
            }
		}

        protected virtual void Remove()
		{
            BlockDestroyedEvent?.Invoke(this);
            bonusGiver?.Activate();
            Destroy(gameObject);
        }

        public bool Touch()
        {
            hitsCurrent -= 1;
            if (hitsCurrent <= 0)
            {
                Remove();
                return true;
            }
            return false;
        }
	}
}