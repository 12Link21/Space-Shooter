using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;

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
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < _screenBottom)
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
                Destroy(this.gameObject);
                break;
            case "Laser":
                // Call Player.ChangeScore(10) from Player;
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.ChangeScore(10);
                }
                Destroy(this.gameObject);
                break;
            default:
                Debug.Log("Unsupported Interaction");
                break;
        }
    }
}
