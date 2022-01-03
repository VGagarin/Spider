using UnityEngine;

namespace Game.Model
{
    internal struct CardMoveData
    {
        public Card CardToMove { get; set; }
        public float DelayBeforeMove { get; set; }
        public Vector3 TargetPosition { get; set; }
        public int TargetLayer { get; set; }
        public bool TargetStateIsOpen { get; set; }
        public CardsZone SourceZone { get; set; }
        public CardsZone TargetZone { get; set; }
        public int ColumnId { get; set; }
        public Transform TargetParent { get; set; }
        public bool IsLocalMove { get; set; }
    }
}