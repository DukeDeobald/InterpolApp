using System;

namespace InterpolApp.Models;

public class Suspect
{
    public int WantedPersonId { get; set; }
    public string Forename { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Weight { get; set; }
    public double Height { get; set; }
    public char SexId { get; set; }
    public string CountryOfBirthId { get; set; }
    public string NationalityId { get; set; }
    public string PlaceOfBirth { get; set; }
    public string DistinguishingMarks { get; set; }
    public string HairColor { get; set; }
    public string EyeColor { get; set; }
    public bool IsDead { get; set; }
}
