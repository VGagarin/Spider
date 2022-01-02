using UnityEngine;

namespace Game.DataTypes
{
    internal struct CardMoveData
    {
        public Card CardToMove { get; set; }
        public float DelayBeforeMove { get; set; }
        public Vector3 TargetPosition { get; set; }
        public int TargetLayer { get; set; }
        public bool TargetStateIsOpen { get; set; }
        public CardsZone TargetZone { get; set; }
        public int ColumnIndex { get; set; }
    }
}