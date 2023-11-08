using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class BookAvailabilityDto
{
    [Column(TypeName = "uniqueidentifier")]
    public bool Available { get; set; }
}