using System.Collections;
using UnityEngine;

namespace Omnix.SceneManagement
{
    public class WaitAndLoadScene : MonoBehaviour
    {
        [SerializeField] private float _waitForSec;
        [SerializeField] private SceneProperties _sceneProperties;

        private IEnumerator Start()
        {
            if (_waitForSec > 0) yield return new WaitForSeconds(_waitForSec);
            _sceneProperties.Load();
        }
    }
}