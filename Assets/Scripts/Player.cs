using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _laserOffset = 0.8f;
    [SerializeField]
    private float _fireRate = 0.2f;
    private float _canFire = -1.0f;

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private float _screenTop = 5.75f;
    [SerializeField]
    private float _screenBottom = -3.75f;
    [SerializeField]
    private float _screenLeft = -10.25f;
    [SerializeField]
    private float _screenRight = 10.25f;


    // Start is called before the first frame update
    void Start()
    {
        // take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        Vector3 clampedPosition = transform.position;

        clampedPosition.y = Mathf.Clamp(transform.position.y, _screenBottom, _screenTop);

        if (transform.position.x >= _screenRight)
        {
            clampedPosition.x = _screenLeft;
        }
        else if (transform.position.x <= _screenLeft)
        {
            clampedPosition.x = _screenRight;
        }

        transform.position = clampedPosition;
    }

    void FireLaser ()
    {
        _canFire = Time.time + _fireRate;
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + _laserOffset, transform.position.z);
        Instantiate(_laserPrefab, spawnPosition, Quaternion.identity);
    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
