using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	//[SerializeField] private float _maxHealth = 0f;
	[SerializeField] private float _sensitivityX = 0f;
	[SerializeField] private float _sensitivityY = 0f;
	[SerializeField] private float _minimumX = 0f;
	[SerializeField] private float _maximumX = 0f;
	[SerializeField] private float _minimumY = 0f;
	[SerializeField] private float _maximumY = 0f;
	[SerializeField] private float _maxTargetDistance = 0f;
	[SerializeField] private float _joinTime = 0f;
	[SerializeField] private AnimationCurve _joinCurve = null;

	private float _rotationX;
	private float _rotationY;
	private List<float> _rotArrayY = new List<float>();
	private List<float> _rotArrayX = new List<float>();
	private float _rotAverageX;
	private float _rotAverageY;
	private int _frameCounter = 20;
	private Quaternion _originalRotation;
	private Fish _targetFish;
	private Fish _joinedFish;
	private float _joinTimer;
	private Vector3 _joinStartPosition;
	private Transform _joinTarget;

	private void Start()
	{
		var rigidbody = GetComponent<Rigidbody>();

		if (rigidbody != null)
		{
			rigidbody.freezeRotation = true;
		}

		_originalRotation = transform.localRotation;

		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		UpdateRotation();
		UpdateTarget();

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (Cursor.lockState == CursorLockMode.Locked)
			{
				Cursor.lockState = CursorLockMode.None;
			}
		}

		if (Input.GetMouseButtonDown(0))
		{
			if(Cursor.lockState == CursorLockMode.None)
			{
				Cursor.lockState = CursorLockMode.Locked;
			}

			if(_targetFish != null)
			{
				_joinTarget = _targetFish.transform;
				_joinStartPosition = transform.position;
			}
		}

		if(_joinTarget != null)
		{
			_joinTimer += Time.deltaTime / _joinTime;

			transform.position = Vector3.Lerp(_joinStartPosition, _joinTarget.position, _joinCurve.Evaluate(_joinTimer));

			if(_joinTimer >= _joinTime)
			{
				_joinedFish = _joinTarget.GetComponent<Fish>();

				_joinTarget = null;
				_joinTimer = 0f;
			}
		}

		if (_joinTarget == null && _joinedFish != null)
		{
			transform.position = _joinedFish.transform.position;
		}
	}

	private void UpdateRotation()
	{
		_rotAverageY = 0f;
		_rotAverageX = 0f;

		_rotationY += Input.GetAxis("Mouse Y") * _sensitivityY;
		_rotationX += Input.GetAxis("Mouse X") * _sensitivityX;

		_rotArrayY.Add(_rotationY);
		_rotArrayX.Add(_rotationX);

		if (_rotArrayY.Count >= _frameCounter)
		{
			_rotArrayY.RemoveAt(0);
		}
		if (_rotArrayX.Count >= _frameCounter)
		{
			_rotArrayX.RemoveAt(0);
		}

		for (int j = 0; j < _rotArrayY.Count; j++)
		{
			_rotAverageY += _rotArrayY[j];
		}
		for (int i = 0; i < _rotArrayX.Count; i++)
		{
			_rotAverageX += _rotArrayX[i];
		}

		_rotAverageY /= _rotArrayY.Count;
		_rotAverageX /= _rotArrayX.Count;

		_rotAverageY = ClampAngle(_rotAverageY, _minimumY, _maximumY);
		_rotAverageX = ClampAngle(_rotAverageX, _minimumX, _maximumX);

		var yQuaternion = Quaternion.AngleAxis(_rotAverageY, Vector3.left);
		var xQuaternion = Quaternion.AngleAxis(_rotAverageX, Vector3.up);

		transform.localRotation = _originalRotation * xQuaternion * yQuaternion;
	}

	private void UpdateTarget()
	{
		if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _maxTargetDistance, LayerMask.GetMask("Fish")))
		{
			if (_targetFish == null)
			{
				_targetFish = hit.transform.GetComponentInParent<Fish>();
				_targetFish.TargetOn();
			}
			else if (_targetFish.transform != hit.transform)
			{
				_targetFish.TargetOff();

				_targetFish = hit.transform.GetComponentInParent<Fish>();
				_targetFish.TargetOn();
			}
		}
		else
		{
			if (_targetFish != null)
			{
				_targetFish.TargetOff();
				_targetFish = null;
			}
		}

		Debug.DrawRay(transform.position, transform.forward * _maxTargetDistance, Color.cyan);
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		angle = angle % 360;

		if ((angle >= -360F) && (angle <= 360F))
		{
			if (angle < -360F)
			{
				angle += 360F;
			}
			if (angle > 360F)
			{
				angle -= 360F;
			}
		}

		return Mathf.Clamp(angle, min, max);
	}
}