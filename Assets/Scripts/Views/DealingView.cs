using UnityEngine;
using ViewModels;

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