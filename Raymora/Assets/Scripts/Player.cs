using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	[SerializeField] private float _sensitivityX = 2f;
	[SerializeField] private float _sensitivityY = 2f;
	[SerializeField] private float _minimumX = -360f;
	[SerializeField] private float _maximumX = 360f;
	[SerializeField] private float _minimumY = -60f;
	[SerializeField] private float _maximumY = 60f;
	[SerializeField] private float _maxTargetDistance = 100f;

	private float _rotationX = 0f;
	private float _rotationY = 0f;
	private List<float> _rotArrayY = new List<float>();
	private List<float> _rotArrayX = new List<float>();
	private float _rotAverageX = 0f;
	private float _rotAverageY = 0f;
	private int _frameCounter = 20;
	private Quaternion _originalRotation;
	private Fish _targetFish;

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
				// join
			}
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
		if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _maxTargetDistance) && hit.transform.tag == "Fish")
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