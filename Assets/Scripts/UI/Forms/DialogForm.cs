using UnityEngine.UI;
using UnityEngine;
using DBMS.Systems;
using DG.Tweening;
using UnityEngine.Events;
namespace DBMS.UI
{
    public class DialogForm : Form
    {
        public class DialogClickEvent : UnityEvent
        {

        }
        public DialogClickEvent onClick = new DialogClickEvent();
        public Text text;
        public Text title;
        public Button buto;
        public Transform dialog;

        private void Awake()
        {     
        }

        public virtual void Start()
        {
            buto.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.GoBack();
            });
        }

        public void SetDialog(UnityAction closed, string title, string text)
        {
            if (closed != null)
            {
                onClick.AddListener(closed);
            }
            this.title.text = title;
            this.text.text = text;
        }

        protected Tweener DoDialogAnimOpen()
        {
            dialog.localScale = new Vector3(0.8f, 0.8f, 1f);
            return dialog.DOScale(Vector3.one, animationTime);
        }

        public override void OnOpen()
        {
            DoBGAnimOpen();
            DoDialogAnimOpen();
        }
        public override void Close()
        {
            background.material.SetFloat("_Size", 4);
            background.material.DOFloat(0, "_Size", animationTime / 2);
            dialog.localScale = Vector3.one;
            dialog.DOScale(new Vector3(0.8f, 0.8f, 1f), animationTime / 2).OnKill(() =>
              {
                  onClick.Invoke();
                  base.Close();
              });
        }

    }
}
