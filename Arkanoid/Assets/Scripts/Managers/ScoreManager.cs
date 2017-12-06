using System;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid
{
	public class ScoreManager : MonoBehaviour
	{
		public int CurrentScore = 0;
		public int HiScore = 0;

		[SerializeField] private Text scoreValueText;
		[SerializeField] private string hiScorePref = "HiScore";
		[SerializeField] private Text menuScoreText;
		[SerializeField] private Text menuHiScoreText;

		private void Awake()
		{
			if (scoreValueText == null)
			{
				throw new Exception("Score value text not assigned");
			}
			if (menuScoreText == null)
			{
				throw new Exception("Menu score text not assigned");
			}
			if (menuHiScoreText == null)
			{
				throw new Exception("Menu hiscore text not assigned");
			}
		}

		private void Start()
		{
			if (PlayerPrefs.HasKey(hiScorePref))
			{
				HiScore = PlayerPrefs.GetInt(hiScorePref);
			}
		}
		
		private void Reset()
		{
			CurrentScore = 0;
			UpdateIngameText();
		}

		private void OnEnable()
		{
			Block.OnBlockDestroyedEvent += EvaluateBlock;
			GameManager.OnGameRestartedEvent += Reset;
			UiManager.OnMenuShownEvent += UpdateMenuText;
		}
		
		private void OnDisable()
		{
			Block.OnBlockDestroyedEvent -= EvaluateBlock;
			GameManager.OnGameRestartedEvent -= Reset;
			UiManager.OnMenuShownEvent -= UpdateMenuText;
		}

		private void OnApplicationQuit()
		{
			if (CurrentScore > HiScore)
			{
				PlayerPrefs.SetInt(hiScorePref, CurrentScore);
				PlayerPrefs.Save();
			}
		}

		private void EvaluateBlock(Block block)
		{
			CurrentScore += block.ScoreRewardFromKill;
			UpdateIngameText();
		}

		private void UpdateIngameText()
		{
			scoreValueText.text = CurrentScore.ToString("D5");
		}
		
		private void UpdateMenuText()
		{
			menuScoreText.text = CurrentScore.ToString("D5");
			menuHiScoreText.text = HiScore.ToString("D5");
		}
	}
}