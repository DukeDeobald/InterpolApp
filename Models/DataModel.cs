using System.Collections.Generic;

namespace InterpolApp.Models;

public class DataModel
{
    public List<Suspect> Suspects { get; set; }
    public List<Suspect> ArchivedSuspects { get; set; }
    public List<Warrant> Warrants { get; set; }
    public List<CriminalGroup> CriminalGroups { get; set; }
    public int LastWantedPersonId { get; set; }
    public int LastWarrantId { get; set; }
    public int LastGroupId { get; set; }
}
