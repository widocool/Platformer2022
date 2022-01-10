namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Utilities;
	using GSGD2.Player;

	/// <summary>
	/// Component that can be hold by a <see cref="ItemHolder"/>.
	/// It can be highlighted by it, grabbed, and gravity is applied if the ground check fail.
	/// It can also be reset at its starting world position by <see cref="ItemKillzoneReceiver"/> component or if the player enter a <see cref="Killzone"/> while holding the item.
	/// </summary>
	[SelectionBase]
	public class Item : MonoBehaviour
	{
		public enum State
		{
			Idle,
			Highlighted,
			Held,
		}

		[SerializeField]
		private MaterialDescription _swappingMaterialReference = null;

		[SerializeField]
		private Vector3 _heldPosition = Vector3.forward;

		[SerializeField]
		private Vector3 _gizmosSize = Vector3.one;

		[SerializeField] // layer must be set to Default only to ignore player
		private Raycaster _groundRaycaster = null;

		[SerializeField]
		private float _gravityScale = 9;

		[SerializeField]
		private Timer _momentumTimer = null;

		[SerializeField]
		private AnimationCurve _momentumCurve = null;

		[Tooltip("Will works only with movement state : StartJump, Dashing, WallJump, WallGrab")]
		[SerializeField]
		private CubeController.State _preventMovementStatesWhenHoldingAnItem = 0;

		private MeshRenderer[] _meshRenderers = null;

		private MaterialDescription _swappingMaterialInstance = null;

		private Vector3 _cachedStartWorldPosition;

		private State _currentState = 0;

		private bool _isFalling = false;

		private Vector3 _positionAtRelease;

		public State CurrentState => _currentState;
		public bool IsFalling => _isFalling;
		public CubeController.State PreventMovementStatesWhenHoldingAnItem => _preventMovementStatesWhenHoldingAnItem;

		public struct ItemEventArgs
		{
			public State previousState;
			public State currentState;
			public bool isSwapped;

			public ItemEventArgs(State previousState, State currentState, bool isMaterialSwapped)
			{
				this.previousState = previousState;
				this.currentState = currentState;
				this.isSwapped = isMaterialSwapped;
			}
		}

		public delegate void ItemEvent(Item sender, ItemEventArgs args);
		public event ItemEvent WorldPositionReset = null;
		public event ItemEvent MaterialChanged = null;
		public event ItemEvent StateChanged = null;

		public void ResetWorldPosition(bool changeStateToIdle = true)
		{
			transform.position = _cachedStartWorldPosition;
			WorldPositionReset?.Invoke(this, GetEventArgs(_currentState, _currentState));
			if (changeStateToIdle)
			{
				ChangeState(State.Idle);
			}
		}

		[ContextMenu("Swap Material")]
		public void Swap()
		{
			_swappingMaterialInstance.SwapMaterial(_meshRenderers);
		}

		public void ChangeMaterial(bool toAlternativeMaterial)
		{
			_swappingMaterialInstance.ChangeMaterial(_meshRenderers, toAlternativeMaterial);
			MaterialChanged?.Invoke(this, GetEventArgs(_currentState, _currentState));
		}

		public void ChangeState(State newState)
		{
			State previousState = _currentState;

			// State Out Transition
			switch (previousState)
			{
				case State.Idle:
					break;
				case State.Highlighted:
					break;
				case State.Held:
				{
					_positionAtRelease = transform.position;
				}
				break;
				default:
					break;
			}

			_currentState = newState;
			StateChanged?.Invoke(this, GetEventArgs(previousState, _currentState));

			// State In Transition
			switch (newState)
			{
				case State.Idle:
				{
					if (previousState == State.Held)
					{
						if (TryRepositionToGround() == false)
						{
							transform.position = NullifyX(transform.position);
							_isFalling = true;
							_momentumTimer.Start();
						}
					}
				}
				break;
				case State.Highlighted:
					break;
				case State.Held:
				{

				}
				break;
				default:
					break;
			}
		}

		public void SetParent(Transform parent)
		{
			transform.SetParent(parent);
			transform.localPosition = _heldPosition;
		}

		private ItemEventArgs GetEventArgs(State previousState, State currentState)
		{
			return new ItemEventArgs(previousState, currentState, _swappingMaterialInstance.IsSwapped);
		}

		private void Awake()
		{
			_cachedStartWorldPosition = transform.position;
		}

		private void OnEnable()
		{
			_swappingMaterialInstance = _swappingMaterialReference.CreateNew();
			_meshRenderers = GetComponentsInChildren<MeshRenderer>();
			_swappingMaterialInstance.SetCachedOriginalMaterial(_meshRenderers);
		}

		private void Update()
		{
			if (_isFalling == true)
			{
				if (TryRepositionToGround() == true)
				{
					_isFalling = false;
					_momentumTimer.ForceFinishState();
				}
				else
				{
					bool applyMomentum = _momentumTimer.Update();
					if (applyMomentum == false)
					{
						var progress = _momentumTimer.Progress;
						Vector3 addedPosition = Vector3.up * _momentumCurve.Evaluate(progress);
						transform.position = Vector3.Lerp(transform.position, _positionAtRelease + addedPosition, progress);
					}
					else
					{
						var position = transform.position;
						position.y += Physics.gravity.y * _gravityScale * Time.deltaTime;
						transform.position = position;
					}
				}
			}

			// Crappy, something is setting the rotation when ItemHolder take and item when the player do a uturn, but what ?
			// This hide the problem
			transform.rotation = Quaternion.identity;
		}

		private Vector3 NullifyX(Vector3 vector)
		{
			vector.x = 0;
			return vector;
		}


		private bool TryRepositionToGround()
		{
			bool result = _groundRaycaster.Raycast(out RaycastHit hit);
			if (result == true)
			{
				transform.position = NullifyX(hit.point);
			}
			return result;
		}

		private void OnDrawGizmos()
		{
			// Item holder is at 1, so +vector3 up
			Gizmos.DrawWireCube(transform.position + _heldPosition, _gizmosSize);
			_groundRaycaster.DrawGizmos();
		}
	}
}