using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using DBMS.Systems;
namespace DBMS.UI
{
    public class AdvancedToolsForm : CanGoBackForm
    {
        public Button postionm;
        public Button departmentm;
        public Button titlem;
        public RectTransform panel;

        public override void Awake()
        {
            base.Awake();
            departmentm.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.OpenNew<DListForm>();
            });
        }
        public override void OnOpen()
        {
            Kernel.Current.Desktop.topBar.SetTopBarItem(false);
            DoAnimOpen();
            DoBGAnimOpen();
        }

        public override void Close()
        {
            DoBGAnimClose();
            DoAnimClose().OnKill(() =>
            {
                Kernel.Current.Desktop.topBar.SetTopBarItem(true);
                base.Close();
            });
        }

        Tweener DoAnimClose()
        {
            panel.anchoredPosition = Vector2.zero;
            return panel.DOAnchorPos(new Vector2(0,500),animationTime);
        }

        Tweener DoAnimOpen()
        {
            panel.anchoredPosition = new Vector2(0, 500);
            return panel.DOAnchorPos(Vector2.zero, animationTime);
        }

    }
}
