using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controller
{
    public class ObservableDynamicJoystick : DynamicJoystick
    {
        public Action<Vector2> JoystickMoveUpdate;
        public Action JoystickActivated;
        public Action JoystickDisabled;

        private bool isActivated;

        private void Update()
        {
            if (isActivated)
                JoystickMoveUpdate?.Invoke(Direction);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            isActivated = true;
            JoystickActivated?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            isActivated = false;
            JoystickDisabled?.Invoke();
        }
    }
}