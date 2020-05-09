using System;
using UnityEngine;

namespace SimpleArkanoid
{
	public class MouseController : InputController
	{
		public static event Action<Vector2> MouseMovedEvent;

        Vector2 lastClickPosition;
        float lastClickDeltaTime = 0;

        void Start()
		{
#if UNITY_ANDROID || UNITY_IPHONE
			Destroy(this);
#endif
		}

        void Update()
		{
			if (Input.GetMouseButtonUp(0))
			{
				RaiseClickTap(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				if (Time.time < lastClickDeltaTime + DoubleTapCatchTime)
				{
					RaiseDoubleTap();
				}
				lastClickDeltaTime = Time.time;
			}

			if (Input.GetAxis("Mouse X") != 0f)
			{
				MouseMovedEvent?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
		}
	}
}