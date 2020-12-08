using UnityEngine;

public class RotationSway : MonoBehaviour {
	[SerializeField]float amount = 0.02f;
	[SerializeField]float maxAmount = 0.03f;
	[SerializeField]float smooth = 3;
	private Vector3 def;
	[SerializeField]private bool  Paused = false;

	void  Start (){
		def = transform.localRotation.eulerAngles;
	}

	void  Update (){
		float factorX = (Input.GetAxis("Mouse Y")) * amount;
		float factorY = -(Input.GetAxis("Mouse X")) * amount;
		float factorZ = 0 * amount;

		if(!Paused){
			if (factorX > maxAmount)
				factorX = maxAmount;

			if (factorX < -maxAmount)
				factorX = -maxAmount;

			if (factorY > maxAmount)
				factorY = maxAmount;

			if (factorY < -maxAmount)
				factorY = -maxAmount;

			if (factorZ > maxAmount)
				factorZ = maxAmount;

			if (factorZ < -maxAmount)
				factorZ = -maxAmount;

			Quaternion Final = Quaternion.Euler(def.x+factorX, def.y+factorY, def.z+factorZ);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Final, (Time.time * smooth));
		}
	}
}