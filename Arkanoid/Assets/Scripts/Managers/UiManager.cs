using System;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid
{
	public class UiManager : MonoBehaviour
	{
		[SerializeField] private GameObject menuPanel;
		[SerializeField] private GameObject winResultHeader;
		[SerializeField] private GameObject loseResultHeader;
		
		public delegate void OnMenuShown();
		public static event OnMenuShown OnMenuShownEvent;
		
		private void Awake()
		{
			if (menuPanel == null)
			{
				throw new Exception("Menu panel not assigned");
			}
			if (winResultHeader == null)
			{
				throw new Exception("Result header not assigned");
			}
			if (loseResultHeader == null)
			{
				throw new Exception("Result header not assigned");
			}
			
		}

		private void OnEnable()
		{
			GameManager.OnGamePausedEvent += ShowMenu;
			GameManager.OnGameContinuedEvent += HideMenu;
			GameManager.OnGameRestartedEvent += HideMenu;
			LivesManager.OnDefeatedEvent += LoseMenu;
			BlocksManager.OnWonEvent += WinMenu;
		}
		
		private void OnDisable()
		{
			GameManager.OnGamePausedEvent -= ShowMenu;
			GameManager.OnGameContinuedEvent -= HideMenu;
			GameManager.OnGameRestartedEvent -= HideMenu;
			LivesManager.OnDefeatedEvent -= LoseMenu;
			BlocksManager.OnWonEvent -= WinMenu;
		}

		private void RaiseOnMenuShownEvent()
		{
			if (OnMenuShownEvent != null)
			{
				OnMenuShownEvent();
			}
		}

		private void ShowMenu()
		{
			menuPanel.SetActive(true);
			RaiseOnMenuShownEvent();
		}

		private void WinMenu()
		{
			ShowMenu();
			winResultHeader.SetActive(true);
		}

		private void LoseMenu()
		{
			ShowMenu();
			loseResultHeader.SetActive(true);
		}
		
		private void HideMenu()
		{
			menuPanel.SetActive(false);
			winResultHeader.SetActive(false);
			loseResultHeader.SetActive(false);
		}
	}
}