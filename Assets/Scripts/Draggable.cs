using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public bool IsDragging;
    public Vector3 LastPosition;

    private Collider2D _collider;
    private DragController _dragcontroller;
    private float _movementTime = 15f;
    private System.Nullable<Vector3> _movementDestination;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _dragcontroller = FindObjectOfType<DragController>();
    }

    void FixedUpdate()
    {
        if (_movementDestination.HasValue)
        {
            if (IsDragging)
            {
                _movementDestination = null;
                return;
            }

            if (transform.position == _movementDestination)
            {
                gameObject.layer = Layer.Default;
                _movementDestination = null;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, _movementDestination.Value, _movementTime * Time.fixedDeltaTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Draggable collidedDraggable = other.GetComponent<Draggable>();
        if (collidedDraggable != null && _dragcontroller.lastDragged.gameObject == gameObject)
        {
            ColliderDistance2D colliderDistance2D = other.Distance(_collider);
            Vector3 diff = new Vector3(colliderDistance2D.normal.x, colliderDistance2D.normal.y) * colliderDistance2D.distance;
            transform.position -= diff;
        }

        //if (other.CompareTag("DropValid") )
        if (other.tag == this.tag)
        {
            // Intentamos obtener el centro del BoxCollider2D del objeto con el que colisionamos
            BoxCollider2D box = other.GetComponent<BoxCollider2D>();
            if (box != null)
            {
                // El centro del collider en coordenadas mundiales
                Vector3 center = box.bounds.center;
                _movementDestination = center;
            }
            else
            {
                // Fallback a la posición del objeto si no tiene BoxCollider2D
                _movementDestination = other.transform.position;
            }

            //_movementDestination = other.transform.position;
        }
        else if (other.tag != this.tag)
        {
            _movementDestination = LastPosition;
        }
    }
}
