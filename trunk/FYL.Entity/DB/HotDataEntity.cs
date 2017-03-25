using System;

namespace FYL.Entity.DB
{
    [Serializable]
    public class HotDataEntity
    {
        public int ItemId { get; set; }
        public int ModuleId { get; set; }
        public int Sequence { get; set; }
        public string XML { get; set; }
        public DateTime? ModiTime { get; set; }
        public DateTime? Created { get; set; }
        public int PV { get; set; }
    }
}
