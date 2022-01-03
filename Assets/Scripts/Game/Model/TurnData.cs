using UnityEngine;

namespace Game.Model
{
    internal struct TurnData
    {
        public bool IsTurnAvailable { get; set; }
        public Card Card { get; set; }
        public int SourceColumnId { get; set; }
        public int TargetColumnId { get; set; }
        public int Layer { get; set; }
        public Transform Parent { get; set; }
        public Vector3 Position { get; set; }
    }
}