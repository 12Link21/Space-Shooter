using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MovementCalculation(Vector3.up);
        }
        else
        {
            MovementCalculation(Vector3.down);
        }
    }

    void MovementCalculation(Vector3 direction)
    {
        transform.Translate(direction * _speed * Time.deltaTime);

        if (direction == Vector3.up)
        {
            if (transform.position.y > 7.0f)
            {
                if (this.transform.parent != null)
                {
                    Destroy(this.transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
        else if (direction == Vector3.down)
        {
            if (transform.position.y < -5.0f)
            {
                if (this.transform.parent != null)
                {
                    Destroy(this.transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
    }

    public void AssignLaserAsEnemy()
    {
        _isEnemyLaser = true;
    }

    public bool CheckIfEnemy()
    {
        return _isEnemyLaser;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(gameObject);
        }
        
    }
}
