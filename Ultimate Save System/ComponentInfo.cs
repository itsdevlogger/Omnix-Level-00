using System;
using UnityEngine;

namespace UltimateSaveSystem
{
    [Serializable]
    public class ComponentInfo
    {
        [SerializeField] private int _savedIndex;
        [SerializeField] private Component _component;

        public int SavedIndex => _savedIndex;
        public Component Component => _component;

        public ComponentInfo(int savedIndex, Component component)
        {
            _savedIndex = savedIndex;
            _component = component;
        }
    }
}