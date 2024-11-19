using System.Collections.Generic;
using UnityEngine;

namespace AndroidInputs.Modifiers
{
    public interface IInputModifier
    {
        public float Modify(float input);
        public Vector2 Modify(Vector2 input);
    }
    
    public static class ModifiersCollectionExtensions
    {
        public static float Modify(this IEnumerable<IInputModifier> modifiers, float input)
        {
            if (modifiers == null) return input;

            var value = input;
            foreach (IInputModifier modifier in modifiers)
            {
                value = modifier.Modify(value);
            }
            
            return value;
        }

        public static Vector2 Modify(this IEnumerable<IInputModifier> modifiers, Vector2 input)
        {
            if (modifiers == null) return input;
            
            var value = input;
            foreach (IInputModifier modifier in modifiers)
            {
                value = modifier.Modify(value);
            }
            return value;
        }
    }
}