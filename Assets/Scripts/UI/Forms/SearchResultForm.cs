using UnityEngine;
using UnityEngine.UI;
using DBMS.Data.Entity;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using System.Text;
using DBMS.Systems;
using DG.Tweening;
namespace DBMS.UI
{
    public class SearchResultForm : CanGoBackForm
    {
        public const int pageMaxCount = 20;
        public Button Excel;
        public ScrollRect scrollRect;
        public RectTransform itemPrefabe;
        private SpawnPool pool;
        public Text pageIndexShow;
        public Button prev;
        public Button next;
        private List<Personnel> personnelList;
        public RectTransform panel;
        internal int pageIndex;

        private int PageCount
        {
            get
            {
                return (personnelList.Count / pageMaxCount) + 1;
            }
        }

        private int PageItemShowCount
        {
            get
            {
                return scrollRect.content.childCount;
            }
        }

        public override void Awake()
        {
            base.Awake();
            pageIndex = 1;
            pool = Kernel.Current.Desktop.transform.Find("ScrollViewItemPool").GetComponent<SpawnPool>();
            personnelList = new List<Personnel>();
            prev.onClick.AddListener(() =>
            {
                if (pageIndex == 1 || PageCount == 1)
                {
                    return;
                }
                pageIndex--;
                pageIndexShow.text = pageIndex + " / " + PageCount;
                ClearPageItems();
                AddPageItems(personnelList.GetRange(pageIndex * pageMaxCount, pageIndex * pageMaxCount + pageMaxCount));
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
                AddPageItems(personnelList.GetRange(pageIndex * pageMaxCount, pageIndex * pageMaxCount + pageMaxCount));
            });
            Excel.onClick.AddListener(() =>
            {
                Kernel.Current.Excel.SaveToExcel(personnelList, () =>
                {
                    Kernel.Current.Desktop.OpenNew<DialogForm>().SetDialog(null, "写入完成", "Excel已经构建.");
                });
            });
            pageIndexShow.text = pageIndex + " / " + PageCount;
        }

        private IEnumerator AddPageItemWorking(List<Personnel> list)
        {
            if (list.Count > pageMaxCount)
            {
                Debug.LogError("max!!! " + list.Count);
            }
            Dictionary<int, string> buffer1 = new Dictionary<int, string>();
            Dictionary<int, string> buffer2 = new Dictionary<int, string>();
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append("编号: ");
                sb.Append(item.ID);
                sb.Append(" ");
                sb.Append("姓名: ");
                sb.Append(item.Name);
                sb.Append(" ");
                sb.Append("学历: ");
                sb.Append(item.Education);
                sb.Append(" ");
                sb.Append("部门: ");
                if (buffer1.ContainsKey(item.DepartmentID))
                {
                    sb.Append(buffer1[item.DepartmentID]);
                }
                else
                {
                    buffer1.Add(item.DepartmentID, Kernel.Current.Sql.LoadEntity<Department>(item.DepartmentID).Name);
                    sb.Append(buffer1[item.DepartmentID]);
                }
                sb.Append(" ");
                sb.Append("职位: ");
                if (buffer2.ContainsKey(item.PositionID))
                {
                    sb.Append(buffer2[item.PositionID]);
                }
                else
                {
                    buffer2.Add(item.PositionID, Kernel.Current.Sql.LoadEntity<Position>(item.PositionID).Name);
                    sb.Append(buffer2[item.PositionID]);
                }
                sb.Append(" ");
                sb.Append("民族: ");
                sb.Append(item.Nation);
                sb.Append(" ");
                var pitem = pool.Spawn(itemPrefabe, scrollRect.content).GetComponent<PageItem>();
                pitem.SetItem(sb.ToString(), item);
                sb.Clear();
                yield return new WaitForSeconds(0.05f);
            }
        }

        public override void OnOpen()
        {
            Kernel.Current.Desktop.topBar.SetTopBarItem(false);
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

        private void AddPageItems(List<Personnel> personnels)
        {
            StartCoroutine(AddPageItemWorking(personnels));
        }

        public void AddItems(List<Personnel> personnels)
        {
            if (personnels == null)
            {
                return;
            }
            if (personnels.Count + personnelList.Count <= pageMaxCount)
            {
                AddPageItems(personnels);
            }
            else if (personnelList.Count < pageMaxCount)
            {
                AddPageItems(personnels.GetRange(0, pageMaxCount - personnelList.Count));
            }
            personnelList.AddRange(personnels);            
        }

        public void Clear()
        {
            StopAllCoroutines();
            personnelList.Clear();
            ClearPageItems();
        }

        private void ClearPageItems()
        {
            while (scrollRect.content.childCount > 0)
            {
                var item = scrollRect.content.GetChild(0);
                item.GetComponent<PageItem>().Clear();
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
                Kernel.Current.Desktop.topBar.SetTopBarItem(true);
                base.Close();
            });
        }
    }

}
