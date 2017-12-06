using System.Collections;
using UnityEngine;

namespace Arkanoid
{
	public class BonusPaddleWidth : Bonus
	{
		[Header("BonusPaddleWidth")]
		public int AdditionalWidth = 10;
		public float Duration = 7f;
		private Paddle paddle;
		
		protected override void AddBonus(Object obj)
		{
			paddle = (Paddle) obj;
			paddle.ChangeWidth(AdditionalWidth);
			paddle.ResetWidth(AdditionalWidth, Duration);
		}
	}
}