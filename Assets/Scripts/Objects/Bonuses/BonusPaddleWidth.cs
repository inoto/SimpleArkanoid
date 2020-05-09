using System;
using System.Collections;
using UnityEngine;

namespace SimpleArkanoid
{
	public class BonusPaddleWidth : Bonus
    {
        public static event Action<float, float> PaddleWidthBonusEvent; 

        [SerializeField] float Multiplier = 1.1f;
        [SerializeField] float Duration = 7f;

        public override void Caught()
		{
            PaddleWidthBonusEvent?.Invoke(Multiplier, Duration);
        }
	}
}