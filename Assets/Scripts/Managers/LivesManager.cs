using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
	public class LivesManager : MonoBehaviour
	{
		[SerializeField] private GameObject livesPanel;
		[SerializeField] private GameObject lifePrefab;

		public int LivesCount = 5;
		private Stack<GameObject> lives = new Stack<GameObject>();
		
		public delegate void OnDefeated();
		public static event OnDefeated OnDefeatedEvent;

		private void Awake()
		{
			if (livesPanel == null)
			{
				throw new Exception("Lifes panel not assigned");
			}
			if (lifePrefab == null)
			{
				throw new Exception("Life prefab not assigned");
			}
		}

		private void OnEnable()
		{
			Ball.OnBallLostEvent += LifeLost;
			GameManager.OnGameRestartedEvent += Reset;
		}
		
		private void OnDisable()
		{
			Ball.OnBallLostEvent -= LifeLost;
			GameManager.OnGameRestartedEvent -= Reset;
		}

		private void Start()
		{
			Reset();
		}
		
		private void Reset()
		{
			while (lives.Count < LivesCount)
			{
				LifeAdd();
			}
		}

		private void SetLivesAmount(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				LifeAdd();
			}
		}

		private void LifeAdd()
		{
			lives.Push(Instantiate(lifePrefab, livesPanel.transform));
		}

		private void LifeLost()
		{
			if (lives.Count > 0)
			{
				Destroy(lives.Pop());
			}
			else
			{
				if (OnDefeatedEvent != null)
				{
					OnDefeatedEvent();
				}
			}
		}
	}
}