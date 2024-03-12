using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Omnix.Monos
{
    /// <summary> Manages keyboard-based navigation and selection of UI elements. </summary>
    public class UiNavigationHandler : MonoBehaviour
    {
        [SerializeField, Tooltip("Which key is used to switch focus")]
        private KeyCode _keyCode;

        [SerializeField, Tooltip("Which key is used to unselect")]
        private KeyCode _unselectKeycode;

        [SerializeField, Tooltip("All items in order.")]
        private Selectable[] _allSelectable;

        private void Reset()
        {
            _keyCode = KeyCode.Tab;
            _unselectKeycode = KeyCode.Escape;
            _allSelectable = GetComponentsInChildren<Selectable>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_keyCode))
            {
                bool isShift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                Select(isShift ? -1 : 1);
            }

            if (Input.GetKeyDown(_unselectKeycode))
            {
                Deselect();
            }
        }
        
        private int GetCurrentSelectedIndex()
        {
            GameObject currentObject = EventSystem.current.currentSelectedGameObject;
            if (currentObject == null) return -1;

            for (var i = 0; i < _allSelectable.Length; i++)
            {
                Selectable selectable = _allSelectable[i];
                if (selectable == null) continue;
                if (selectable.gameObject == currentObject) return i;
            }

            return -1;
        }

        /// <param name="direction"> +1 to select next, -1 to select previous. </param>
        private void Select(int direction)
        {
            int counting = 0;
            int max = _allSelectable.Length;

            int nextIndex = GetCurrentSelectedIndex();
            if (nextIndex <= -1)
            {
                _allSelectable[0].Select();
                return;
            }

            Increment();
            while (CanSelect(_allSelectable[nextIndex]) == false)
            {
                Increment();
                counting++;

                if (counting > max)
                {
                    Debug.LogError("Unable to find selectable element");
                    return;
                }
            }

            _allSelectable[nextIndex].Select();
            return;

            void Increment() => nextIndex = (nextIndex + direction + max) % max;
        }

        private void Deselect()
        {
            int nextIndex = GetCurrentSelectedIndex();
            if (nextIndex <= -1) return;

            EventSystem.current.SetSelectedGameObject(null);
        }
        
        private static bool CanSelect(Selectable selectable)
        {
            return selectable.enabled
                   && selectable.interactable
                   && selectable.targetGraphic.enabled
                   && selectable.gameObject.activeInHierarchy
                   && selectable.targetGraphic.gameObject.activeInHierarchy;
        }
    }
}