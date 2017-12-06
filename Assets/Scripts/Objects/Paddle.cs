using System;
using System.Collections;
using UnityEngine;

namespace Arkanoid
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class Paddle : MonoBehaviour
	{
		private Transform trans;
		private RectTransform rectTrans;
		[SerializeField] private RectTransform leftBorderRect;
		[SerializeField] private RectTransform rightBorderRect;
		private float leftLimiter;
		private float rightLimiter;
		
		[SerializeField] private GameObject ballPrefab;

		public float Speed = 20f;
		public float Width;
		private float widthInitial;

		private void Awake()
		{
			trans = GetComponent<Transform>();
			rectTrans = GetComponent<RectTransform>();
			if (ballPrefab == null)
			{
				throw new Exception("Ball prefab not assigned");
			}
			if (leftBorderRect == null)
			{
				throw new Exception("Left border rect not assigned");
			}
			if (rightBorderRect == null)
			{
				throw new Exception("Right border rect not assigned");
			}
		}
		
		private void OnEnable()
		{
#if UNITY_EDITOR
			MouseController.OnMouseMovedEvent += Move;
#else
			TouchController.OnTouchEvent += Move;
#endif
			GameManager.OnGameRestartedEvent += Reset;
		}
		
		private void OnDisable()
		{
#if UNITY_EDITOR
			MouseController.OnMouseMovedEvent -= Move;
#else
			TouchController.OnTouchEvent -= Move;
#endif
			GameManager.OnGameRestartedEvent -= Reset;
		}

		private void Start()
		{
			widthInitial = rectTrans.rect.width;
			Reset();
		}

		private void Reset()
		{
			StopAllCoroutines();
			
			trans.position = new Vector3(0, trans.position.y, 0);
			rectTrans.sizeDelta = new Vector2(widthInitial, rectTrans.rect.height);
			
			Canvas canvas = GetComponentInParent<Canvas>();
			if (canvas != null)
			{
				float scaleX = canvas.GetComponent<RectTransform>().localScale.x;
				Width = rectTrans.rect.width * scaleX;
				leftLimiter = leftBorderRect.position.x + leftBorderRect.rect.width*scaleX;
				rightLimiter = rightBorderRect.position.x - rightBorderRect.rect.width*scaleX;
			}
			
			if (!FindObjectOfType<Ball>())
			{
				CreateBall();
			}
		}

		private void Move(Vector2 clickedWorldPoint)
		{
			if (Time.timeScale < 0.1f)
			{
				return;
			}
			if (clickedWorldPoint.y-2 > 0)
			{
				return;
			}
			float newDirectionX = clickedWorldPoint.x - trans.position.x;
			newDirectionX = Mathf.Clamp(newDirectionX, -1f, 1f);
			Vector2 newPosition = trans.position + new Vector3(newDirectionX * Speed * 0.05f, 0, 0);
			newPosition.x = Mathf.Clamp(newPosition.x, leftLimiter + Width / 2, rightLimiter - Width / 2);
			trans.position = newPosition;
		}

		private void CreateBall()
		{
			Ball ball = Instantiate(ballPrefab, trans).GetComponent<Ball>();
			ball.Init(this);
		}

		public void ChangeWidth(float addition)
		{
			// update transform
			rectTrans.sizeDelta = new Vector2(rectTrans.rect.width + addition, rectTrans.rect.height);
			
			// update collider
			BoxCollider2D boxColleder = GetComponent<BoxCollider2D>();
			boxColleder.size = new Vector2(rectTrans.rect.width, boxColleder.size.y);
			
			// update Width for limiters
			Canvas canvas = GetComponentInParent<Canvas>();
			if (canvas != null)
			{
				float scaleX = canvas.GetComponent<RectTransform>().localScale.x;
				Width = rectTrans.rect.width * scaleX;
			}
		}

		public void ResetWidth(float addition, float duration)
		{
			StartCoroutine(ResetWidthCoroutine(addition, duration));
		}
		
		IEnumerator ResetWidthCoroutine(float addition, float duration)
		{
			yield return new WaitForSeconds(duration);
			ChangeWidth(-addition);
			yield return null;
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, new Vector3(leftLimiter + Width/2, transform.position.y));
			Gizmos.DrawLine(transform.position, new Vector3(rightLimiter - Width/2, transform.position.y));
		}
#endif
	}
}