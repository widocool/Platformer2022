namespace GSGD2.Gameplay
{
	using GSGD2.Player;
	using System.Collections;
	using UnityEngine;
	using UnityEngine.InputSystem.Switch;

	/// <summary>
	/// Type of <see cref="PickupCommand"/> that can modify player abilities in runtime. In can enable or disable Jump and Dash as well as add or remove on allowed force.
	/// </summary>
	[CreateAssetMenu(menuName = "GameSup/PlayerAbilityModifierCommand", fileName = "PlayerAbilityModifierCommand")]
	public class PlayerAbilityModifierCommand : PickupCommand
	{
		private enum Ability
		{
			Jump,
			Dash
		}

		// TODO AL : add modifiers : SetAllowedForce, 
		private enum Modifier
		{
			Enable,
			Disable,
			AddOneAllowedForce,
			RemoveOneAllowedForce,
		}

		[SerializeField]
		private Ability _ability = 0;

		[SerializeField]
		private Modifier _modifier = 0;

		protected override bool ApplyPickup(ICommandSender from)
		{
			var player = LevelReferences.Instance.Player;

			switch (_ability)
			{
				case Ability.Jump:

					if (_modifier == Modifier.Enable || _modifier == Modifier.Disable)
					{
						player.EnableJump(_modifier == Modifier.Enable ? true : false);
					}
					else if (_modifier == Modifier.AddOneAllowedForce || _modifier == Modifier.RemoveOneAllowedForce)
					{
						player.AddMaximumAllowedForceToJump(_modifier == Modifier.AddOneAllowedForce ? 1 : -1);
					}
					break;
				case Ability.Dash:
				{
					if (_modifier == Modifier.Enable || _modifier == Modifier.Disable)
					{
						player.EnableDash(_modifier == Modifier.Enable ? true : false);
					}
					else if (_modifier == Modifier.AddOneAllowedForce || _modifier == Modifier.RemoveOneAllowedForce)
					{
						player.AddMaximumAllowedForceToDash(_modifier == Modifier.AddOneAllowedForce ? 1 : -1);
					}
				}
					break;
				default:
					break;
			}

			return true;
		}


	}
}