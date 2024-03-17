using UnityEngine.InputSystem;

namespace Omnix.CharaCon.Abilities
{
    [DefaultAbilityIndex(-1)]
    [DefaultInputHandling(InputHandling.Start, InputHandling.Ignore, InputHandling.Stop)]
    public class AbilityCrouch : BaseAbility
    {
        protected override InputAction StartAction => InputMap.BasicAbilities.Crouch;
    }
}