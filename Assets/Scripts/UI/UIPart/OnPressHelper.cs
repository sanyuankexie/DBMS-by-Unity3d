using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace DBMS.UI
{
    public class OnPressHelper : EventTrigger
    {
        public class OnPressEvent : UnityEngine.Events.UnityEvent
        {
           
        }
        public OnPressEvent onPress;
        private void Awake()
        {
            onPress = new OnPressEvent();
        }
        private bool isPointDown = false;
        public override void OnPointerDown(PointerEventData eventData)
        {
            isPointDown = true;
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            isPointDown = false;                            //鼠标移出按钮时推出长按状态  
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            isPointDown = false;
        }
        private void Update()
        {
            if (isPointDown)
            {
                onPress.Invoke();
            }
        }
    }
}
