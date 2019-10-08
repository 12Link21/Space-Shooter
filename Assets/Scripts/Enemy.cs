using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        Vector3 clampedPosition = transform.position;

        if (transform.position.y < _screenBottom)
        {
            clampedPosition.y = _screenTop;
            clampedPosition.x = Random.Range(_screenLeft, _screenRight);
            transform.position = clampedPosition;
        }
    }
}
