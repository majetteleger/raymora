using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fish : MonoBehaviour
{
	//[SerializeField] private float _maxHealth = 0f;
	[SerializeField] private Color _targetedColor = Color.black;
	[SerializeField] private float _rotationspeed = 0f;
	
	public Pellet TargetPellet { get; set; }

	private Color _baseColor;
	private Material[] _materials;
	
	private void Awake()
	{
		_materials = GetComponentsInChildren<SkinnedMeshRenderer>().Select(x => x.material).ToArray();
		_baseColor = _materials[0].color;
	}

	private void Update()
	{
		if (TargetPellet != null)
		{
			var direction = TargetPellet.transform.position - transform.position;

			transform.position += direction * _rotationspeed * Time.deltaTime;
		}
		else
		{
			transform.RotateAround(Vector3.zero, Vector3.up, _rotationspeed * Time.deltaTime);
		}
	}
	
	private void OnTriggerEnter(Collider other)
	{
		var pellet = other.transform.GetComponentInParent<Pellet>();

		if(pellet == null)
		{
			return;
		}

		if (other == pellet.AttractionCollider)
		{
			TargetPellet = pellet;

			TargetPellet.TargetingFish.Add(this);
		}
		else if (other == pellet.ConsumptionCollider)
		{
			// Consume

			if (pellet == TargetPellet)
			{
				TargetPellet = null;
			}

			Destroy(pellet.gameObject);
		}
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
