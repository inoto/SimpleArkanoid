using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(CircleCollider2D))]
	public class Ball : MonoBehaviour
	{
		private bool isActive = false;
		public float Speed = 100f;
		
		private Transform trans;
		private Rigidbody2D rigidb;
		private Paddle paddle;
		private Image image;
		
		public delegate void OnBallLost();
		public static event OnBallLost OnBallLostEvent;

		private void Awake()
		{
			trans = GetComponent<Transform>();
			rigidb = GetComponent<Rigidbody2D>();
			image = GetComponent<Image>();
		}

		private void OnEnable()
		{
			InputController.OnDoubleClickTapEvent += Launch;
			LivesManager.OnDefeatedEvent += Remove;
			GameManager.OnGameRestartedEvent += Reset;
		}
		
		private void OnDisable()
		{
			InputController.OnDoubleClickTapEvent -= Launch;
			LivesManager.OnDefeatedEvent -= Remove;
			GameManager.OnGameRestartedEvent -= Reset;
		}

		private void Start()
		{
			Reset();
		}

		private void Update()
		{
			if (trans.position.y < paddle.transform.position.y - 1)
			{
				if (OnBallLostEvent != null)
				{
					OnBallLostEvent();
				}
				Reset();
			}
		}

		public void Init(Paddle pad)
		{
			paddle = pad;
		}

		private void Remove()
		{
			Destroy(gameObject);
		}

		private void Launch()
		{
			if (isActive)
			{
				return;
			}
			if (Time.timeScale < 1)
			{
				return;
			}
			trans.parent = paddle.transform.parent;
			trans.SetSiblingIndex(paddle.transform.GetSiblingIndex()+1);
			rigidb.velocity = new Vector2(Random.Range(-1f,1f), Vector2.up.y) * Speed;
			isActive = true;
		}

		private void Reset()
		{
			isActive = false;
			trans.parent = paddle.transform;
			trans.position = paddle.transform.position + new Vector3(0, 0.5f, 0);
			rigidb.velocity = Vector2.zero;
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			if (isActive && other.gameObject == paddle.gameObject)
			{
				float newX = (trans.position.x - paddle.transform.position.x) / paddle.Width;

				Vector2 direction = new Vector2(newX, 1).normalized;
				rigidb.velocity = direction * Speed;
			}
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (isActive)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawSphere(rigidb.velocity, 0.2f);
				Gizmos.DrawLine(rigidb.velocity, trans.position);
			}
		}
#endif
	}
}