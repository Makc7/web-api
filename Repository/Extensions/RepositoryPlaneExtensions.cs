using Entities.Models;
using System.Linq.Dynamic.Core;
using Repository.Extensions.Utility;

namespace Repository.Extensions
{
    public static class RepositoryPlaneExtensions
    {

        public static IQueryable<Plane> FilterPlane(this IQueryable<Plane> Plane, string firstPlaneBrend, string lastPlaneBrend) =>
                Plane.Where(e => (e.Brend[0] >= firstPlaneBrend[0] && e.Brend[0] <= lastPlaneBrend[0]));
        public static IQueryable<Plane> Search(this IQueryable<Plane> Plane, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Plane;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return Plane.Where(e => e.Brend.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Plane> Sort(this IQueryable<Plane> Plane, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return Plane.OrderBy(e => e.Brend);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Plane>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderQuery))
                return Plane.OrderBy(e => e.Brend);
            return Plane.OrderBy(orderQuery);
        }
    }
}