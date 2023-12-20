using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Plane
    {
        [Column("PlaneId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Plane brend is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Brend { get; set; }
        [Required(ErrorMessage = "Plane model is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for rhe Address is 30 characte")]
        public string Model { get; set; }

        [ForeignKey(nameof(Pilot))]
        public Guid PilotId { get; set; }
        public Pilot Pilot { get; set; }

    }
}
