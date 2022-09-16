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

        private Canvas canvas;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            JoystickActivated?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            JoystickDisabled?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            JoystickMoveUpdate(eventData.delta);
        }
    }
}