using System;
using System.Collections;
using UnityEngine;

namespace SimpleArkanoid
{
	public class BonusAdditionalBall : Bonus
    {
        public static event Action AdditionalBallBonusEvent;

        public override void Caught()
		{
            AdditionalBallBonusEvent?.Invoke();
        }
	}
}