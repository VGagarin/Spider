using Models.Base;
using Models.GameZones;
using UnityEngine;
using ViewModels.Base;

namespace ViewModels
{
    internal class GameZonesViewModel : BaseViewModel
    {
        private GameZonesPointsModel _gameZonesPointsModel;
        
        public GameZonesViewModel()
        {
            _gameZonesPointsModel = ModelRepository.GetModel<GameZonesPointsModel>();
        }
        
        public void SetColumnPoints(Transform[] points)
        {
            _gameZonesPointsModel.SetMainZonePoints(points);
        }
        
        public void SetWaitingZonePoint(Transform point)
        {
            _gameZonesPointsModel.SetWaitingZonePoint(point);
        }

        public void SetDiscardZonePoint(Transform point)
        {
            _gameZonesPointsModel.SetDiscardZonePoint(point);
        }
    }
}