using System;
using System.Collections;
using System.Net.Mime;
using UnityEngine;

namespace SimpleArkanoid
{
	public class BonusBallSpeed : Bonus
    {
        public static event Action<float, float> BallSpeedBonusEvent; 

        [SerializeField] float Multiplier = 1.3f;
        [SerializeField] float Duration = 7f;

        public override void Caught()
		{
            BallSpeedBonusEvent?.Invoke(Multiplier, Duration);
        }
	}
}