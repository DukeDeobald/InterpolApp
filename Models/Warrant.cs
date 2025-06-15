namespace InterpolApp.Models;

public class Warrant
{
        public int WarrantId { get; set; }
        public string Description { get; set; }
        
        public int SuspectId { get; set; }
        public string SuspectName { get; set; }
}
