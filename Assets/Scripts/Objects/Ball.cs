using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SimpleArkanoid
{
    public class Ball : MonoBehaviour
	{
        public static event Action<Ball> LostEvent;
        public static event Action<Ball> LaunchedEvent;

        [SerializeField] float Speed = 1f;
        
		Transform _transform;

        CircleCollider2D collider;
        public CircleCollider2D Collider => collider;

		Paddle paddle;
        bool isActive = false;
		Vector2 direction = Vector2.zero;
        Vector2 originalScale;

        void Awake()
		{
			_transform = GetComponent<Transform>();
            collider = GetComponent<CircleCollider2D>();

            originalScale = _transform.localScale;
        }

        void OnEnable()
        {
            isActive = false;
            direction = Vector2.zero;
            paddle = null;

            InputController.DoubleClickTapEvent += Launch;
            BonusBallSpeed.BallSpeedBonusEvent += ModifySpeed;
        }

        void UnSubscribe()
        {
            InputController.DoubleClickTapEvent -= Launch;
            BonusBallSpeed.BallSpeedBonusEvent -= ModifySpeed;
        }

        void OnDisable()
        {
            UnSubscribe();
        }

        void OnDestroy()
        {
            UnSubscribe();
        }

        void Update()
		{
            if (Time.timeScale < 1f)
                return;

            transform.Translate(direction * Speed * Time.deltaTime);

			if (isActive && _transform.position.y < paddle.transform.position.y - 1)
            {
                BallLost();
            }
		}

        void BallLost()
        {
            LostEvent?.Invoke(this);
            SimplePool.Despawn(gameObject);
        }

		public void Init(Paddle paddle)
		{
			this.paddle = paddle;

            isActive = false;
		}

        void Launch()
        {
            if (isActive)
                return;

            _transform.SetParent(paddle.transform.parent);

            direction = new Vector2(Random.Range(-1f, 1f), Vector2.up.y);
			isActive = true;
            LaunchedEvent?.Invoke(this);
        }

        public void BouncePaddle()
        {
            float newX = (_transform.position.x - paddle.transform.position.x) /
                         paddle.transform.localScale.x;
            direction = new Vector2(newX, 1).normalized;
		}

        public void Bounce(Vector2 contactPoint, Vector2 norm)
        {
            direction = Vector2.Reflect(direction.normalized, norm.normalized);
        }

        public void ModifySpeed(float multiplier, float duration)
        {
            Speed *= multiplier;
            StartCoroutine(ResetSpeed(multiplier, duration));
        }

        IEnumerator ResetSpeed(float multiplier, float duration)
        {
            yield return new WaitForSeconds(duration);
            if (multiplier > 0)
                Speed /= multiplier;
        }
    }
}