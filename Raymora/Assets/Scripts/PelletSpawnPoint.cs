using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletSpawnPoint : MonoBehaviour
{
	[SerializeField] private GameObject _pelletPrefab = null;
	[SerializeField] private Vector2 _spawnDelayRange = Vector2.zero;

	private float _spawnTime;
	private float _spawnTimer;
	
	private void Start()
	{
		ResetTimer();
	}

	private void Update()
	{
		_spawnTimer += Time.deltaTime;

		if(_spawnTimer >= _spawnTime)
		{
			Instantiate(_pelletPrefab, transform.position, Quaternion.identity).GetComponent<Pellet>();
			
			ResetTimer();
		}
	}

	private void ResetTimer()
	{
		_spawnTime = Random.Range(_spawnDelayRange.x, _spawnDelayRange.y);

		_spawnTimer = 0f;
	}
}
