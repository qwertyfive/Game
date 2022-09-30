using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Core : MonoBehaviour
{


    public Vector3 MoveVector;

    private Vector3 Velocity;

    [SerializeField] private Material BallCollor;

    //Speed is between 0 and 1
    [SerializeField] public float Speed;


    public Transform RotationTransform;
    [SerializeField] private Transform ScaleTransform;
    [SerializeField] private GameObject Sphere;
    [SerializeField] private Rigidbody BallRigedbody;
    
    [SerializeField] private InventoryController Inventory;

    [SerializeField] private ParticleSystem Strike;
    [SerializeField] private ParticleSystem CoinAnimation;

    [SerializeField] private float ScaleChangeRate;
    [SerializeField] private float SquashMultiplier;
    [SerializeField] private float DelayMultiplier;
    [SerializeField] private float SpeedMultiplier;
    [SerializeField] private float MaxDelay;

    [SerializeField] private float MaxSquash;


    private Quaternion _targetRotation;
    private Quaternion _currentRotation;

    private Vector3 _currentScale;
    private Vector3 _targetScale;
    private Vector3 _currentPosition;
    private Vector3 _targetPosition;
    private Vector3 _savedMoveVector;
    private float _savedSpeed;
    private Vector3 _savedContactNormal;
    [HideInInspector] public bool _ground = false;


    bool CanMove = true;
    void Start()
    {
        _currentPosition = _targetPosition = BallRigedbody.transform.position;
        _currentScale = _targetScale = Vector3.one;
    }

    void FixedUpdate()
    {
        if (CanMove)
        {
            Move();
        }
    }

    private void Move()
    {

        Velocity = MoveVector * MoveCalculation(Speed) * SpeedMultiplier;

        if (Speed > 0)
        {
            _targetPosition = transform.position + Velocity;
        }

        if (!_ground)
        {
            Sphere.transform.Rotate(0f, 5f * MoveCalculation(Speed) * SpeedMultiplier, 0f);
            SpeedCheck(Time.deltaTime / 3);
            if(!Input.GetMouseButton(0)) ChangeTargetRotation( Quaternion.LookRotation(MoveVector, Vector3.up));
        }

        _currentScale = Vector3.Lerp(_currentScale, _targetScale, Time.deltaTime * 15f);
        if (_ground)
        {
            _currentRotation = Quaternion.Lerp(_currentRotation, _targetRotation, Time.deltaTime * 15f);
        }
        else
            _currentRotation = _targetRotation;
        _currentPosition = Vector3.Lerp(_currentPosition, _targetPosition,  Time.deltaTime * 15f);

        ChangeAllValues();
    }

    public void ChangeTargetRotation(Quaternion quaternion)
    {
        _targetRotation = quaternion;
    }
    public void ChangeAllValues()
    {

        ScaleTransform.localScale = _currentScale;
        transform.position = _currentPosition;
        RotationTransform.rotation = _targetRotation;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_ground) return;
        _ground = true;

        _savedSpeed = Speed;
        _savedMoveVector = MoveVector;
        Speed = 0;

        BallRigedbody.isKinematic = true;
        _savedContactNormal = other.contacts[0].normal;


        DelayMultiplier = MaxDelay * (1 - Mathf.Max(1f - _savedSpeed, MaxSquash));

        _targetScale =  new Vector3(Mathf.Min(1f + _savedSpeed, 1f + MaxSquash), 1.1f, Mathf.Max(1f - _savedSpeed, MaxSquash));
        Debug.Log(_targetScale);
        _targetPosition = _targetPosition + other.contacts[0].normal * Mathf.Max(1f - _savedSpeed, MaxSquash) /2;
        ChangeTargetRotation(Quaternion.LookRotation(other.contacts[0].normal, Vector3.up));

        Strike.transform.position = other.contacts[0].point;
        Strike.transform.rotation = _targetRotation;
        Strike.Play();


        float velocityProjectMagnitude = Vector3.Project(_savedMoveVector, -_savedContactNormal).magnitude;
        float groundedTimer = velocityProjectMagnitude * DelayMultiplier;
        groundedTimer = Mathf.Clamp(groundedTimer, 0f, 0.15f);
        Invoke("StartToChange", groundedTimer );
        Invoke("ResetSpeed", groundedTimer * 1.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            Inventory.AddMoney();
            other.GetComponent<CoinController>().Touch();
            CoinAnimation.transform.position = other.transform.position;
            CoinAnimation.transform.rotation = other.transform.rotation;
            CoinAnimation.Play();
        }
    }


    private void StartToChange()
    {
        _targetScale = Vector3.one;
    }
    private void ResetSpeed()
    {
        BallRigedbody.isKinematic = false;
        Speed = Mathf.Min(_savedSpeed + 0.05f, 1f);
        MoveVector = Vector3.Reflect(_savedMoveVector.normalized, _savedContactNormal);
        Invoke("ExitGroundMode", 0.05f);

    }
    private void ExitGroundMode()
    {
        _ground = false;
    }

    private void SpeedCheck(float delta)
    {
        Speed = Mathf.Max(0f, Speed - delta);
    }

    private float MoveCalculation(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }
}


