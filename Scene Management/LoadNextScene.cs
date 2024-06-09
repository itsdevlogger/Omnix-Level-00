using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Omnix.SceneManagement
{
    public class LoadNextScene : MonoBehaviour
    {
        [SerializeField] private float _waitForSec;

        private IEnumerator Start()
        {
            if (_waitForSec > 0) yield return new WaitForSeconds(_waitForSec);
            int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex) + 1;
            nextSceneIndex.GetId().Load();
        }
    }
}