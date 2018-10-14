using UnityEngine;
using UnityEngine.UI;
using DBMS.Systems;
using UnityEngine.Events;
using System.Collections;
using DBMS.Data.Entity;
using PathologicalGames;
using System.Collections.Generic;
using DG.Tweening;
using System.Text;
namespace DBMS.UI
{
    public class JournalForm : CanGoBackForm
    {
        public const int pageMaxCount = 20;
        public RectTransform itemPrefabe;
        public Button Turnover;
        public Button Entry;
        public RectTransform panel;
        public ScrollRect scrollRect;
        public Text pageIndexShow;
        public Button prev;
        public Button next;

        private List<IJournalRecord> records;
        private int PageCount
        {
            get
            {
                return (records.Count / pageMaxCount) + 1;
            }
        }
        private int PageItemShowCount
        {
            get
            {
                return scrollRect.content.childCount;
            }
        }
        private SpawnPool pool;
        private int pageIndex;

        public override void Awake()
        {
            base.Awake();
            pageIndex = 1;
            pool = Kernel.Current.Desktop.transform.Find("ScrollViewItemPool").GetComponent<SpawnPool>();
            UnityAction entryAction = () =>
            {
                SetItems(new List<IJournalRecord>(Kernel.Current.Sql.Query<EntryRecord>(null)));
            };
            prev.onClick.AddListener(() =>
            {
                if (pageIndex == 1 || PageCount == 1)
                {
                    return;
                }
                pageIndex--;
                pageIndexShow.text = pageIndex + " / " + PageCount;
                ClearPageItems();
                AddPageItems(records.GetRange(pageIndex * pageMaxCount, pageIndex * pageMaxCount + pageMaxCount));
            });
            next.onClick.AddListener(() =>
            {
                if (pageIndex == PageCount || pageIndex == 1)
                {
                    return;
                }
                pageIndex++;
                pageIndexShow.text = pageIndex + " / " + PageCount;
                ClearPageItems();
                AddPageItems(records.GetRange(pageIndex * pageMaxCount, pageIndex * pageMaxCount + pageMaxCount));
            });
            Entry.onClick.AddListener(entryAction);
            Turnover.onClick.AddListener(() =>
            {
                SetItems(new List<IJournalRecord>(Kernel.Current.Sql.Query<TurnoverRecord>(null)));
            });
            entryAction();
        }

        private void AddPageItems(List<IJournalRecord> list)
        {
            StartCoroutine(AddPageItemWorking(list));
        }

        public void SetItems(List<IJournalRecord> records)
        {
            if (records == null)
            {
                return;
            }
            Clear();
            this.records = records;
            foreach (var item in records)
            {
                Debug.Log(item.Info);
            }
            StartCoroutine(AddPageItemWorking(records));
            pageIndex = 1;
            pageIndexShow.text = pageIndex + " / " + PageCount;
        }

        private IEnumerator AddPageItemWorking(List<IJournalRecord> list)
        {
            if (list.Count > pageMaxCount)
            {
                Debug.LogError("max!!! " + list.Count);
            }
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append("员工号: ");
                sb.Append(item.PersonnelID);
                sb.Append(' ');
                sb.Append("时间: ");
                sb.Append(item.Time);
                sb.Append(' ');
                if (item.GetType().IsAssignableFrom(typeof(EntryRecord)))
                {
                    sb.Append("入职原因: ");
                }
                else
                {
                    sb.Append("离职原因: ");
                }
                sb.Append(item.Info);
                var text= pool.Spawn(itemPrefabe, scrollRect.content).Find("Text").GetComponent<Text>();
                text.text = sb.ToString();
                sb.Clear();
                yield return new WaitForSeconds(0.05f);
            }
        }

        public override void OnOpen()
        {
            DoBGAnimOpen();
            DoPanelAnimOpen();
        }

        protected Tweener DoPanelAnimOpen()
        {
            //DoAllGraphicOpen();
            Image image = scrollRect.GetComponent<Image>();
            image.color = new Color(1, 1, 1, 0);
            image.DOColor(new Color(1, 1, 1, 100f / 255f), animationTime);
            panel.localScale = new Vector3(0.8f, 0.8f, 1);
            return panel.DOScale(Vector3.one, animationTime);
        }

        protected Tweener DoPanelAnimClose()
        {
            //DoAllGraphicClose();
            Image image = scrollRect.GetComponent<Image>();
            image.color = new Color(1, 1, 1, 100f / 255f);
            image.DOColor(new Color(1, 1, 1, 0), animationTime);
            panel.localScale = Vector3.one;
            return panel.DOScale(new Vector3(0.8f, 0.8f, 1), animationTime);
        }

        public void Clear()
        {
            StopAllCoroutines();
            ClearPageItems();
            records = null;
        }

        private void ClearPageItems()
        {
            while (scrollRect.content.childCount > 0)
            {
                var item = scrollRect.content.GetChild(0);
                item.Find("Text").GetComponent<Text>().text = string.Empty;
                item.SetParent(pool.transform);
                pool.Despawn(item);
            }

        }

        public override void Close()
        {
            Clear();
            DoBGAnimClose();
            DoPanelAnimClose().OnKill(() =>
            {
                base.Close();
            });
        }
    }
}
