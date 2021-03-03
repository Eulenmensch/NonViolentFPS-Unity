using UnityEngine;

public class LagBehindTarget : MonoBehaviour
{
	[SerializeField] private float xSnappyness;
	[SerializeField] private float ySnappyness;
	[SerializeField] private float rollAmount;


	[SerializeField] private Transform target;

	private float defaultZRotation;

	private void Start()
	{
		transform.parent = null;
		defaultZRotation = transform.rotation.eulerAngles.z;
	}

	private void Update()
	{
		transform.position = target.position;
		// transform.position = Vector3.Lerp(transform.position, target.position, xSnappyness);

		// var rotation = transform.rotation;
		// var eulerRotation = rotation.eulerAngles;
		// var targetRotation = target.rotation;
		// var eulerTargetRotation = targetRotation.eulerAngles;
		//
		// eulerRotation.x = Mathf.Lerp(eulerRotation.x, eulerTargetRotation.x, xSnappyness );	//pitch
		// eulerRotation.z = Mathf.Lerp(eulerRotation.z, eulerTargetRotation.z, ySnappyness );	//yaw
		// //this is to make the weapon roll slightly when moving horizontally
		// eulerRotation.y = (1 - (eulerTargetRotation.z - eulerRotation.z)) * defaultZRotation * rollAmount; //roll
		//
		// transform.rotation = Quaternion.Euler(eulerRotation);

		transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, xSnappyness * Time.deltaTime );
	}
}
