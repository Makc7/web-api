using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class PilotForCreatonDto
    {
        [Required(ErrorMessage = "Pilot name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Pilot address is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for rhe Address is 60 characte")]
        public string Address { get; set; }
        public IEnumerable<PlaneForCreationDto> Planes { get; set; }
    }
}
