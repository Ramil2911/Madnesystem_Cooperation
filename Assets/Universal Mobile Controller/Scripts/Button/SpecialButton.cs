using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
namespace UniversalMobileController { 
    public class SpecialButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public UnityEvent onButtondown;
        public UnityEvent onButtonup;
        public bool isPressed;
        [SerializeField] private State buttonState;
        
        private int _isDown;

        public bool isDown
        {
            get
            {
                if (_isDown > 0)
                {
                    _isDown--;
                    return true;
                }
                return false;
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
            if(_isDown == 0) _isDown++;
            onButtondown.Invoke();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isPressed = false;
            onButtonup.Invoke();
        }

        public void SetState(State state)
        {
            buttonState = state;
        }
        public State GetState()
        {
            return buttonState;
        }
    }
}
