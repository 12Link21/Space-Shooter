using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;

    private Animator _animator;
    private BoxCollider2D _collider;

    private bool _isDead = false;

    [SerializeField]
    private AudioClip _explosionSoundEffect;
    [SerializeField]
    private AudioClip _laserSoundEffect;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _doubleLaserPrefab;
    private float _laserOffset = -0.8f;
    [SerializeField]
    private float _fireRate = 3.0f;
    [SerializeField]
    private float _canFire = -1.0f;

    [SerializeField]
    private float _screenTop = 6.25f;
    [SerializeField]
    private float _screenBottom = -4.25f;
    [SerializeField]
    private float _screenLeft = -10.25f;
    [SerializeField]
    private float _screenRight = 10.25f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _collider = gameObject.GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player not found");
        }

        if (_animator == null)
        {
            Debug.LogError("Enemy's Animator component not found");
        }

        if (_collider == null)
        {
            Debug.LogError("Enemy's BoxCollider2D component not found");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Enemy's AudioSource component not found");
        }
        else
        {
            _audioSource.clip = _explosionSoundEffect;
        }

        //StartCoroutine(StartShooting());
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire && _isDead == false)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < _screenBottom && _isDead == false)
        {
            Vector3 clampedPosition = transform.position;
            clampedPosition.y = _screenTop;
            clampedPosition.x = Random.Range(_screenLeft, _screenRight);
            transform.position = clampedPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.transform.tag)
        {
            case "Player":
                _player.Damage();
                DeathRoutine();
                break;
            case "Laser":
                if (other.GetComponent<Laser>().CheckIfEnemy() == false)
                {
                    Destroy(other.gameObject);
                    if (_player != null)
                    {
                        _player.ChangeScore(10);
                    }
                    DeathRoutine();
                }
                break;
            case "Enemy":
                break;
            default:
                //Debug.Log("Unsupported Interaction");
                break;
        }
    }

    private void DeathRoutine()
    {
        _isDead = true;
        _animator.SetTrigger("OnDeath");
        _collider.enabled = false;
        _audioSource.Play();
        Destroy(this.gameObject, 3.0f);
    }

    private void FireLaser()
    {
        _fireRate = Random.Range(3.0f, 7.0f);
        _canFire = Time.time + _fireRate;
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + _laserOffset, transform.position.z);
        GameObject doubleLaser = Instantiate(_doubleLaserPrefab, spawnPosition, Quaternion.identity);
        Laser[] lasers = doubleLaser.GetComponentsInChildren<Laser>();
        foreach (Laser item in lasers)
        {
            item.AssignLaserAsEnemy();
        }
        AudioSource.PlayClipAtPoint(_laserSoundEffect, new Vector3(0, 0, -9),0.5f);
    }

    /*
    private IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(1.0f);
        while (_isDead == false)
        {
            if(transform.position.y > _screenBottom)
            {
                FireLaser();
            }
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }*/
}
