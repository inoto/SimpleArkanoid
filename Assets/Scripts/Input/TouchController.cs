using System;
using UnityEngine;

namespace SimpleArkanoid
{
	public class TouchController : InputController
	{
		public static event Action<Vector2> TouchEvent;

        Vector2 lastClickPosition;
        float lastClickDeltaTime = 0;

        void Start()
		{
#if UNITY_EDITOR
			Destroy(this);
#endif
		}

        void Update()
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);

				if (touch.phase == TouchPhase.Ended)
				{
					RaiseClickTap(Camera.main.ScreenToWorldPoint(touch.position));
					if (Time.time < lastClickDeltaTime + DoubleTapCatchTime)
					{
						RaiseDoubleTap();
					}
					lastClickDeltaTime = Time.time;
				}

                TouchEvent?.Invoke(Camera.main.ScreenToWorldPoint(touch.position));
            }
		}
	}
}