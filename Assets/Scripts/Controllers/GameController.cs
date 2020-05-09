using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SimpleArkanoid
{
	public class GameController : MonoBehaviour
    {
        public static event Action WinEvent;

        [SerializeField] GameObject TextBackObj = null;
        [SerializeField] TextMeshProUGUI TextOverlay = null;

        int blocksCount = 0;

        void Start()
        {
            Time.timeScale = 1f;

            blocksCount = FindObjectsOfType<Block>().Length;
            Debug.Log($"blocksCount {blocksCount}");
            Debug.Log($"scene {SceneManager.GetActiveScene().buildIndex}");
            Debug.Log($"scene count {SceneManager.sceneCountInBuildSettings}");

            LivesController.DefeatedEvent += Defeated;
            Block.BlockDestroyedEvent += BlockDestroyed;
        }

        void OnDestroy()
        {
            LivesController.DefeatedEvent -= Defeated;
            Block.BlockDestroyedEvent -= BlockDestroyed;
        }

        void BlockDestroyed(Block block)
        {
            blocksCount -= 1;
            if (blocksCount <= 0)
            {
                Win();
            }
        }

        void Defeated()
        {
            Time.timeScale = 0.99f;
            TextBackObj.SetActive(true);
            TextOverlay.text = "game over";
            Invoke("ReloadScene", 2f);
        }

        void ReloadScene()
        {
            StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));
        }

        void Win()
        {
            Time.timeScale = 0.99f;
            TextBackObj.SetActive(true);
            if (SceneManager.GetActiveScene().buildIndex <
                SceneManager.sceneCountInBuildSettings - 1)
            {
                TextOverlay.text = "win!";
                Invoke("NextScene", 2f);
            }
            else
            {
                TextOverlay.text = "The end. hope you enjoyed :)";
            }
        }

        void NextScene()
        {
            StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1));
        }

        IEnumerator LoadSceneAsync(int sceneIndex)
        {
            SimplePool.Reset();
            var operation = SceneManager.LoadSceneAsync(sceneIndex);
            while (!operation.isDone)
            {
                yield return null;
            }
		}
    }
}
