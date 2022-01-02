using UnityEngine;
using ViewModels;
using Views.Base;

namespace Views
{
    internal class DealingView : BaseView<DealingViewModel>
    {
        [SerializeField] private Transform _mainZone;
        
        protected override void Initialize()
        {
            _viewModel.MainZonePosition = _mainZone.position;
        }
    }
}