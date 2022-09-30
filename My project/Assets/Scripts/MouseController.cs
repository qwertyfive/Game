using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{
    private bool isMouseDragging = false;
    [SerializeField] public Camera CameraField;
    [SerializeField] public Transform Slicer;
    [SerializeField] public GameObject Ball;
    [SerializeField] public Image SlicerColor;

    [SerializeField] public float SliderRadius;
    [SerializeField] public float ScaleValue;

    private Vector3 CameraVector;
    private float _savedVelocity = 0;
    
    void FixedUpdate()
    {
        if(Ball.GetComponent<Core>()._ground)
        {
            Slicer.transform.localPosition = Vector3.zero;
            Slicer.transform.localScale = Vector3.zero;
            return;
        }

        if (Input.GetMouseButton(0) && Ball.GetComponent<Core>()._ground == false)
        {

            isMouseDragging = true;
            CameraVector = CameraField.ScreenToWorldPoint(Input.mousePosition);

            Ball.GetComponent<Core>().RotationTransform.rotation = Quaternion.LookRotation(CameraVector.normalized, Vector3.up);

            if (Vector2.Distance(Vector2.zero, new Vector2(CameraVector.x, CameraVector.z)) > SliderRadius)
            {
                float Distance = Vector2.Distance(Vector2.zero, new Vector2(CameraVector.x, CameraVector.z));
                CameraVector.x *= SliderRadius / Distance;
                CameraVector.z *= SliderRadius / Distance;

            }
            
            float alpha = Mathf.Deg2Rad * Camera.main.fieldOfView ;
            float beta = Mathf.Deg2Rad * Camera.main.transform.localEulerAngles.x;
            
            _savedVelocity = Vector2.Distance(Vector2.zero, new Vector2(CameraVector.x, FindLenght(alpha, beta, -CameraVector.z))) / SliderRadius;
            Coloring(_savedVelocity);

            Slicer.transform.localPosition = new Vector3(0f, 0f, ScaleValue * _savedVelocity);
            Slicer.transform.localScale = new Vector3(1f, 2 * ScaleValue * _savedVelocity, 1f);
        }
        if(!Input.GetMouseButton(0) && isMouseDragging)
        {
            isMouseDragging = false;

            Ball.GetComponent<Core>().MoveVector = CameraVector.normalized;
            Ball.GetComponent<Core>().Speed = Mathf.Min(_savedVelocity, 1f);

            Slicer.transform.localPosition = Vector3.zero;
            Slicer.transform.localScale = Vector3.zero;
        }
    }
    private void Coloring(float value)
    {
        if (value < 0.3f) {
            SlicerColor.color = Color.white;
            return;
        }

        if (value < 0.6f)
        {
            SlicerColor.color = Color.yellow;
            return; 
        }

        SlicerColor.color = Color.red;

    }
    private float FindLenght(float alpha, float beta, float a)
    {
        beta /= 2;

        return Mathf.Abs(a * Mathf.Sin(Mathf.PI + Mathf.Sign(a) * alpha) / Mathf.Sin(Mathf.Sign(a) * alpha + beta));
    }
}