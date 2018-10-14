using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DBMS.Utils;
using DG.Tweening;
using DBMS.Systems;

namespace DBMS.UI
{
    public class TopBar : UIElement
    {
        public Button min;
        public Button close;
        public RectTransform fastSearch;
        public Button fastAdd;
        public Text title;

        private void Awake()
        {
            min.onClick.AddListener(() => Win32API.ShowMinWindow());
            close.onClick.AddListener(() =>
            {
                Application.Quit();
            });
            GetComponent<OnPressHelper>().onPress.AddListener(() =>
             {
#if !UNITY_EDITOR
                 Win32API.PanelTitleMouseDown();
#endif
             });
            fastAdd.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.OpenNew<InfoForm>().SetWorkMode(InfoFormWorkMode.CreateNew);
                SetTopBarItem(false);
            });
        }

        public void SetTopBarItem(bool b)
        {
            SetFastAdd(b);
            SetSearchBar(b);
        }

        public void SetSearchBarActive(bool b)
        {
            switch (Kernel.Current.LoginType)
            {
                case Data.Entity.LoginType.DataEntryOnly:
                    {
                        fastSearch.transform.Find("SearchMenu").GetComponent<SearchMenu>().interactable = false;
                        fastSearch.transform.Find("InputField").GetComponent<InputField>().interactable = false;
                    }
                    break;
                default:
                    {
                        fastSearch.transform.Find("SearchMenu").GetComponent<SearchMenu>().interactable = b;
                        fastSearch.transform.Find("InputField").GetComponent<InputField>().interactable = b;
                    }
                    break;
            }
        }

        public void SetFastAddActive(bool b)
        {
            fastAdd.GetComponent<Button>().interactable = b;
        }

        public void SetTitle(bool b)
        {
            if (b)
            {
                title.gameObject.SetActive(b);
                DoTileAnimOpen();
            }
            else
            {
                DoTileAnimClose().OnKill(() =>
                {
                    title.gameObject.SetActive(b);
                });
            }
        }

        public void SetSearchBar(bool b)
        {
            if (b)
            {
                fastSearch.gameObject.SetActive(b);
                DoSeachBarAnimOpen();
            }
            else
            {
                DoSeachBarAnimClose().OnKill(() =>
                {
                    fastSearch.gameObject.SetActive(b);
                });
            }

        }

        public void SetFastAdd(bool b)
        {
            if (b)
            {
                fastAdd.gameObject.SetActive(b);
                DoFastAddOpen();
            }
            else
            {
                DoFastAddClose().OnKill(() =>
                {
                    fastAdd.gameObject.SetActive(b);
                });
            }
        }

        protected Tweener DoFastAddOpen()
        {
            SetFastAddActive(false);
            fastAdd.GetRectTransform().anchoredPosition = new Vector2(fastAdd.GetRectTransform().anchoredPosition.x, 36.6f);
            return fastAdd.GetRectTransform().DOAnchorPosY(-3.8147e-06f, animationTime).OnKill(() => { SetFastAddActive(true); });
        }

        protected Tweener DoFastAddClose()
        {
            SetFastAddActive(false);
            fastAdd.GetRectTransform().anchoredPosition = new Vector2(fastAdd.GetRectTransform().anchoredPosition.x, -3.8147e-06f);
            return fastAdd.GetRectTransform().DOAnchorPosY(36.6f, animationTime);
        }

        protected Tweener DoTileAnimOpen()
        {
            title.color = new Color(1, 1, 1, 0);
            return title.DOColor(new Color(1, 1, 1, 1), animationTime);
        }

        protected Tweener DoTileAnimClose()
        {
            title.color = new Color(1, 1, 1, 1);
            return title.DOColor(new Color(1, 1, 1, 0), animationTime);
        }

        protected Tweener DoSeachBarAnimOpen()
        {
            SetSearchBarActive(false);
            fastSearch.anchoredPosition = new Vector2(fastSearch.anchoredPosition.x, 36.6f);
            return fastSearch.DOAnchorPosY(-3.8147e-06f, animationTime).OnKill(() => SetSearchBarActive(true));
        }

        protected Tweener DoSeachBarAnimClose()
        {
            SetSearchBarActive(false);
            fastSearch.anchoredPosition = new Vector2(fastSearch.anchoredPosition.x, -3.8147e-06f);
            return fastSearch.DOAnchorPosY(36.6f, animationTime);
        }
    }
}
