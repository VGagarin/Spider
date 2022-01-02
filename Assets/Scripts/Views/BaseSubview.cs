using UnityEngine;

namespace Views
{
    internal abstract class BaseSubview<T> : MonoBehaviour where T: IView
    {
        protected T _baseView;
    }
}