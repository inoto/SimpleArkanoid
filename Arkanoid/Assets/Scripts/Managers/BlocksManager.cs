using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arkanoid
{
	public class BlocksManager : MonoBehaviour
	{
		private List<Block> blocks;
		public int AliveBlocksAmount;
		
		public delegate void OnWon();
		public static event OnWon OnWonEvent;

		private void OnEnable()
		{
			Block.OnBlockDestroyedEvent += MinusBlock;
			GameManager.OnGameRestartedEvent += Reset;
		}
		
		private void OnDisable()
		{
			Block.OnBlockDestroyedEvent -= MinusBlock;
			GameManager.OnGameRestartedEvent -= Reset;
		}

		private void Start()
		{
			blocks = new List<Block>(FindObjectsOfType<Block>());
			AliveBlocksAmount = blocks.Where(b => b.CanAffectGame).Count();
		}

		private void Reset()
		{
			foreach (var block in blocks)
			{
				block.gameObject.SetActive(true);
			}
			AliveBlocksAmount = blocks.Where(b => b.CanAffectGame).Count();
		}

		private void MinusBlock(Block block)
		{
			AliveBlocksAmount -= 1;
			if (AliveBlocksAmount <= 0)
			{
				if (OnWonEvent != null)
				{
					OnWonEvent();
				}
			}
		}
	}
}