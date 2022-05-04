using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_Pointer : MonoBehaviour
{
    Transform _transform;
    Vector3 _mousePos;

    public Vector3 ScreenPointerPosition => Input.mousePosition;
    public Vector3 CanvasPointerPosition
    {
        get
        {
            return GetCanvasPointerPosition();
        }
    }

    // v2.6 - added property Instance in the BE2_Pointer
    static BE2_Pointer _instance;
    public static BE2_Pointer Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType<BE2_Pointer>();
            }
            return _instance;
        }
        set => _instance = value;
    }

    void Awake()
    {
        _transform = transform;
    }

    //void Start()
    //{
    //
    //}

    void Update()
    {
        UpdatePointerPosition();
    }

    Vector3 GetMouseInCanvas(Vector3 position)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            BE2_DragDropManager.DragDropComponentsCanvas.transform as RectTransform,
            position,
            BE2_Inspector.Instance.Camera,
            out Vector3 mousePosition
        );
        return mousePosition;
    }

    Vector3 GetCanvasPointerPosition()
    {
        Camera mainCamera = BE2_Inspector.Instance.Camera;
        if (BE2_Inspector.Instance.CanvasRenderMode == RenderMode.ScreenSpaceOverlay)
        {
            return ScreenPointerPosition;
        }
        else if (BE2_Inspector.Instance.CanvasRenderMode == RenderMode.ScreenSpaceCamera)
        {
            var screenPoint = ScreenPointerPosition;
            screenPoint.z = BE2_DragDropManager.DragDropComponentsCanvas.transform.position.z - mainCamera.transform.position.z; //distance of the plane from the camera
            return GetMouseInCanvas(screenPoint);
        }
        else if (BE2_Inspector.Instance.CanvasRenderMode == RenderMode.WorldSpace)
        {
            var screenPoint = ScreenPointerPosition;
            screenPoint.z = BE2_DragDropManager.DragDropComponentsCanvas.transform.position.z - mainCamera.transform.position.z; //distance of the plane from the camera
            return GetMouseInCanvas(screenPoint);
        }

        return Vector3.zero;
    }

    public void UpdatePointerPosition()
    {
        _mousePos = GetCanvasPointerPosition();

        _transform.position = new Vector3(_mousePos.x, _mousePos.y, _transform.position.z);
        _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y, 0);
        _transform.localEulerAngles = Vector3.zero;
    }
}
