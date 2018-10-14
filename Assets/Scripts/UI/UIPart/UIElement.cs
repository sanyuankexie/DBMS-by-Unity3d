using System.Linq;
using UnityEngine;
namespace DBMS.UI
{

    public abstract class UIElement : MonoBehaviour
    {
        public RectTransform rectTransform
        {
            get
            {
                return transform as RectTransform;
            }
        }

        protected const float animationTime = 0.5f;

        public T GetChildComponent<T>(string name)
        {
            return transform.Find(name).GetComponent<T>();
        }

    }
}
