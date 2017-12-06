using UnityEngine;

namespace Arkanoid
{
	public abstract class InputController : MonoBehaviour
	{
		public delegate void OnClickTap(Vector2 clickedWorldPoint);
		public static event OnClickTap OnClickTapEvent;
		public delegate void OnDoubleClickTap();
		public static event OnDoubleClickTap OnDoubleClickTapEvent;
		
		public float DoubleClickCatchTime = 0.3f;
		
		protected void RaiseClickTap(Vector2 position)
		{
			if (OnClickTapEvent != null)
			{
				OnClickTapEvent(position);
			}
		}
		
		protected void RaiseDoubleTap()
		{
			if (OnDoubleClickTapEvent != null)
			{
				OnDoubleClickTapEvent();
			}
		}
	}
}