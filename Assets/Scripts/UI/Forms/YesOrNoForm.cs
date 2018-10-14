using UnityEngine.UI;
using UnityEngine.Events;
using DBMS.Systems;
namespace DBMS.UI
{
    public class YesOrNoForm : DialogForm
    {
        public Button butx;
        public override void Start()
        {
            base.Start();
            butx.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.GoBack();
            });
        }
        public void SetDialog(UnityAction oaction,UnityAction closed, string title, string text)
        {
            if (oaction != null)
            {
                buto.onClick.AddListener(oaction);
            }
            SetDialog(closed, title, text);
        }
    }
}
