using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;

    [SerializeField]
    private float _rotateSpeed = -3.0f;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate object on Z axis at 3m/s
        transform.Rotate(0, 0, _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Laser":
                Destroy(collision.gameObject);
                GameObject explosion =  Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, 3.0f);
                _spawnManager.StartSpawning();
                Destroy(this.gameObject);
                break;
        }
    }
}
