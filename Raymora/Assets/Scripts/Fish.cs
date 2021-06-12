using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fish : MonoBehaviour
{
	[SerializeField] private Color _targetedColor = Color.black;
	[SerializeField] private float _rotationspeed = 0f;

	private Color _baseColor;
	private Material[] _materials;

	private void Awake()
	{
		_materials = GetComponentsInChildren<SkinnedMeshRenderer>().Select(x => x.material).ToArray();
		_baseColor = _materials[0].color;
	}

	private void Update()
	{
		transform.RotateAround(Vector3.zero, Vector3.up, _rotationspeed * Time.deltaTime);
	}

	public void TargetOn()
	{
		foreach (var material in _materials)
		{
			material.color = _targetedColor;
		}
	}

	public void TargetOff()
	{
		foreach (var material in _materials)
		{
			material.color = _baseColor;
		}
	}
}
