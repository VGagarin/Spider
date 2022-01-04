using Models.Base;
using Models.GameZones;
using UnityEngine;
using ViewModels.Base;

namespace ViewModels
{
    internal class GameZonesViewModel : BaseViewModel
    {
        private GameZonesModel _gameZonesModel;
        
        public GameZonesViewModel()
        {
            _gameZonesModel = ModelRepository.GetModel<GameZonesModel>();
        }
        
        public void SetColumnPoints(Transform[] points)
        {
            _gameZonesModel.SetMainZonePoints(points);
        }
        
        public void SetWaitingZonePoint(Transform point)
        {
            _gameZonesModel.SetWaitingZonePoint(point);
        }

        public void SetDiscardZonePoint(Transform point)
        {
            _gameZonesModel.SetDiscardZonePoint(point);
        }
    }
}