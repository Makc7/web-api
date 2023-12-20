namespace Entities.DataTransferObjects
{
    public class pilotForCreatonDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public IEnumerable<PlaneForCreationDto> Plans { get; set; }
    }
}
