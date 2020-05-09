using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SimpleArkanoid
{
	public class BonusGiver : MonoBehaviour
	{
        [SerializeField] List<GameObject> BonusPrefabs = new List<GameObject>();
		public float BonusChance = 0.05f;

		public void Activate()
		{
			if (Random.Range(0f, 1f) <= BonusChance)
			{
				if (BonusPrefabs.Count > 0)
				{
					Bonus bonus = Instantiate( // TODO: use SimplePool
						BonusPrefabs[Random.Range(0, BonusPrefabs.Count)],
						transform.position, transform.rotation, transform.parent).GetComponent<Bonus>();
				}
			}
        }
	}
}