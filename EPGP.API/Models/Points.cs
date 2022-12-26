namespace EPGP.API.Models
{
    public class Points
    {
        public int RaiderId { get; set; }

        public decimal EffortPoints { get; set; }

        public decimal GearPoints { get; set; }

        public decimal Priority => GearPoints == 0 ? 0 : EffortPoints / GearPoints;
    }
}
