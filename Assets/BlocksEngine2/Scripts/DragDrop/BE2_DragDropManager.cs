using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [ExecuteInEditMode]
public class BE2_DragDropManager : MonoBehaviour
{
    BE2_UI_ContextMenuManager _contextMenuManager;

    // v2.6 - BE2_DragDropManager using instance as property to guarantee return
    static BE2_DragDropManager _instance;
    public static BE2_DragDropManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType<BE2_DragDropManager>();
            }
            return _instance;
        }
        set => _instance = value;
    }

    public I_BE2_Raycaster Raycaster { get; set; }
    public Transform draggedObjectsTransform;
    public Transform DraggedObjectsTransform => draggedObjectsTransform;
    BE2_Pointer _pointer;
    public I_BE2_Drag CurrentDrag { get; set; }
    public I_BE2_Spot CurrentSpot { get; set; }
    List<I_BE2_Spot> _spotsList;
    public List<I_BE2_Spot> SpotsList
    {
        get
        {
            if (_spotsList == null)
                _spotsList = new List<I_BE2_Spot>();
            return _spotsList;
        }
        set
        {
            _spotsList = value;
        }
    }
    [SerializeField]
    Transform _ghostBlock;
    public Transform GhostBlockTransform => _ghostBlock;
    // v2.6 - removed unused ProgrammingEnv property from the Drag and Drop Manager
    //public I_BE2_ProgrammingEnv ProgrammingEnv { get; set; }
    public bool isDragging;
    public float detectionDistance = 40;

    // v2.6 - added property DragDropComponentsCanvas to the Drag and Drop Manager to be used as a reference Canvas 
    static Canvas _dragDropComponentsCanvas;
    public static Canvas DragDropComponentsCanvas
    {
        get
        {
            if (!_dragDropComponentsCanvas)
            {
                _dragDropComponentsCanvas = Instance.draggedObjectsTransform.GetComponentInParent<Canvas>();
            }
            return _dragDropComponentsCanvas;
        }
    }

    void OnEnable()
    {
        Instance = this;
    }

    void Awake()
    {
        _pointer = draggedObjectsTransform.GetComponent<BE2_Pointer>();
        Raycaster = GetComponent<I_BE2_Raycaster>();
        _dragDropComponentsCanvas = BE2_DragDropManager.Instance.draggedObjectsTransform.GetComponentInParent<BE2_Canvas>().Canvas;
    }

    void Start()
    {
        _contextMenuManager = BE2_UI_ContextMenuManager.instance;
    }

    float _holdCounter = 0;
    Vector2 _lastPosition;
    void Update()
    {
        // pointer 0 down
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnPointerDown();
        } 

        // pointer 1 down or pointer 1 hold
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            OnRightPointerDownOrHold();
        }
        if (CurrentDrag != null && !isDragging)
        {
            _holdCounter += Time.deltaTime;
            if (_holdCounter > 0.6f)
            {
                OnRightPointerDownOrHold();
                _holdCounter = 0;
            }
        }

        // pointer 0
        if (Input.GetKey(KeyCode.Mouse0))
        {
            // v2.6 - using BE2_Pointer as main pointer input source
            float distance = Vector2.Distance(_lastPosition, (Vector2)_pointer.ScreenPointerPosition);
            if (distance > 0.5f && !_contextMenuManager.isActive)
            {
                OnDrag();
            }
        }

        // pointer 0 up
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            OnPointerUp();
            _holdCounter = 0;
        }

        _lastPosition = _pointer.ScreenPointerPosition;
    }

    IEnumerator C_OnPointerDown()
    {
        yield return new WaitForEndOfFrame();

        _pointer.UpdatePointerPosition();
        I_BE2_Drag drag = Raycaster.GetDragAtPosition(_pointer.ScreenPointerPosition);
        if (drag != null)
        {
            CurrentDrag = drag;
            drag.OnPointerDown();
        }
        else
        {
            CurrentDrag = null;
        }
    }
    void OnPointerDown()
    {
        StartCoroutine(C_OnPointerDown());
    }

    void OnRightPointerDownOrHold()
    {
        I_BE2_Drag drag = Raycaster.GetDragAtPosition(_pointer.ScreenPointerPosition);
        if (drag != null)
        {
            drag.OnRightPointerDownOrHold();
        }
    }

    void OnDrag()
    {
        if (CurrentDrag != null)
        {
            CurrentDrag.OnDrag();
            isDragging = true;
        }

        BE2_MainEventsManager.Instance.TriggerEvent(BE2EventTypes.OnDrag);
    }

    void OnPointerUp()
    {
        if (CurrentDrag != null && isDragging)
        {
            CurrentDrag.OnPointerUp();
        }

        CurrentDrag = null;
        CurrentSpot = null;
        GhostBlockTransform.SetParent(null);
        isDragging = false;

        BE2_MainEventsManager.Instance.TriggerEvent(BE2EventTypes.OnPointerUpEnd);
    }

    public void AddToSpotsList(I_BE2_Spot spot)
    {
        if (!SpotsList.Contains(spot) && spot != null)
            SpotsList.Add(spot);
    }

    public void RemoveFromSpotsList(I_BE2_Spot spot)
    {
        if (SpotsList.Contains(spot))
            SpotsList.Remove(spot);
    }
}
