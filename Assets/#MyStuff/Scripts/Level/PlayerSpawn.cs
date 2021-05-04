using System;
using NonViolentFPS.Events;
using NonViolentFPS.Manager;
using NonViolentFPS.Utility;
using Unity.Mathematics;
using UnityEngine;

namespace NonViolentFPS.Level
{
	public class PlayerSpawn : MonoBehaviour
	{
		[SerializeField] private Mesh arrowMesh;
		[SerializeField] private Material arrowMaterial;
		[SerializeField] private Vector3 arrowOffset;
		[SerializeField] private Vector3 arrowRotation;
		[SerializeField] private float arrowSize;

		private void OnEnable()
		{
			GameEvents.Instance.OnPlayerLoaded += SetPlayerLocation;
		}

		private void OnDisable()
		{
			GameEvents.Instance.OnPlayerLoaded -= SetPlayerLocation;
		}

		private void Start()
		{
			SetPlayerLocation(GameManager.Instance.Player.transform);
		}

		private void SetPlayerLocation(Transform _playerTransform)
		{
			_playerTransform.position = transform.position;
			_playerTransform.rotation = transform.rotation;
		}

		#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			var iconSize = AnnotationUtiltyWrapper.IconSize;

			var localToWorldMatrix = transform.localToWorldMatrix;
			var offsetMatrix = Matrix4x4.Translate(arrowOffset * iconSize);
			var rotationMatrix = Matrix4x4.Rotate(quaternion.Euler(arrowRotation));
			var scaleMatrix = Matrix4x4.Scale(Vector3.one * arrowSize * iconSize);
			var arrowMatrix = localToWorldMatrix * offsetMatrix * rotationMatrix * scaleMatrix;

			arrowMaterial.SetPass(0);
			Graphics.DrawMeshNow(arrowMesh, arrowMatrix);
		}
		#endif
	}
}