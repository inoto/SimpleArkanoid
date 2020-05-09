using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleArkanoid
{
	public class LivesController : MonoBehaviour
    {
        public static event Action DefeatedEvent;

		[SerializeField] GameObject LivesPanel = null;

        Stack<GameObject> lives = new Stack<GameObject>();

        void Start()
        {
            for (int i = 0; i < LivesPanel.transform.childCount; i++)
            {
				lives.Push(LivesPanel.transform.GetChild(i).gameObject);
            }

            Ball.LostEvent += LifeLost;
		}

        void OnDestroy()
        {
            Ball.LostEvent -= LifeLost;
        }

        void LifeLost(Ball ball)
		{
			if (lives.Count > 0)
			{
				Destroy(lives.Pop());
			}
			else
			{
                DefeatedEvent?.Invoke();
			}
		}
	}
}