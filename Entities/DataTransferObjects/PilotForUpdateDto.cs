namespace Entities.DataTransferObjects
{
    public class PilotForUpdateDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public IEnumerable<PlaneForCreationDto> Plans { get; set; }
    }
}
