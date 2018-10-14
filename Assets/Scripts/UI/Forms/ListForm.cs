using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using System.Text;
using DBMS.Systems;
using DG.Tweening;
namespace DBMS.UI
{
    public abstract class ListForm<T> : CanGoBackForm
    {
        public const int pageMaxCount = 20;
        public ScrollRect scrollRect;
        public RectTransform itemPrefabe;
        private SpawnPool pool;
        public Text pageIndexShow;
        public Button prev;
        public Button next;
        private List<T> dataList;
        public RectTransform panel;
        internal int pageIndex;

        private int PageCount
        {
            get
            {
                return (dataList.Count / pageMaxCount) + 1;
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
            dataList = new List<T>();
            prev.onClick.AddListener(() =>
            {
                if (pageIndex == 1 || PageCount == 1)
                {
                    return;
                }
                pageIndex--;
                pageIndexShow.text = pageIndex + " / " + PageCount;
                ClearPageItems();
                AddPageItems(dataList.GetRange(pageIndex * pageMaxCount, pageIndex * pageMaxCount + pageMaxCount));
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
                AddPageItems(dataList.GetRange(pageIndex * pageMaxCount, pageIndex * pageMaxCount + pageMaxCount));
            });
            pageIndexShow.text = pageIndex + " / " + PageCount;
        }

        private IEnumerator AddPageItemWorking(List<T> list)
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
                MachiningData(item, pool.Spawn(itemPrefabe, scrollRect.content));
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

        private void AddPageItems(List<T> personnels)
        {
            StartCoroutine(AddPageItemWorking(personnels));
        }

        public void AddItems(List<T> personnels)
        {
            if (personnels == null)
            {
                return;
            }
            if (personnels.Count + dataList.Count <= pageMaxCount)
            {
                AddPageItems(personnels);
            }
            else if (dataList.Count < pageMaxCount)
            {
                AddPageItems(personnels.GetRange(0, pageMaxCount - dataList.Count));
            }
            dataList.AddRange(personnels);
        }

        public void Clear()
        {
            StopAllCoroutines();
            dataList.Clear();
            ClearPageItems();
        }

        public abstract void ClearItem(Transform v);

        private void ClearPageItems()
        {
            while (scrollRect.content.childCount > 0)
            {
                
                var item = scrollRect.content.GetChild(0);
                ClearItem(item);
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

        public abstract void MachiningData(T value, Transform item);
    }
}
