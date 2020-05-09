using UnityEngine;
using UnityEngine.UI;

namespace SimpleArkanoid
{
	public class PreLaunchText : MonoBehaviour
	{
        void Start()
		{
			InputController.DoubleClickTapEvent += Hide;
        }

        void OnDestroy()
		{
			InputController.DoubleClickTapEvent -= Hide;
        }

        void Hide()
		{
			Destroy(gameObject);
		}
    }
}