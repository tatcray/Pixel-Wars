using System;
using Controller;
using UnityEngine;

namespace Dependencies
{
    [Serializable]
    public class CrosshairReferences
    {
        public Camera camera;
        public RectTransform crosshairCanvasRect; 
        public float crosshairDragSpeed;
        public ObservableDynamicJoystick joystick;
        public RectTransform crosshairTransform;
    }
}