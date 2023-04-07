using UnityEngine;

namespace Logic.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private const string HorizontalKey = "Horizontal";
        private const string VerticalKey = "Vertical";

        public float Horizontal => SimpleInput.GetAxis(HorizontalKey);
        public float Vertical => SimpleInput.GetAxis(VerticalKey);
    }
}