using UnityEngine;

namespace Arkanoid
{
	public class MouseController : InputController
	{
		public delegate void OnMouseMoved(Vector2 clickedWorldPoint);
		public static event OnMouseMoved OnMouseMovedEvent;

		private Vector2 lastClickPosition;
		private float lastClickDeltaTime = 0;

		private void Start()
		{
#if !UNITY_EDITOR
			DestroyObject(this);
#endif
		}

		private void Update()
		{
			if (Input.GetMouseButtonUp(0))
			{
				RaiseClickTap(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				if (Time.time < lastClickDeltaTime + DoubleClickCatchTime)
				{
					RaiseDoubleTap();
				}
				lastClickDeltaTime = Time.time;
			}

			if (OnMouseMovedEvent != null)
			{
				OnMouseMovedEvent(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
		}
	}
}