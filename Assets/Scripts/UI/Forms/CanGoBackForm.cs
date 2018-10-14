using UnityEngine.UI;
using DBMS.Systems;
using UnityEngine;
namespace DBMS.UI
{
    public abstract class CanGoBackForm : Form
    {
        public Button goBack;

        public virtual void Awake()
        {
            goBack.onClick.AddListener(() => Kernel.Current.Desktop.GoBack());
        }
    }

}
