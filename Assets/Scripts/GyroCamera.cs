using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GyroCamera : MonoBehaviour
{
    public float horizontalRange = 33.0f;
    public float verticalRange = 29.0f;

    private Gyroscope gyro;
    private Vector3 rotationAmount;
    private Camera camera;
    void Awake()
    {
        gyro = Input.gyro;
        gyro.enabled = true;

        camera = GetComponent<Camera>();
    }

    private void Start()
    {
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        GyroInput();
        TouchInput();
    }

    private void GyroInput()
    {
        rotationAmount = gyro.rotationRateUnbiased ;
        rotationAmount.x *= -1;
        rotationAmount.y *= -1;
        
        transform.Rotate(rotationAmount);
    }

    private void TouchInput()
    {
        foreach(Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = camera.ScreenPointToRay(touch.position);
                if(Physics.Raycast(ray, out hit))
                {
                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 3.0f);

                    if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        GameObject obj = hit.collider.gameObject;
                        Material material = hit.collider.gameObject.GetComponent<Renderer>().sharedMaterial;
                        
                        material.SetVector("Polygon_Tiling", Vector4.zero);

                    }

                }
            }
        }
    }

    public void OnResetOrientation()
    {
        transform.rotation = Quaternion.identity;
    }
}
