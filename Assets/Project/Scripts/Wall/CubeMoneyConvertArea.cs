using Extensions;
using UnityEngine;

namespace Wall
{
    public class CubeMoneyConvertArea
    {
        private ObservableSerializedObject<int> moneyObject;
        
        public CubeMoneyConvertArea(ObservableSerializedObject<int> moneyObject, TriggerListener area)
        {
            this.moneyObject = moneyObject;
            area.TriggerEntered += RegisterAreaTrigger;
        }

        private void RegisterAreaTrigger(Collider2D enter)
        {
            Cube cube = CubeTransformGlobalDictionary.Get(enter.transform);
            if (cube != null && cube.IsFall())
            {
                ConvertCube(cube);
            }
        }

        private void ConvertCube(Cube cube)
        {
            cube.Hide();
            moneyObject.Value++;
        }
    }
}