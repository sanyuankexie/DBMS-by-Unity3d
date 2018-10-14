namespace DBMS.UI
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using PathologicalGames;
    using DG.Tweening;
    using DBMS.Systems;
    using System.Collections;
    using DBMS.Utils;
    public class BarGraph : MonoBehaviour
    {
        public ScrollRect scrollRect;
        public RectTransform itemPrefabe;
        private SpawnPool pool;
        private void Awake()
        {
            pool = Kernel.Current.Desktop.transform.Find("ScrollViewItemPool").GetComponent<SpawnPool>();
        }
        public void SetValues(List<KeyValuePair<string,float>> list,float count)
        {
            Clear();
            StartCoroutine(Work(list,count));
        }
        private IEnumerator Work(List<KeyValuePair<string, float>> list,float count)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var item = pool.Spawn(itemPrefabe, scrollRect.content);
                var num = item.Find("Num").GetComponent<Text>();
                num.text = list[i].Value.ToString() + "人";
                var img = item.Find("Bar").GetComponent<Image>();
                img.fillAmount = 0;
                img.DOFillAmount(list[i].Value/count, 0.25f);
                img.color = new Color(1, 192f / 255f, 61f / 255f, 0);
                img.DOColor(new Color(1, 192f / 255f, 61f / 255f, 1),0.25f);
                item.Find("Text").GetComponent<Text>().text = list[i].Key;
                yield return new WaitForSeconds(0.25f);
            }
        }
        public void Clear()
        {
            StopAllCoroutines();
            while (scrollRect.content.childCount > 0)
            {
                var item = scrollRect.content.GetChild(0);
                item.SetParent(pool.transform);
                pool.Despawn(item);
            }
        }
    }
}
