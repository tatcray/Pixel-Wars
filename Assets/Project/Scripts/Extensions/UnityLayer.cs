using System;
using UnityEngine;

namespace Extensions
{
    [Serializable]
    public class UnityLayer
    {
        [SerializeField]
        private int layerIndex = 0;

        public int LayerIndex
        {
            get { return layerIndex; }
        }

        public void Set(int layerIndex)
        {
            if (layerIndex > 0 && layerIndex < 32)
            {
                this.layerIndex = layerIndex;
            }
        }

        public int Mask
        {
            get { return 1 << layerIndex; }
        }
    }
}