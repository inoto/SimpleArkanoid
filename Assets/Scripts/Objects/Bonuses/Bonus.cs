using System;
using TMPro;
using UnityEngine;

namespace SimpleArkanoid
{
	public class Bonus : MonoBehaviour
    {
        public static event Action<Bonus> SpawnedEvent;
        public static event Action<Bonus> LostEvent;

        [SerializeField] float Speed = 2f;
        [SerializeField] protected TextMeshPro Text;

        Collider2D collider;
        public Collider2D Collider => collider;

        void Awake()
        {
            collider = GetComponent<Collider2D>();
        }

		public virtual void Caught() { }

        void Start()
        {
            SpawnedEvent?.Invoke(this);
            Invoke("Lost", 8f);
        }

        void Lost()
        {
            LostEvent?.Invoke(this);
            Destroy(gameObject);
        }

        void Update()
        {
            if (Time.timeScale < 1f)
                return;

            transform.Translate(Vector3.down * Speed * Time.deltaTime);
        }
		
		public void Touch()
		{ 
			Caught();
            Destroy(gameObject);
        }
	}
}