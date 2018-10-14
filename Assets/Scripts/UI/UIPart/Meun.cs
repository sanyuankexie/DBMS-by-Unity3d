namespace DBMS.UI
{
    using global::System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    public abstract class Meun : Dropdown
    {
        protected Dictionary<string, System.Action> actionDir;
        protected override void Awake()
        {
            base.Awake();
            actionDir = new Dictionary<string, System.Action>();
        }

        protected override DropdownItem CreateItem(DropdownItem itemTemplate)
        {
            var item = base.CreateItem(itemTemplate);
            var b = item.GetComponentInChildren<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() =>
            {
                OnCancel(new BaseEventData(EventSystem.current));
                if (actionDir.ContainsKey(item.text.text))
                {
                    actionDir[item.text.text].Invoke();
                }
                else
                {
                    Debug.Log($"{item.text.text} not action");
                }
            });
            return item;
        }
    }
}
