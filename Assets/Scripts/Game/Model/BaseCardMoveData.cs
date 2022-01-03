namespace Game.Model
{
    internal struct BaseCardMoveData
    {
        public int CardId { get; set; }
        public float DelayBeforeMove { get; set; }
        public int ColumnId { get; set; }
        public CardsZone SourceZone { get; set; }
        public CardsZone TargetZone { get; set; }
        public bool TargetStateIsOpen { get; set; }
    }
}