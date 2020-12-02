using System;
using NonViolentFPS.Physics;
using UnityEditor;
using UnityEngine;

namespace NonViolentFPS.Shooting
{
	public class HoverEffect : MonoBehaviour,IHitscanEffect
	{
		#region Settings
		[Header( "Hover Settings" )]
		[SerializeField] private float hoverForce;                  //The force that pushes the board upwards
		[SerializeField] private float hoverHeight;                 //The ideal height at which the board wants to hover

		[Header( "Ground Stick Settings" )]
		[SerializeField] private bool stickToGround;                //Whether the board should stick to the ground when grounded
		[SerializeField] private float groundStickForce;            //The force applied inverse to ground normal
		[SerializeField] private float groundStickHeight;           //The maximum height at which the ground stick force is applied
		[SerializeField] private float maxStickAngle;               //The maximum angle relative to world xz plane where ground stick force is applied
		[SerializeField] private LayerMask groundMask;              //The layer mask that determines what counts as ground

		[Header( "Hover Point Settings" )]
		[SerializeField] private GameObject hoverPointPrefab;       //The hover point prefab
		[SerializeField] private GameObject hoverPointContainer;    //The GameObject to which the generated hoverpoints are childed
		[SerializeField] private BoxCollider hoverArea;             //The area in which a hoverpoint array is generated
		[SerializeField] private int hoverPointRows;                //how many hoverpoint rows are generated
		[SerializeField] private int hoverPointColumns;             //how many hoverpoint columns are generated

		[Header( "PID Controller Settings" )]
		[SerializeField, Range( 0.0f, 1.0f )] private float proportionalGain;  //A tuning value for the proportional error correction
		[SerializeField, Range( 0.0f, 1.0f )] private float integralGain;      //A tuning value for the integral error correction
		[SerializeField, Range( 0.0f, 1.0f )] private float derivativeGain;    //A tuning value for the derivative error correction

#if UNITY_EDITOR
		[Header( "Debug Settings" )]
		[SerializeField] private bool debugging;
		[SerializeField] private GUIStyle debugTextStyle;
		[SerializeField] private Gradient debugGradient;
#endif
		#endregion

		private Rigidbody rb;               //A reference to the board's rigidbody
		private Transform[] hoverPoints;    //The points from which ground distance is measured and where hover force is applied
		private PIDController[] piDs;       //References to the PIDController class that handles error correction and smoothens out the hovering

		public void Initialize(RaycastHit _hit)
		{
			var hoverEffect = _hit.transform.GetComponentInChildren<HoverEffect>();
			if (hoverEffect != this)
			{
				Destroy(gameObject);
			}
		}

		private void Start()
		{
			var parent = transform.parent;
			rb = parent.GetComponent<Rigidbody>();
			hoverArea = parent.GetComponent<BoxCollider>();

			//Initialize an array to contain all hover points
			hoverPoints = new Transform[hoverPointRows * hoverPointColumns];
			GenerateHoverPoints( hoverArea, hoverPointColumns, hoverPointRows );

			CreatePIDs();
		}

		private void FixedUpdate()
		{
			ApplyHoverForces();
			ApplyGroundStickForce();
#if UNITY_EDITOR
			UpdatePIDs();
#endif
		}

		private void GenerateHoverPoints(BoxCollider _area, int _columns, int _rows)
		{
			float columnSpacing = _area.size.x / ( _columns - 1 );
			float rowSpacing = _area.size.z / ( _rows - 1 );
			Vector3 rowOffset = new Vector3( 0, 0, rowSpacing );

			for ( int i = 0; i < _columns; i++ )
			{
				Vector3 columnHead = new Vector3(
					( _area.center.x - ( _area.size.x / 2 ) ) + ( columnSpacing * i ),
					_area.center.y,
					_area.center.z + ( _area.size.z / 2 )
				);

				for ( int j = 0; j < _rows; j++ )
				{
					Vector3 hoverPointPos = columnHead - ( rowOffset * j );
					hoverPointPos = transform.TransformPoint( hoverPointPos );
					GameObject newHoverPoint = Instantiate( hoverPointPrefab, hoverPointPos, Quaternion.identity, hoverPointContainer.transform );
					hoverPoints[( i * _rows ) + j] = newHoverPoint.transform;
				}
			}
		}

		private void CreatePIDs()
		{
			//Create an instance of the PIDController class for each hover point
			piDs = new PIDController[hoverPoints.Length];
			for ( int i = 0; i < hoverPoints.Length; i++ )
			{
				piDs[i] = new PIDController( proportionalGain, integralGain, derivativeGain );
			}
		}

		private void ApplyHoverForces()
		{
			foreach ( Transform hoverPoint in hoverPoints )
			{
				Vector3 hoverPointPos = hoverPoint.position;
				RaycastHit hit;

				if ( HoverRay( hoverPointPos, out hit ) )
				{
					float actualHeight = hit.distance;
					Vector3 groundNormal = hit.normal;

					//Use the respective PID controller to calculate the percentage of hover force to be used
					float forcePercent = piDs[Array.IndexOf( hoverPoints, hoverPoint )].Control( hoverHeight, actualHeight );

					//calculate the adjusted force in the direction of the ground normal
					Vector3 adjustedForce = hoverForce * forcePercent * groundNormal;

					//Add the force to the rigidbody at the respective hoverpoint's position
					rb.AddForceAtPosition( adjustedForce, hoverPointPos, ForceMode.Acceleration );
				}
			}
		}

		private void ApplyGroundStickForce()
		{
			var groundStickRay = new Ray( transform.position, -transform.up );

			if ( stickToGround)
			{
				if ( UnityEngine.Physics.Raycast( groundStickRay, out var hit, groundStickHeight, groundMask ) )
				{
					if ( CheckMaxAngle( hit.normal, maxStickAngle ) )
					{
						var force = -hit.normal * groundStickForce;
						rb.AddForce( force, ForceMode.Acceleration );
					}
				}
			}
		}

		private bool HoverRay(Vector3 _hoverPointPosition, out RaycastHit _hit)
		{
			RaycastHit hit;
			bool ray = UnityEngine.Physics.Raycast( _hoverPointPosition, -transform.up, out hit, hoverHeight, groundMask, QueryTriggerInteraction.Ignore );
			_hit = hit;
			return ray;
		}

		private bool CheckMaxAngle(Vector3 _normal, float _maxAngle)
		{
			float angle = Vector3.Angle( Vector3.up, _normal );
			if ( angle <= _maxAngle )
			{
				return true;
			}
			return false;
		}

		#region Editor
#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (Camera.current != Camera.main)
			{
				return;
			}
			if ( debugging )
			{
				if ( hoverPoints != null )
				{
					foreach ( var hoverPoint in hoverPoints )
					{
						Gizmos.DrawWireSphere( hoverPoint.position, 0.1f );

						RaycastHit hit;
						if ( HoverRay( hoverPoint.position, out hit ) )
						{
							Gizmos.color = debugGradient.Evaluate( hit.distance / hoverHeight );
							Gizmos.DrawSphere( hit.point, 0.07f );
							Debug.DrawLine( hoverPoint.position, hoverPoint.position - transform.up * hit.distance, debugGradient.Evaluate( hit.distance / hoverHeight ) );
						}
						Gizmos.color = Color.white;
					}
				}
			}
		}
		private void OnGUI()
		{
			if (Camera.current != Camera.main)
			{
				return;
			}
			if ( debugging )
			{
				foreach ( var hoverPoint in hoverPoints )
				{
					RaycastHit hit;
					if ( HoverRay( hoverPoint.position, out hit ) )
					{
						string text = ( hit.distance / hoverHeight ).ToString( "0.00" ); //the ratio of intended height and actual height
						Handles.Label( hoverPoint.position - transform.up * ( hit.distance / 2 ), text, debugTextStyle );
					}
				}
			}
		}

		private void UpdatePIDs()
		{
			foreach ( PIDController pid in piDs )
			{
				pid.Kp = proportionalGain;
				pid.Ki = integralGain;
				pid.Kd = derivativeGain;
			}
		}
#endif
		#endregion
	}
}
