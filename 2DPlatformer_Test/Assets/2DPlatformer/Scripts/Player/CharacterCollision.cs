namespace GSGD2.Player
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Utilities;

	/// <summary>
	/// Class that check and store world collision for movement. Ground, wall and slope will be checked and their normals remembered in order to use it for movement (project movement on xy plane, wall grab, y replacer direction, etc...)
	/// </summary>
	public class CharacterCollision : MonoBehaviour
	{
		#region Fields
		[Header("Ground")]
		[SerializeField]
		private Raycaster _frontGroundRaycaster = null;

		[SerializeField]
		private Raycaster _backGroundRaycaster = null;

		[Header("Wall")]
		[SerializeField]
		private Raycaster _topRightWallRaycaster = null;

		[SerializeField]
		private Raycaster _midRightWallRaycaster = null;

		[SerializeField]
		private Raycaster _downRightWallRaycaster = null;

		[SerializeField]
		private Raycaster _topLeftWallRaycaster = null;

		[SerializeField]
		private Raycaster _midLeftWallRaycaster = null;

		[SerializeField]
		private Raycaster _downLeftWallRaycaster = null;

		[SerializeField]
		private Raycaster _yRightReplacerBonusRaycaster = null;

		[SerializeField]
		private Raycaster _yLeftReplacerBonusRaycaster = null;

		/// <summary>
		/// Half size of the character. Used to find the bound when replacing the character, <see cref="HandleWallCollisionAndApplyBonusYReplacement(bool)"/>
		/// </summary>
		[SerializeField]
		private float _characterZExtent = 0.5f;

		/// <summary>
		/// Threshold at which a collider found by wall raycasters is considered a slope or a wall.
		/// </summary>
		[SerializeField]
		private float _slopeNormalThreshold = 0.7f;

		// Runtime fields
		private Vector3 _groundNormale;
		private Vector3 _wallNormal;
		private Vector3 _slopeNormal;
		private RaycastHit _lastWallRaycastHitResult;
		private RaycastHit _lastSlopeRaycastHitResult;
		private bool _hasAWallInFrontOfCharacter = false;
		private bool _hasAWallBehindCharacter = false;
		private bool _hasASlopeInFrontOfOrBehindCharacter = false;
		#endregion Fields

		#region Properties
		public Vector3 GroundNormal => _groundNormale;
		public Vector3 WallNormal => _wallNormal;
		public Vector3 SlopeNormal => _slopeNormal;
		public RaycastHit LastWallRaycastHitResult => _lastWallRaycastHitResult;
		public RaycastHit LastSlopeRaycastHitResult => _lastSlopeRaycastHitResult;
		public bool HasAWallInFrontOfCharacter => _hasAWallInFrontOfCharacter;
		public bool HasAWallBehindCharacter => _hasAWallBehindCharacter;
		public bool HasASlopeInFrontOfOrBehindCharacter => _hasASlopeInFrontOfOrBehindCharacter;
		public bool HasAWallNearCharacter => HasAWallBehindCharacter && HasAWallInFrontOfCharacter;

		/// <summary>
		/// Local position of character right bound, see <see cref="HandleWallCollisionAndApplyBonusYReplacement(bool)"/>
		/// </summary>
		public Vector3 RightBoundPosition => -Vector3.forward * _characterZExtent;

		/// <summary>
		/// Local position of character left bound, see <see cref="HandleWallCollisionAndApplyBonusYReplacement(bool)"/>
		/// </summary>
		public Vector3 LeftBoundPosition => Vector3.forward * _characterZExtent;
		#endregion Properties

		#region Methods

		public void HandleWallCollisionAndApplyBonusYReplacement(int lastMovementDirection, bool debug = false)
		{
			RaycastHit chosenWallRaycastHitResult = new RaycastHit();
			RaycastHit chosenSlopeRaycastHitResult = new RaycastHit();

			//Debug.LogFormat("HandleWallCollisionAndApplyBonusYReplacement {0}", _currentState);

			// Right
			bool topRightResult = _topRightWallRaycaster.RaycastAll(out RaycastHit[] topRightHits, debug: debug);
			bool downRightResult = _downRightWallRaycaster.RaycastAll(out RaycastHit[] downRightHits, debug: debug);
			bool midRightResult = _midRightWallRaycaster.RaycastAll(out RaycastHit[] midRightHits, debug: debug);
			bool yRightReplacerBonusResult = _yRightReplacerBonusRaycaster.RaycastAll(out RaycastHit[] yRightReplacerBonusHits, debug: debug);
			bool topRightSlopeResult = false, downRightSlopeResult = false, midRightSlopeResult = false;
			Vector3 rightWallNormal = Vector3.zero;
			if (topRightResult == true)
			{
				topRightResult = IsNormalIndicateAnyOfThisAsAWall(ref topRightHits, out rightWallNormal);
				if (topRightResult == true)
				{
					chosenWallRaycastHitResult = topRightHits[0];
				}
				else
				{
					topRightSlopeResult = IsNormalIndicateAnyOfThisAsASlope(ref topRightHits, out rightWallNormal);
					if (topRightSlopeResult == true)
					{
						chosenSlopeRaycastHitResult = topRightHits[0];
					}
				}
			}

			if (downRightResult == true)
			{
				downRightResult = IsNormalIndicateAnyOfThisAsAWall(ref downRightHits, out rightWallNormal);
				if (downRightResult == true)
				{
					chosenWallRaycastHitResult = downRightHits[0];
				}
				else
				{
					downRightSlopeResult = IsNormalIndicateAnyOfThisAsASlope(ref downRightHits, out rightWallNormal);
					if (downRightSlopeResult == true)
					{
						chosenSlopeRaycastHitResult = downRightHits[0];
					}
				}
			}

			if (midRightResult == true)
			{
				midRightResult = IsNormalIndicateAnyOfThisAsAWall(ref midRightHits, out rightWallNormal);
				if (midRightResult == true)
				{
					chosenWallRaycastHitResult = midRightHits[0];
				}
				else
				{
					midRightSlopeResult = IsNormalIndicateAnyOfThisAsASlope(ref midRightHits, out rightWallNormal);
					if (midRightSlopeResult == true)
					{
						chosenSlopeRaycastHitResult = midRightHits[0];
					}
				}
			}

			if (yRightReplacerBonusResult == true)
			{
				yRightReplacerBonusResult = IsNormalIndicateAnyOfThisAsAWall(ref yRightReplacerBonusHits, out _);
			}

			// Left
			bool topLeftResult = _topLeftWallRaycaster.RaycastAll(out RaycastHit[] topLeftHits, debug: debug);
			bool downLeftResult = _downLeftWallRaycaster.RaycastAll(out RaycastHit[] downLeftHits, debug: debug);
			bool midLeftResult = _midLeftWallRaycaster.RaycastAll(out RaycastHit[] midLeftHits, debug: debug);
			bool yLeftReplacerBonusResult = _yLeftReplacerBonusRaycaster.RaycastAll(out RaycastHit[] yLeftReplacerBonusHits, debug: debug);
			bool topLeftSlopeResult = false, downLeftSlopeResult = false, midLeftSlopeResult = false;

			Vector3 leftWallNormal = Vector3.zero;
			if (topLeftResult == true)
			{
				topLeftResult = IsNormalIndicateAnyOfThisAsAWall(ref topLeftHits, out leftWallNormal);
				if (topLeftResult == true)
				{
					chosenWallRaycastHitResult = topLeftHits[0];
				}
				else
				{
					topLeftSlopeResult = IsNormalIndicateAnyOfThisAsASlope(ref topLeftHits, out leftWallNormal);
					if (topLeftSlopeResult == true)
					{
						chosenSlopeRaycastHitResult = midRightHits[0];
					}
				}
			}
			if (downLeftResult == true)
			{
				downLeftResult = IsNormalIndicateAnyOfThisAsAWall(ref downLeftHits, out leftWallNormal);
				if (downLeftResult == true)
				{
					chosenWallRaycastHitResult = downLeftHits[0];
				}
				else
				{
					downLeftSlopeResult = IsNormalIndicateAnyOfThisAsASlope(ref downLeftHits, out leftWallNormal);
					if (downLeftSlopeResult == true)
					{
						chosenSlopeRaycastHitResult = downLeftHits[0];
					}
				}
			}

			if (midLeftResult == true)
			{
				midLeftResult = IsNormalIndicateAnyOfThisAsAWall(ref midLeftHits, out leftWallNormal);
				if (midLeftResult == true)
				{
					chosenWallRaycastHitResult = midLeftHits[0];
				}
				else
				{
					midLeftSlopeResult = IsNormalIndicateAnyOfThisAsASlope(ref midLeftHits, out leftWallNormal);
					if (midLeftSlopeResult == true)
					{
						chosenSlopeRaycastHitResult = midLeftHits[0];
					}
				}
			}

			if (yLeftReplacerBonusResult == true)
			{
				yLeftReplacerBonusResult = IsNormalIndicateAnyOfThisAsAWall(ref yLeftReplacerBonusHits, out _);
			}

			bool hasABonusYReplacement =
				lastMovementDirection > 0
				? downRightResult == true && yRightReplacerBonusResult == false
				: downLeftResult == true && yLeftReplacerBonusResult == false;

			if (hasABonusYReplacement == true)
			{
				Vector3 position = transform.position;
				position.y = _yRightReplacerBonusRaycaster.WorldPosition.y;
				//TODO AL  here apply a little of z position
				transform.position = position;
			}
			else
			{
				_lastWallRaycastHitResult = chosenWallRaycastHitResult;
				_lastSlopeRaycastHitResult = chosenSlopeRaycastHitResult;
				if (lastMovementDirection > 0)
				{
					_hasAWallInFrontOfCharacter = topRightResult || midRightResult || downRightResult;
					_hasAWallBehindCharacter = topLeftResult || midLeftResult || downLeftResult;
					_wallNormal = rightWallNormal;
					_hasASlopeInFrontOfOrBehindCharacter = topRightSlopeResult || downRightSlopeResult || midRightSlopeResult;
				}
				else
				{
					_hasAWallInFrontOfCharacter = topLeftResult || midLeftResult || downLeftResult;
					_hasAWallBehindCharacter = topRightResult || midRightResult || downRightResult;
					_wallNormal = leftWallNormal;
					_hasASlopeInFrontOfOrBehindCharacter = topLeftSlopeResult || downLeftSlopeResult || midLeftSlopeResult;
				}
			}
		}

		public float GetReplacementZPosition(float lastMovementDirection)
		{
			return LastWallRaycastHitResult.point.z + (lastMovementDirection > 0 ? RightBoundPosition.z : LeftBoundPosition.z);
			//return LastWallRaycastHitResult.point + (lastMovementDirection > 0 ? RightBoundPosition : LeftBoundPosition);
		}

		public void ResetCurrentValues()
		{
			_groundNormale = Vector3.zero;
			_wallNormal = Vector3.zero;
			_slopeNormal = Vector3.zero;
		}

		public void ResetMaxDistances()
		{
			_topRightWallRaycaster.ResetMaxDistance();
			_midRightWallRaycaster.ResetMaxDistance();
			_downRightWallRaycaster.ResetMaxDistance();
			_topLeftWallRaycaster.ResetMaxDistance();
			_midLeftWallRaycaster.ResetMaxDistance();
			_downLeftWallRaycaster.ResetMaxDistance();
			_yLeftReplacerBonusRaycaster.ResetMaxDistance();
			_yRightReplacerBonusRaycaster.ResetMaxDistance();
		}

		public void SetMaxDistance(float maxDistance)
		{
			_topRightWallRaycaster.SetMaxDistance(maxDistance);
			_midRightWallRaycaster.SetMaxDistance(maxDistance);
			_downRightWallRaycaster.SetMaxDistance(maxDistance);
			_topLeftWallRaycaster.SetMaxDistance(maxDistance);
			_midLeftWallRaycaster.SetMaxDistance(maxDistance);
			_downLeftWallRaycaster.SetMaxDistance(maxDistance);
			_yLeftReplacerBonusRaycaster.SetMaxDistance(maxDistance);
			_yRightReplacerBonusRaycaster.SetMaxDistance(maxDistance);
		}

		public bool CheckGround()
		{
			bool frontResult = _frontGroundRaycaster.RaycastAll(out RaycastHit[] frontHits);
			bool backResult = _backGroundRaycaster.RaycastAll(out RaycastHit[] backHits);
			Vector3 floorNormal = Vector3.up;
			if (frontResult == true)
			{
				frontResult = IsNormalIndicateAnyOfThisAsAFloor(ref frontHits, out floorNormal);
			}
			if (frontResult == false && backResult == true)
			{
				backResult = IsNormalIndicateAnyOfThisAsAFloor(ref backHits, out floorNormal);
			}

			// if touching ground
			if (frontResult == true || backResult == true)
			{
				_groundNormale = floorNormal;
				return true;
			}
			else
			{
				_groundNormale = Vector3.zero;
				return false;
			}
		}

		private void Awake()
		{
			_topRightWallRaycaster.Initialize();
			_midRightWallRaycaster.Initialize();
			_downRightWallRaycaster.Initialize();
			_backGroundRaycaster.Initialize();
			_frontGroundRaycaster.Initialize();
			_topLeftWallRaycaster.Initialize();
			_midLeftWallRaycaster.Initialize();
			_downLeftWallRaycaster.Initialize();
			_yRightReplacerBonusRaycaster.Initialize();
			_yLeftReplacerBonusRaycaster.Initialize();
		}

		// TODO AL : improve perfs by checking walls and slopes in the same loop
		// use this pattern in a later retake
		//private enum CollidedType
		//{
		//	Ground,
		//	Wall,
		//	Slope
		//}

		private bool IsNormalIndicateAnyOfThisAsAWall(ref RaycastHit[] hits, out Vector3 wallNormal)
		{
			for (int i = 0; i < hits.Length; i++)
			{
				var hit = hits[i];
				//Debug.LogFormat("hit.normal.y {0} < {1} _slopeNormalThreshold", hit.normal.y,  _slopeNormalThreshold);
				if (hit.normal.y < _slopeNormalThreshold)
				{
					wallNormal = hit.normal;

					Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red);
					return true;
				}
				Debug.DrawLine(hit.point, hit.point + hit.normal, Color.green);
			}
			wallNormal = Vector3.zero;
			return false;
		}

		private bool IsNormalIndicateAnyOfThisAsAFloor(ref RaycastHit[] hits, out Vector3 floorNormal)
		{
			for (int i = 0; i < hits.Length; i++)
			{
				var hit = hits[i];

				//Debug.LogFormat("hit.normal.y {0} < {1} _slopeNormalThreshold", hit.normal.y,  _slopeNormalThreshold);
				if (hit.normal.y > _slopeNormalThreshold)
				{
					Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red);
					floorNormal = hit.normal;
					return true;
				}
				Debug.DrawLine(hit.point, hit.point + hit.normal, Color.green);
			}
			floorNormal = Vector3.zero;
			return false;
		}

		private bool IsNormalIndicateAnyOfThisAsASlope(ref RaycastHit[] hits, out Vector3 slopeNormal)
		{
			for (int i = 0; i < hits.Length; i++)
			{
				var hit = hits[i];

				//Debug.LogFormat("hit.normal.y {0} < {1} _slopeNormalThreshold", hit.normal.y,  _slopeNormalThreshold);
				var absZNormal = Mathf.Abs(hit.normal.z);
				if (absZNormal != 1 && absZNormal != 0)
				{
					Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red);
					slopeNormal = hit.normal;
					return true;
				}
				Debug.DrawLine(hit.point, hit.point + hit.normal, Color.green);
			}
			slopeNormal = Vector3.zero;
			return false;
		}

		private void OnDrawGizmos()
		{
			_topRightWallRaycaster.DrawGizmos();
			_midRightWallRaycaster.DrawGizmos();
			_downRightWallRaycaster.DrawGizmos();
			_backGroundRaycaster.DrawGizmos();
			_frontGroundRaycaster.DrawGizmos();
			_topLeftWallRaycaster.DrawGizmos();
			_midLeftWallRaycaster.DrawGizmos();
			_downLeftWallRaycaster.DrawGizmos();
			_yRightReplacerBonusRaycaster.DrawGizmos();
			_yLeftReplacerBonusRaycaster.DrawGizmos();

			Gizmos.DrawWireCube(transform.position + Vector3.up * 1f, new Vector3(1f, 2f, _characterZExtent * 2));
		}
		#endregion Methods
	}
}