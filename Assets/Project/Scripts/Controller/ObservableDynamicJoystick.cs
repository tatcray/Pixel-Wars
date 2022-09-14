using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controller
{
    public class ObservableDynamicJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Action<Vector2> JoystickMoveUpdate;
        public Action JoystickActivated;
        public Action JoystickDisabled;

        private bool isActivated;
        private Canvas canvas;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isActivated = true;
            JoystickActivated?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isActivated = false;
            JoystickDisabled?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            JoystickMoveUpdate(eventData.delta);
        }
    }
}