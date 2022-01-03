using UnityEngine;

namespace Game.Model
{
    internal struct CardInputData
    {
        public int CardId { get; set; }
        public int ColumnId { get; set; }
        public Vector3 Position { get; set; }
    }
}