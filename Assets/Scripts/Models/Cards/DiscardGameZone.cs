using Game.Model;

namespace Models.Cards
{
    internal class DiscardGameZone : SimpleGameZone
    {
        public override GameZoneType ZoneType => GameZoneType.Discard;
    }
}