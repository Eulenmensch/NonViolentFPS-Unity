using CMF;
using NonViolentFPS.Manager;
using UnityEngine;

public class LagBehindTarget : MonoBehaviour
{
	[SerializeField] private float rotationSnappyness;
	[SerializeField] private float positionSnappyness;

	[SerializeField] private Transform target;

	[SerializeField] private float xOffsetMax;
	[SerializeField] private float yOffsetMax;
	[SerializeField] private float zOffsetMax;
	[SerializeField, Min(0.01f)] private float xVelocityMax;
	[SerializeField, Min(0.01f)] private float yVelocityMax;
	[SerializeField, Min(0.01f)] private float zVelocityMax;

	private GameObject player;
	private AdvancedWalkerController controller;
	private float xRotationOffset;
	private float yRotationOffset;
	private float zPositionOffset;
	private Camera mainCamera;
	private Vector3 positionOffsetDynamic;

	private void Start()
	{
		mainCamera = Camera.main;
		transform.parent = null;

		player = GameManager.Instance.Player;
		controller = player.GetComponent<AdvancedWalkerController>();
	}

	private void Update()
	{
		CalculateOffsets();
		SetZPositionOffset();
		SetRotationOffset();
	}

	private void SetRotationOffset()
	{
		var targetRotation = target.rotation;
		targetRotation *= Quaternion.Euler(xRotationOffset, 0, -yRotationOffset);

		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSnappyness * Time.deltaTime);
	}

	private void SetZPositionOffset()
	{
		//Calculate the zOffset and then lerp it to create easing
		var positionOffset = new Vector3(0, 0, -zPositionOffset);
		positionOffsetDynamic = Vector3.Lerp(positionOffsetDynamic, positionOffset, positionSnappyness * Time.deltaTime);

		//Transform the target position into camera coordinates,
		//add the offset to it, then transform it back into world space
		var targetPosition = target.position;
		targetPosition = mainCamera.transform.InverseTransformPoint(targetPosition);
		targetPosition += positionOffsetDynamic;
		targetPosition = mainCamera.transform.TransformPoint(targetPosition);

		transform.position = targetPosition;
	}

	private void CalculateOffsets()
	{
		var velocity = controller.GetVelocity();
		velocity = mainCamera.transform.InverseTransformDirection(velocity);

		//calculate t value of lerp so that at 0 velocity, t is 0.5,
		//at >=max velocity t is 1 and at <= -max velocity t is 0
		var xVelocityScaled = 0.5f + velocity.x / (xVelocityMax * 2);
		xRotationOffset = Mathf.Lerp(-xOffsetMax, xOffsetMax, xVelocityScaled);
		var yVelocityScaled = 0.5f + velocity.y / (yVelocityMax * 2);
		yRotationOffset = Mathf.Lerp(-yOffsetMax, yOffsetMax, yVelocityScaled);
		var zVelocityScaled = velocity.z / zVelocityMax;
		zPositionOffset = Mathf.Lerp(0, zOffsetMax, zVelocityScaled);
	}
}
