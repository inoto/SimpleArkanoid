using System;
using UnityEngine;

namespace Arkanoid
{
	public class TouchController : InputController
	{
		public delegate void OnTouch(Vector2 clickedWorldPoint);
		public static event OnTouch OnTouchEvent;
		
		private Vector2 lastClickPosition;
		private float lastClickDeltaTime = 0;
		
		private void Start()
		{
#if UNITY_EDITOR
			DestroyObject(this);
#endif
		}

		private void Update()
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);

				if (touch.phase == TouchPhase.Ended)
				{
					RaiseClickTap(Camera.main.ScreenToWorldPoint(touch.position));
					if (Time.time < lastClickDeltaTime + DoubleClickCatchTime)
					{
						RaiseDoubleTap();
					}
					lastClickDeltaTime = Time.time;
				}
				
				if (OnTouchEvent != null)
				{
					OnTouchEvent(Camera.main.ScreenToWorldPoint(touch.position));
				}
			}
		}
	}
}