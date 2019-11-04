using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefab;    

    [SerializeField]
    private AudioClip _explosionSoundEffect;

    [SerializeField]
    private float _rotateSpeed = -3.0f;

    [SerializeField]
    private GameStarter _gameStarter = null;

    // Start is called before the first frame update
    void Start()
    {

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
                AudioSource.PlayClipAtPoint(_explosionSoundEffect, new Vector3(0,0,-9));

                if (_gameStarter != null)
                {
                    _gameStarter.SendStartMessage();
                }

                Destroy(this.gameObject);
                break;
        }
    }
}
