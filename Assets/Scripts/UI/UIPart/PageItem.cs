using UnityEngine;
using UnityEngine.UI;
using DBMS.Data.Entity;
using DBMS.Systems;
namespace DBMS.UI
{
    public class PageItem : MonoBehaviour
    {
        public void SetItem(string show,Personnel personnel)
        {
            text.text = show;
            this.personnel = personnel;            
        }
        public Button button;
        public Text text;
        public Personnel personnel;
        private void Awake()
        {
            button.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.OpenNew<InfoForm>().SetData(personnel);
            });
        }
        public void Clear()
        {
            text.text = string.Empty;
            personnel = null;
        }
    }

}
