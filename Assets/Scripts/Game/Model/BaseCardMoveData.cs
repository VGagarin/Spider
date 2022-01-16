namespace Game.Model
{
    internal struct BaseCardMoveData
    {
        public int CardId { get; set; }
        public float DelayBeforeMove { get; set; }
        public int ColumnId { get; set; }
        public GameZoneType SourceZoneType { get; set; }
        public GameZoneType TargetZoneType { get; set; }
        public bool TargetStateIsOpen { get; set; }
    }
}