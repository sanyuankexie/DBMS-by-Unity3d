using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using DBMS.Systems;
namespace DBMS.UI
{
    public class MainForm : Form
    {
        public Button advancedTools;//高级功能
        public Button statistical;//统计
        public Button changeInformation;//变更记录
        public Button detailedinquiry;//缺省查询
        public RectTransform infoBox;
        private void Awake()
        {
            statistical.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.OpenNew<StatisticsForm>();
            });
            detailedinquiry.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.OpenNew<QueryForm>();
            });
            changeInformation.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.OpenNew<JournalForm>();
            });
            advancedTools.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.OpenNew<AdvancedToolsForm>();
            });
            switch (Kernel.Current.LoginType)
            {
                case Data.Entity.LoginType.Administrator:
                    {
                        statistical.interactable = true;
                        detailedinquiry.interactable = true;
                        changeInformation.interactable = true;
                        advancedTools.interactable = true;
                    }
                    break;
                case Data.Entity.LoginType.DataEntryOnly:
                    {
                        statistical.interactable = false;
                        detailedinquiry.interactable = false;
                        changeInformation.interactable = false;
                        advancedTools.interactable = false;
                    }
                    break;
                case Data.Entity.LoginType.DataMaintainer:
                    {
                        statistical.interactable = true;
                        detailedinquiry.interactable = true;
                        changeInformation.interactable = true;
                        advancedTools.interactable = false;
                    }
                    break;
                default:
                    break;
            }
        }

        public override void OnOpen()
        {
            Kernel.Current.Desktop.topBar.SetFastAdd(true);
            Kernel.Current.Desktop.topBar.SetSearchBar(true);
            Kernel.Current.Desktop.topBar.SetTitle(false);
            DoAnimOpen();
        }
        protected Tweener DoAnimOpen()
        {
            rectTransform.localScale = new Vector3(1.5f, 1.5f, 1);
            return rectTransform.DOScale(Vector3.one, animationTime / 2);
        }
        public override void Close()
        {
            Debug.LogError("主界面不能退出!!");
        }
    }
}
