﻿using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class PiotForCreatonDto
    {
        [Required(ErrorMessage = "Piot name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Piot address is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for rhe Address is 60 characte")]
        public string Address { get; set; }
        public IEnumerable<PlaneForCreationDto> Planes { get; set; }
    }
}
