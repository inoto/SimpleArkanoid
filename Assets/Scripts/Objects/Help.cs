using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid
{
	public class Help : MonoBehaviour
	{
		private void OnEnable()
		{
			InputController.OnDoubleClickTapEvent += Hide;
//			GameManager.OnGameRestartedEvent += Show;
		}

		private void OnDisable()
		{
			InputController.OnDoubleClickTapEvent -= Hide;
//			GameManager.OnGameRestartedEvent -= Show;
		}

		private void Hide()
		{
			gameObject.SetActive(false);
		}

//		private void Show()
//		{
//			gameObject.SetActive(true);
//		}
	}
}