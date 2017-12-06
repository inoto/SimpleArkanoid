using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid
{
	public class GameManager : MonoBehaviour
	{
		private static GameManager _instance;
		public static GameManager Instance { get { return _instance; } }

		public bool IsPaused = false;
		[SerializeField] private GameObject menuPanel;
		
		
		
		public delegate void OnGamePaused();
		public static event OnGamePaused OnGamePausedEvent;
		public delegate void OnGameContinued();
		public static event OnGameContinued OnGameContinuedEvent;
		public delegate void OnGameRestarted();
		public static event OnGameRestarted OnGameRestartedEvent;

		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(this.gameObject);
			}
			else
			{
				_instance = this;
			}
			if (menuPanel == null)
			{
				throw new Exception("Menu panel not assigned");
			}
		}

		private void OnEnable()
		{
//			InputController.OnClickTapEvent += Hit;
			LivesManager.OnDefeatedEvent += PauseGame;
			BlocksManager.OnWonEvent += PauseGame;
		}
		
		private void OnDisable()
		{
//			InputController.OnClickTapEvent -= Hit;
			LivesManager.OnDefeatedEvent -= PauseGame;
			BlocksManager.OnWonEvent -= PauseGame;
		}

//		private void Hit(Vector2 worldPoint)
//		{
//			RaycastHit2D hit = Physics2D.Raycast(worldPoint, transform.forward);
//			if (hit)
//			{
//				Debug.Log("hit");
//				Button button = hit.transform.GetComponent<Button>();
//				if (button != null)
////				if (hit.transform.CompareTag("Button"))
//				{
//					Debug.Log("button clicked");
////					if (OnButtonClickedEvent != null)
////					{
////						OnButtonClickedEvent(hit.transform.gameObject);
////					}
//					button.onClick.Invoke();
//				}
//			}
//			else
//			{
//				Debug.Log("no hit");
//			}
//		}

		public void PauseGame()
		{
			if (IsPaused)
			{
				StartCoroutine(UnpauseCoroutine());
				return;
			}
			IsPaused = true;
			Time.timeScale = 0;
			if (OnGamePausedEvent != null)
			{
				OnGamePausedEvent();
			}
		}

		IEnumerator UnpauseCoroutine()
		{
			IsPaused = false;
			if (OnGameContinuedEvent != null)
			{
				OnGameContinuedEvent();
			}
			Time.timeScale = 0.2f;
			while (Time.timeScale < 1)
			{
				Time.timeScale += 0.1f;
				yield return new WaitForSeconds(0.1f);
			}
			yield return null;
		}

		public void RestartGame()
		{
			IsPaused = false;
			StopAllCoroutines();
			Time.timeScale = 1;
			if (OnGameRestartedEvent != null)
			{
				OnGameRestartedEvent();
			}
		}

		public void CloseApplication()
		{
			Application.Quit();
		}
	}
}
