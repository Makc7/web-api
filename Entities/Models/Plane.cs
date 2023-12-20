using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Plane
    {
        [Column("PlaneID")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = " name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for a name is 60 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = " name a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for an address is 60 characters")]
        public string Make { get; set; }
        public string Country { get; set; }
        public ICollection<Plane> Planes { get; set; }
    }
}
