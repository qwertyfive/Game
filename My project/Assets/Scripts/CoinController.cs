using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private float _scaleMultiplier;
    private Vector3 _currentScale;
    private Vector3 _targetScale;
    [SerializeField] private MeshRenderer Box;
    [SerializeField] private MeshRenderer Sphere;



    private void Start()
    {
        _scaleMultiplier = 1;
        _currentScale = _targetScale = transform.localScale;
    }
    private void FixedUpdate()
    {
        if(_currentScale.x > 0f ) transform.Rotate(0f, 45f * Time.deltaTime * _scaleMultiplier, 0f);

        _currentScale = Vector3.Lerp(_currentScale, _targetScale, Time.deltaTime * 3f);

        transform.localScale = _currentScale;
    }

    public void Touch()
    {
        _targetScale = Vector3.zero;
        Box.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        Sphere.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _scaleMultiplier = 15;
        this.GetComponent<BoxCollider>().enabled = this.GetComponent<SphereCollider>().enabled = false;
    }
}
