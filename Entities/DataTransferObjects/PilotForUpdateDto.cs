namespace Entities.DataTransferObjects
{
    public class PiotForUpdateDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public IEnumerable<PlaneForCreationDto> Planes { get; set; }
    }
}
