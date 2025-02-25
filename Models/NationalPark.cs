namespace NationalParksApi.Models
{
    public class NationalPark
    {
        // Parameterless constructor for EF Core
        public NationalPark()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public int YearEstablished { get; set; }

        public string Latitude {  get; set; }
        public string Longitude { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}