using System;

namespace Game.Model
{
    internal struct CardMoveData
    {
        public int CardToMoveId { get; set; }
        public float DelayBeforeMove { get; set; }
        public int TargetLayer { get; set; }
        public GameZoneType SourceZoneType { get; set; }
        public GameZoneType TargetZoneType { get; set; }
        public int ColumnId { get; set; }
        public int RowId { get; set; }
        
        public Action MoveCompleted;
    }
}