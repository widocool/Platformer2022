namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Serialization;
	using GSGD2.Utilities;

#if UNITY_EDITOR
	using UnityEditor;
#endif //UNITY_EDITOR

	/// <summary>
	/// Component class that slide its GameObject on from <see cref="_startPosition"/> to <see cref="_endPosition"/> at a given <see cref="_speed"/>.
	/// The movement can ben eased by a <see cref="_movementCurve"/>.
	/// Its has various settings Play, PlayReverse, PingPong settings and can be interacted with a <see cref="MovingPlatformInteractor"/> and <see cref="MovingPlatformInteractorActivator"/>
	/// </summary>
	[SelectionBase]
	public class MovingPlatform : MonoBehaviour
	{
		[SerializeField]
		private bool _enabledAtStart = false;

		[SerializeField]
		private Vector3 _startPosition = Vector3.zero;

		[SerializeField]
		private Vector3 _endPosition = Vector3.forward;

		[SerializeField]
		private float _speed = 1f;

		[SerializeField]
		private AnimationCurve _movementCurve = null;

		[SerializeField]
		private bool _stopWhenAtEndOfPath = false; // rename stop when at end of path

		[SerializeField]
		private Timer _timerBetweenPingPong = null;

		[SerializeField]
		private bool _applyTimerAtDefaultPlay = true;

		[SerializeField]
		private bool _applyTimerAtReversePlay = true;

		[SerializeField]
		private bool _beginWithTimerAtFinishedState = true;

		private float _currentLerpTime = 0f;

		private Vector3 _worldStartPosition, _worldEndPosition;

		private bool _isReversing = false;

		private bool _willStopAtEnd = false;

		private bool _hasBeenStoppedInterrupted = false;

		public bool IsReversing => _isReversing;

		#region Simplified command for unity events
		public void Play(bool reverse)
		{
			if (reverse == true)
			{
				PlayReverse(false, false);
			}
			else
			{
				Play(false, false);
			}
		}

		public void ForcePlay(bool reverse)
		{
			if (reverse == true)
			{
				PlayReverse(false, true);
			}
			else
			{
				Play(false, true);
			}
		}

		public void PlayInterrupt(bool reverse)
		{
			if (reverse == true)
			{
				PlayReverse(true, false);
			}
			else
			{
				Play(true, false);
			}
		}

		public void ForcePlayInterrupt(bool reverse)
		{
			if (reverse == true)
			{
				PlayReverse(true, true);
			}
			else
			{
				Play(true, true);
			}
		}

		public void PlayPingPong() => PlayPingPong(false);
		public void ForcePlayPingPong() => PlayPingPong(true);
		#endregion

		public void SetStopWhenAtEndOfPath(bool value)
		{
			_stopWhenAtEndOfPath = value;
		}

		public bool PlayPingPong(bool interrupt = false)
		{
			bool playReverse = _isReversing;

			// Do not reverse if it has been interrupted by a Stop() before
			if (_hasBeenStoppedInterrupted == true)
			{
				playReverse = playReverse == false;
				_hasBeenStoppedInterrupted = false;

				return playReverse == true ? Play(interrupt, true) : PlayReverse(interrupt, true);
			}

			return playReverse == true ? Play(interrupt, false) : PlayReverse(interrupt);
		}

		public bool PlayReverse(bool interrupt = false, bool force = false)
		{
			bool canPlay = interrupt == true || enabled == false;
			if (canPlay == true)
			{
				canPlay = _isReversing == false;
			}
			if (canPlay == true || force == true)
			{
				_isReversing = true;
				SetComponentEnable(true);
				return true;
			}
			return false;
		}

		public bool Play(bool interrupt = false, bool force = false)
		{
			bool canPlay = interrupt == true || enabled == false;
			if (canPlay == true)
			{
				canPlay = _isReversing == true;
			}
			if (canPlay == true || force == true)
			{
				_isReversing = false;
				SetComponentEnable(true);
				return true;
			}
			return false;
		}

		public void Stop(bool interrupt = false)
		{
			if (interrupt == true)
			{
				if (_currentLerpTime > 0 && _currentLerpTime < 1)
				{
					_hasBeenStoppedInterrupted = true;
				}
				SetComponentEnable(false);
			}
			else
			{
				_willStopAtEnd = enabled == true;
			}
		}

		public void SetPositions(Vector3 startPosition, Vector3 endPosition)
		{
			_startPosition = startPosition;
			_endPosition = endPosition;
			CacheWorldPositions();
		}

		private void Awake()
		{
			CacheWorldPositions();

		}

		private void Start()
		{
			if (_enabledAtStart == false)
			{
				enabled = false;
				_isReversing = true; // permit to play the first time 
			}
		}

		private void OnEnable()
		{
			if (_beginWithTimerAtFinishedState == true)
			{
				_timerBetweenPingPong.ForceFinishState();
			}
			else
			{
				_timerBetweenPingPong.Start();
			}
			//_timerBetweenPingPong.ResetTimeElapsed();
		}

		private void FixedUpdate()
		{
			if (_timerBetweenPingPong.Update() == true)
			{
				_currentLerpTime = ComputePingPongMovement();

				transform.position = Vector3.Lerp(_worldStartPosition, _worldEndPosition, _movementCurve.Evaluate(_currentLerpTime));
			}
		}

		private float ComputePingPongMovement()
		{
			float result = _currentLerpTime + _speed * (_isReversing == true ? -Time.deltaTime : Time.deltaTime);
			if (result >= 1f)
			{
				result = 1f;
				if (_stopWhenAtEndOfPath == true || _willStopAtEnd == true)
				{
					_willStopAtEnd = false;
					SetComponentEnable(false);
				}
				else
				{
					_isReversing = true;
				}

				if (_applyTimerAtDefaultPlay == true && _timerBetweenPingPong.Duration > 0)
				{
					_timerBetweenPingPong.Start();
				}
			}
			else if (result < 0f)
			{
				result = 0f;
				if (_stopWhenAtEndOfPath == true || _willStopAtEnd == true)
				{
					_willStopAtEnd = false;
					SetComponentEnable(false);
				}
				else
				{
					_isReversing = false;
				}

				if (_applyTimerAtReversePlay == true && _timerBetweenPingPong.Duration > 0)
				{
					_timerBetweenPingPong.Start();
				}
			}

			return result;
		}

		private void SetComponentEnable(bool isEnabled)
		{
			enabled = isEnabled;
		}

		private void CacheWorldPositions()
		{
			_worldStartPosition = _startPosition + transform.position;
			_worldEndPosition = _endPosition + transform.position;
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Handles.DrawDottedLine(_startPosition + transform.position, _endPosition + transform.position, 4);

			if (Application.isPlaying == false)
			{
				CacheWorldPositions();
			}
			Vector3 size = new Vector3(1f, 1f, 0.2f);
			Gizmos.color = Color.green;
			Gizmos.DrawCube(_worldStartPosition, size);
			Gizmos.color = Color.red;
			Gizmos.DrawCube(_worldEndPosition, size);
		}
#endif //UNITY_EDITOR
	}

//#if UNITY_EDITOR
//	[CustomEditor(typeof(MovingPlatform))]
//	public class MovingPlatformEditor : Editor
//	{
//		SerializedProperty startPositionProperty;
//		SerializedProperty endPositionProperty;
//		Transform transform;
//		MovingPlatform concreteTarget;
//		private void OnEnable()
//		{
//			startPositionProperty = serializedObject.FindProperty("_startPosition");
//			endPositionProperty = serializedObject.FindProperty("_endPosition");
//			transform = (serializedObject.targetObject as MonoBehaviour).transform;
//			concreteTarget = (target as MovingPlatform);
//		}

//		//private void OnSceneGUI()
//		//{
//		//	//Vector3 startPosition = startPositionProperty.vector3Value;
//		//	//Vector3 endPosition = endPositionProperty.vector3Value;

//		//	//Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

//		//	//float startSize = HandleUtility.GetHandleSize(startPosition + transform.position) * 0.5f;
//		//	//float endSize = HandleUtility.GetHandleSize(endPosition + transform.position) * 0.5f;

//		//	//EditorGUI.BeginChangeCheck();
//		//	////startPosition = Handles.PositionHandle(startPosition + transform.position, transform.rotation) - transform.position;
//		//	////endPosition = Handles.PositionHandle(endPosition + transform.position, transform.rotation) - transform.position;

//		//	////Vector3 snap = Vector3.one * 0.5f;
//		//	////startPosition = Handles.FreeMoveHandle(startPosition + transform.position, transform.rotation, startSize, snap, Handles.CubeHandleCap) - transform.position;
//		//	////endPosition = Handles.FreeMoveHandle(endPosition + transform.position, transform.rotation, endSize, snap, Handles.CubeHandleCap) - transform.position;


//		//	//UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
//		//	//if (EditorGUI.EndChangeCheck())
//		//	//{
//		//	//	Undo.RecordObject(target, "Change Moving Platform Start Or End Position");
//		//	//	concreteTarget.SetPositions(startPosition, endPosition);
//		//	//}
//		//}

//	}
//#endif //UNITY_EDITOR
}