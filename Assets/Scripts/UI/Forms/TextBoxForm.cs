using UnityEngine;
using UnityEngine.UI;
using DBMS.Systems;
using System;
using UnityEngine.Events;
using DG.Tweening;

namespace DBMS.UI
{
    public class TextBoxForm : Form
    {
        public Text title;
        public InputField input;
        public Button submit;
        public Button cancel;
        public RectTransform panel;
        private void Awake()
        {
            submit.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.GoBack();
            });
            cancel.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.GoBack();
            });
        }
        public void SetCallback(string title, UnityAction<string> submit, UnityAction cancel)
        {
            if (title != null)
            {
                this.title.text = title;
            }
            if (submit != null)
            {
                this.submit.onClick.AddListener(() => submit.Invoke(input.text));
            }
            if (cancel != null)
            {
                this.cancel.onClick.AddListener(cancel);
            }
        }
        protected Tweener DoPanelOpen()
        {
            panel.localScale = new Vector3(0.8f, 0.8f, 1);
            return panel.DOScale(Vector3.one, animationTime);
        }
        protected Tweener DoPanelClose()
        {
            panel.localScale = Vector3.one;
            return panel.DOScale(new Vector3(0.8f, 0.8f, 1), animationTime);
        }
        public override void Close()
        {
            DoBGAnimClose();
            DoPanelClose().OnKill(() => base.Close());
        }
        public override void OnOpen()
        {
            DoBGAnimOpen();
            DoPanelOpen();
        }
    }
}
