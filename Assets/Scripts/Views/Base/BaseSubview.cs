using UnityEngine;

namespace Views.Base
{
    internal abstract class BaseSubview<T> : MonoBehaviour where T: IView
    {
        protected T _baseView;
    }
}