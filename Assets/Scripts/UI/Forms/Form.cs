using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Events;
namespace DBMS.UI
{
    public abstract class Form : UIElement
    {
        public Image background;

        protected Tweener DoBGAnimOpen()
        {
            background.material.SetFloat("_Size", 0);
            return background.material.DOFloat(4, "_Size", animationTime);
        }

        protected Tweener DoBGAnimClose()
        {
            background.material.SetFloat("_Size", 4);
            return background.material.DOFloat(0, "_Size", animationTime);
        }


        public virtual void OnOpen()
        {


        }

        public virtual void Close()
        {
            Destroy(gameObject);
        }


    }

}
