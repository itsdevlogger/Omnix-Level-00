using UnityEngine;

namespace AndroidInputs.Modifiers
{
    public class RoundToInt : IInputModifier
    {
        public float Modify(float input)
        {
            return Mathf.RoundToInt(input);
        }

        public Vector2 Modify(Vector2 input)
        {
            return new Vector2(Mathf.RoundToInt(input.x), Mathf.RoundToInt(input.y));
        }
    }
}