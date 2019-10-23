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

    private bool isDead = false;

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
        if (_player == null)
        {
            Debug.LogError("Player not found");
        }

        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Enemy's Animator component not found");
        }

        _collider = gameObject.GetComponent<BoxCollider2D>();
        if (_collider == null)
        {
            Debug.LogError("Enemy's BoxCollider2D component not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < _screenBottom && isDead == false)
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
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.ChangeScore(10);
                }
                DeathRoutine();
                break;
            case "Enemy":
                break;
            default:
                Debug.Log("Unsupported Interaction");
                break;
        }
    }

    private void DeathRoutine()
    {
        isDead = true;
        _animator.SetTrigger("OnDeath");
        _collider.enabled = false;
        Destroy(this.gameObject, 3.0f);
    }
}
