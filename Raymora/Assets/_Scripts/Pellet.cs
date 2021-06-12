using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
	[SerializeField] private SphereCollider _attractionCollider = null;
	[SerializeField] private SphereCollider _consumptionCollider = null;
	//[SerializeField] private float _healthGiven = 0f;
	
	public SphereCollider AttractionCollider => _attractionCollider;
	public SphereCollider ConsumptionCollider => _consumptionCollider;
	//public float HealthGiven => _healthGiven;

	public List<Fish> TargetingFish { get; set; } = new List<Fish>();

	public void OnDestroy()
	{
		foreach(var fish in TargetingFish)
		{
			fish.TargetPellet = null;
		}
	}
}
