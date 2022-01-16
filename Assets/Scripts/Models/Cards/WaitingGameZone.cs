using Game.Model;

namespace Models.Cards
{
    internal class WaitingGameZone : SimpleGameZone
    {
        public override GameZoneType ZoneType => GameZoneType.Waiting;
    }
}