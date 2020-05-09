using System;
using UnityEngine;

namespace SimpleArkanoid
{
	public abstract class InputController : MonoBehaviour
	{
		public static event Action<Vector2> ClickTapEvent;
		public static event Action DoubleClickTapEvent;
		
		public float DoubleTapCatchTime = 0.3f;
		
		protected void RaiseClickTap(Vector2 position)
        {
            ClickTapEvent?.Invoke(position);
        }
		
		protected void RaiseDoubleTap()
        {
            DoubleClickTapEvent?.Invoke();
        }
	}
}