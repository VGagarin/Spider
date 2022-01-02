using System;
using UnityEngine;
using ViewModels;

namespace Views
{
    internal abstract class BaseView<T> : MonoBehaviour, IView where T : BaseViewModel, new()
    {
        protected T _viewModel;
        
        protected void Awake()
        {
            if (_viewModel != null)
                throw new Exception("ViewModel already created");
            
            _viewModel = new T();
        }
    }
}