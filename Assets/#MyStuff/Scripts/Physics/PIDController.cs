//handles calculating a desired value based on a PID algorithm, a control system used in robotics

using UnityEngine;

namespace NonViolentFPS.Physics
{
    [System.Serializable]
    public class PIDController
    {
        //PID coefficients for tuning the controller
        //Default values from Unity hover racer workshop
        public float Kp = 0.8f;
        public float Ki = 0.1f;
        public float Kd = 0.1f;
        public float Minimum = -1;
        public float Maximum = 1;

        public PIDController(float pCoefficient, float iCoefficient, float dCoefficient)
        {
            Kp = pCoefficient;
            Ki = iCoefficient;
            Kd = dCoefficient;
        }

        //Variables to store values between calculations
        float integral;
        //float lastIntegral;
        float lastProportional;

        private Vector3 integralPos;
        private Vector3 lastProportionalPos;

        //Pass in the value we want and the value we currently have, the code
        //returns a number that moves the affected value towards its goal
        public float Control(float targetValue, float currentValue)
        {
            float deltaTime = Time.fixedDeltaTime;
            float proportional = targetValue - currentValue;

            float derivative = (proportional - lastProportional) / deltaTime;
            integral += proportional * deltaTime;
            lastProportional = proportional;

            //This is the actual PID formula. This gives us the value that is returned
            float value = Kp * proportional + Ki * integral + Kd * derivative;
            value = Mathf.Clamp(value, Minimum, Maximum);

            return value;
        }

        public Vector3 Control(Vector3 targetPosition, Vector3 currentPosition)
        {
            float deltaTime = Time.fixedDeltaTime;
            Vector3 proportional = targetPosition - currentPosition;

            Vector3 derivative = (proportional - lastProportionalPos);
            integralPos += proportional * deltaTime;
            lastProportionalPos = proportional;

            Vector3 value = Kp * proportional + Ki * integralPos + Kd * derivative;
            value = Vector3.ClampMagnitude(value, Maximum);

            return value;
        }
    }
}