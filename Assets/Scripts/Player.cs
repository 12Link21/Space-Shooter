using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShootPrefab;
    [SerializeField]
    private float _laserOffset = 0.8f;
    [SerializeField]
    private float _fireRate = 0.2f;

    private bool _isTripleShootActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;
    private float _canFire = -1.0f;

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2.0f;
    [SerializeField]
    private int _lives = 3;

    private UIManager _uiManager;
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _shieldsVisual;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    private Vector3 _startingPosition;

    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private bool _isPlayer1;
    [SerializeField]
    private bool _isPlayer2;

    private int _playerId;
    private string _horizontalAxis;
    private string _verticalAxis;
    private KeyCode _fireButton;

    [SerializeField]
    private float _screenTop = 5.75f;
    [SerializeField]
    private float _screenBottom = -3.75f;
    [SerializeField]
    private float _screenLeft = -10.25f;
    [SerializeField]
    private float _screenRight = 10.25f;

    [SerializeField]
    private AudioClip _laserSoundEffect;
    [SerializeField]
    private AudioClip _explosionSoundEffect;
    private AudioSource _audioSource;
    private Animator _animator;


    // Start is called before the first frame update
    void Start()
    {
        if (_isPlayer1 == true)
        {
            _playerId = 0;
            _horizontalAxis = "Horizontal";
            _verticalAxis = "Vertical";
            _fireButton = KeyCode.Space;
        }
        else if (_isPlayer2 == true)
        {
            _playerId = 1;
            _horizontalAxis = "Horizontal2";
            _verticalAxis = "Vertical2";
            _fireButton = KeyCode.Return;
        }
        else
        {
            Debug.LogError("Invalid Player detected");
        }

        _startingPosition = transform.position;

        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();

        if (_uiManager == null)
        {
            Debug.LogError("The UI_Manager is NULL");
        }

        if (_gameManager == null)
        {
            Debug.LogError("The Game_Manager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError(this.name + "'s AudioSource component not found");
        }

        if (_audioSource == null)
        {
            Debug.LogError(this.name + "'s AudioSource component not found");
        }

        if (_animator == null)
        {
            Debug.LogError(this.name + "'s Animator component not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement(_horizontalAxis, _verticalAxis);

#if UNITY_ANDROID
        if (CrossPlatformInputManager.GetButtonDown("Fire") && Time.time > _canFire)
        {
            FireLaser();
        }

#else 
        if (_gameManager.IsSinglePlayer == true)
        {
            if (Input.GetKeyDown(_fireButton) || Input.GetMouseButtonDown(0) && Time.time > _canFire)
            {
                FireLaser();
            }
        }
        else
        {
            if (Input.GetKeyDown(_fireButton) && Time.time > _canFire)
            {
                FireLaser();
            }
        }
#endif
    }

    void CalculateMovement(string h, string v)
    {
#if UNITY_ANDROID
        Vector3 direction = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"), 0);
#else
        Vector3 direction = new Vector3(Input.GetAxis(h), Input.GetAxis(v), 0);
#endif
        _animator.SetFloat("Direction", direction.x);

        if (_isSpeedBoostActive == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
        }

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

        if (_isTripleShootActive)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject tripleLaser = Instantiate(_tripleShootPrefab, spawnPosition, Quaternion.identity);
            foreach (Transform child in tripleLaser.transform)
            {
                child.GetComponent<Laser>().AssignShooterPlayer(this.gameObject);
            }
        }
        else
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + _laserOffset, transform.position.z);
            GameObject laser = Instantiate(_laserPrefab, spawnPosition, Quaternion.identity);
            laser.GetComponent<Laser>().AssignShooterPlayer(this.gameObject);
        }

        _audioSource.PlayOneShot(_laserSoundEffect);
    }

    public void Damage()
    {
        if (_isShieldsActive == true)
        {
            _isShieldsActive = false;
            _shieldsVisual.SetActive(false);
            return;
        }
        _lives--;

        _uiManager.UpdateLives(_lives,_playerId);

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
            _audioSource.PlayOneShot(_explosionSoundEffect, .5f);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
            _audioSource.PlayOneShot(_explosionSoundEffect, .5f);
        }
        else if (_lives < 1)
        {
            AudioSource.PlayClipAtPoint(_explosionSoundEffect, new Vector3(0, 0, -9));
            if (_gameManager.Players.Count == 1)
            {
                _gameManager.GameOver();
            }
            else
            {
                foreach (GameObject item in _gameManager.Players)
                {
                    if (item == this.gameObject)
                    {
                        _gameManager.Players.Remove(item);
                        break;
                    }
                }
            }
            Destroy(this.gameObject);
        }
    }

    public void EnableTripleShoot()
    {
        _isTripleShootActive = true;
        StartCoroutine(TripleShootPowerDownRoutine());
    }

    private IEnumerator TripleShootPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShootActive = false;
    }

    public void EnableSpeedBoost()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    private IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }

    public void EnableShields()
    {
        _isShieldsActive = true;
        _shieldsVisual.SetActive(true);
    }

    public void ChangeScore (int points)
    {
        _score += points;
        // call UpdateScore(_score) from UI_Manager.UIMAnager
        _uiManager.UpdateScore(_score,_playerId);
    }
}
