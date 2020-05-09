using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleArkanoid
{
    public class Paddle : MonoBehaviour
	{
        [SerializeField] GameObject BallPrefab;
        [SerializeField] float Speed = 20f;
        [SerializeField] SpriteRenderer Field;

        Transform _transform;

		BoxCollider2D collider;
        public BoxCollider2D Collider => collider;

		float widthInitial;
        float leftLimiter;
        float rightLimiter;
        List<Ball> balls = new List<Ball>();
        float ballPositionInitialY;

		void Awake()
		{
			_transform = GetComponent<Transform>();
            collider = GetComponent<BoxCollider2D>();
        }
		
		void Start()
		{
            widthInitial = _transform.localScale.x;
            SimplePool.Preload(BallPrefab, transform, 3);

            float halfFieldX = (Field.size.x * Field.transform.localScale.x) / 2;
            float halfWidth = _transform.localScale.x / 2;
            leftLimiter = -halfFieldX + halfWidth;
            rightLimiter = halfFieldX - halfWidth;

            Ball ball = FindObjectOfType<Ball>();
            if (ball != null)
            {
                ballPositionInitialY = ball.transform.position.y;
                ball.Init(this);
                balls.Add(ball);
            }
            else
            {
                ballPositionInitialY = _transform.position.y + 1f;
                SpawnBall();
            }

            MouseController.MouseMovedEvent += Move;
            TouchController.TouchEvent += Move;
            BonusPaddleWidth.PaddleWidthBonusEvent += ModifyWidth;
            BonusAdditionalBall.AdditionalBallBonusEvent += SpawnBall;
            Ball.LostEvent += Lost;
            // LivesController.DefeatedEvent += ResetAll;
        }
		
		void OnDestroy()
		{
            MouseController.MouseMovedEvent -= Move;
            TouchController.TouchEvent -= Move;
            BonusPaddleWidth.PaddleWidthBonusEvent -= ModifyWidth;
            BonusAdditionalBall.AdditionalBallBonusEvent -= SpawnBall;
            Ball.LostEvent -= Lost;
            // LivesController.DefeatedEvent -= ResetAll;
		}

        void ResetAll()
		{
            _transform.position = new Vector3(0, _transform.position.y, 0);
			balls.Clear();

            SpawnBall();
        }

		void Move(Vector2 clickedWorldPoint)
		{
			if (Time.timeScale < 1f)
				return;

            float newDirectionX = clickedWorldPoint.x - _transform.position.x;
			newDirectionX = Mathf.Clamp(newDirectionX, -1f, 1f);

			Vector2 newPosition = // cache all of these variables
                _transform.position + new Vector3(newDirectionX * Speed * 0.05f, 0, 0);
            newPosition.x = Mathf.Clamp(
                newPosition.x, leftLimiter, rightLimiter);
			_transform.position = newPosition;
		}

        void Lost(Ball ball)
        {
            balls.Remove(ball);
            if (balls.Count == 0)
				ResetAll();
        }

		void SpawnBall()
		{
			GameObject go = SimplePool.Spawn(BallPrefab,
                new Vector3(_transform.position.x, ballPositionInitialY), Quaternion.identity);
            // go.transform.parent = transform;
			go.transform.SetParent(transform);
            Ball ball = go.GetComponent<Ball>();
			ball.Init(this);
            balls.Add(ball);
        }

        public void ModifyWidth(float multiplier, float duration)
        {
            ChangeWidth(multiplier);
            StartCoroutine(ResetWidthCoroutine(multiplier, duration));
        }

		void ChangeWidth(float multiplier, bool increase = true)
		{
			// update transform
            float multipliedScaleX;
			if (increase)
                multipliedScaleX = _transform.localScale.x * multiplier;
			else
                multipliedScaleX = _transform.localScale.x / multiplier;
			_transform.localScale =
                new Vector2(multipliedScaleX, _transform.localScale.y);
			
			// update collider
			BoxCollider2D boxColleder = (BoxCollider2D)collider;
			boxColleder.size = _transform.localScale;

            // update Width for limiters
			Canvas canvas = GetComponentInParent<Canvas>();
			if (canvas != null)
			{
                float halfFieldX = (Field.size.x * Field.transform.localScale.x) / 2;
                float halfWidth = _transform.localScale.x / 2;
                leftLimiter = -halfFieldX + halfWidth;
                rightLimiter = halfFieldX - halfWidth;
			}
		}

        IEnumerator ResetWidthCoroutine(float multiplier, float duration)
		{
			yield return new WaitForSeconds(duration);
			ChangeWidth(multiplier, false);
			yield return null;
		}

        void OnDrawGizmos()
        {
            Transform trans = transform;

			Gizmos.color = Color.magenta;
            float halfColliderX = trans.localScale.x / 2;
            float halfColliderY = trans.localScale.y / 2;
			Gizmos.DrawLine(
                new Vector3(trans.position.x - halfColliderX, trans.position.y),
                new Vector3(trans.position.x + halfColliderX, trans.position.y));

			Gizmos.color = Color.red;
            float halfFieldX = (Field.size.x * Field.transform.localScale.x) / 2;
            float halfWidth = trans.localScale.x / 2;
			leftLimiter = -halfFieldX + halfWidth;
            rightLimiter = halfFieldX - halfWidth;
            Gizmos.DrawLine(trans.position,
                new Vector3(leftLimiter, trans.position.y));
			Gizmos.DrawLine(trans.position,
                new Vector3(rightLimiter, trans.position.y));
        }
    }
}