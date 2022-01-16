namespace Game.Model
{
    internal struct TurnData
    {
        public bool IsTurnInitiatedFromUser { get; set; }
        public GameZoneType SourceZoneType { get; set; }
        public GameZoneType TargetZoneType { get; set; }
        public Card SourceCard { get; set; }
        public Card TargetCard { get; set; }
        public Card? AdditionalCardToOpen { get; set; }
    }
}