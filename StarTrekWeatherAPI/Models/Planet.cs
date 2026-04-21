using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("planets")]
public class Planet
{
    [Key]
    [Column("name")]
    public string Name { get; set; } = "";

    [Column("solar_system")]
    public string SolarSystem { get; set; } = "";

    [Column("atmospheric_pressure")]
    public float AtmosphericPressure { get; set; }

    [Column("max_temp")]
    public float MaxTemp { get; set; }

    [Column("min_temp")]
    public float MinTemp { get; set; }

    [Column("description")]
    public string Description { get; set; } = "";
}