using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arkanoid
{
	public class BlockWithBonus : Block
	{
		[Header("BlockWithBonus")]
		[SerializeField] private List<GameObject> bonusPrefabs = new List<GameObject>();
		public float BonusChance = 0.05f;

		protected override void Remove()
		{
			if (Random.Range(0f, 1f) <= BonusChance)
			{
				if (bonusPrefabs.Count > 0)
				{
					Bonus bonus = Instantiate(
						bonusPrefabs[Random.Range(0,bonusPrefabs.Count)],
						transform.position, transform.rotation, transform.parent).GetComponent<Bonus>();
				}
			}
			base.Remove();
		}
	}
}