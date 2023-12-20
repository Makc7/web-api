namespace Entities.RequestFeatures
{
    public class PlaneParameters: RequestParameters
    {
        public PlaneParameters() 
        {
            OrderBy = "brend";
        }

        public string FirstPlaneBrand { get; set; } = "A";
        public string LastPlaneBrand { get; set; } = "Z";
        public string SearchTerm { get; set; }

    }
}
