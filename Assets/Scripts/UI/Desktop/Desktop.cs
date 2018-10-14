namespace DBMS.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using DBMS.Systems;
    using global::System.Collections.Generic;

    public class Desktop : MonoBehaviour
    {
        public GameObject[] formsPrefbe;
        public Image background;
        public RectTransform formSpace;
        private Stack<Form> uiStack;
        public TopBar topBar;
        private void Awake()
        {
            uiStack = new Stack<Form>();
        }
        public T OpenNew<T>() where T : Form
        {
            foreach (var item in formsPrefbe)
            {
                if (item.GetComponent<Form>() is T)
                {
                    T r = Instantiate(item, formSpace).GetComponent<T>();                                        
                    r.transform.SetAsLastSibling();
                    r.OnOpen();
                    uiStack.Push(r);
                    return r;
                }
            }
            Debug.LogError(typeof(T).Name + " match faild!!");
            return null;
        }
        public void GoBack()
        {
            uiStack.Pop().Close();
        }
        private void Start()
        {
#if !UNITY_EDITOR
            if (PlayerPrefs.HasKey("Desktop"))
            {
                background.sprite = Kernel.Current.Image.Load(PlayerPrefs.GetString("Desktop"), 1400, 900);
            }
#endif
        }

    }
}
