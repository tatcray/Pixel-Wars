using System;
using Dependencies;
using UnityEngine;

namespace Controller
{
    public class Crosshair
    {
        public event Action<Vector3> PositionUpdated;
        public event Action ShootStarted;
        public event Action ShootStopped;
        
        private CrosshairReferences references;

        private float dragSpeed;
        private RectTransform transform;
        private RectTransform canvasRect;
        private Camera camera;
        
        public Crosshair(CrosshairReferences references)
        {
            this.references = references;
            transform = references.crosshairTransform;
            canvasRect = references.crosshairCanvasRect;
            dragSpeed = references.crosshairDragSpeed;
            camera = this.references.camera;
            
            references.joystick.JoystickMoveUpdate += MoveCrosshair;
            references.joystick.JoystickActivated += () => ShootStarted?.Invoke();
            references.joystick.JoystickDisabled += () => ShootStopped?.Invoke();
        }

        private void MoveCrosshair(Vector2 direction)
        {
            Vector2 anchoredPosition = transform.anchoredPosition + direction * dragSpeed;
            anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, 0, canvasRect.sizeDelta.x);
            anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, 0, canvasRect.sizeDelta.y);
            
            transform.anchoredPosition = anchoredPosition;
            
            Debug.DrawLine(transform.position, GetWorldPosition());
            PositionUpdated?.Invoke(GetWorldPosition());
        }

        private Vector3 GetWorldPosition()
        {
            Vector3 startPoint = transform.position;
            startPoint.z = -camera.transform.position.z;

            startPoint = camera.ScreenToWorldPoint(startPoint);
            
            return startPoint;
        }
    }
}