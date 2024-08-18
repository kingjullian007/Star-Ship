using UnityEngine;

namespace _StarShip
{
    public enum Direction
    {
        Left,
        Right,
        Down,
        Up,
        LeftTop,
        LeftDown,
        RightTop,
        RightDow,
        None
    }
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        public Vector2 startPos;
        public Vector2 directionVec;

        public Direction direction;

        public float touchX;
        public float touchY;

        public float sensitivityX;
        public float sensitivityY;

        private bool isPressed;
        private Vector3 prePos;

        private void Awake()
        {
            if (!Instance) Instance = this;
                direction = Direction.None;
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                prePos = Input.mousePosition;
                isPressed = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isPressed = false;
                touchX = 0;
                touchY = 0;
            }
  
            if (isPressed)
            {
                Vector3 delta = Input.mousePosition - prePos;
                if (delta.magnitude != 0)
                {
                    Vector2 touchDeltaPosition = delta;
                    touchX = touchDeltaPosition.x;
                    touchY = touchDeltaPosition.y;
                    touchX = Mathf.Clamp(touchX / sensitivityX, -1.0f, 1.0f);
                    touchY = Mathf.Clamp(touchY / sensitivityY, -1.0f, 1.0f);
                    prePos = Input.mousePosition;
                }
                else
                {
                    touchX = 0;
                    touchY = 0;
                }
            }
        }
    }
}

