using Entities.Models;
using System.Linq.Dynamic.Core;
using Repository.Extensions.Utility;

namespace Repository.Extensions
{
    public static class RepositoryPlaneExtensions
    {

        public static IQueryable<Plane> FilterPlane(this IQueryable<Plane> planes, string firstPlaneBrend, string lastPlaneBrend) =>
                planes.Where(e => (e.Brend[0] >= firstPlaneBrend[0] && e.Brend[0] <= lastPlaneBrend[0]));
        public static IQueryable<Plane> Search(this IQueryable<Plane> planes, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return planes;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return planes.Where(e => e.Brend.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Plane> Sort(this IQueryable<Plane> planes, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return planes.OrderBy(e => e.Brend);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Plane>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderQuery))
                return planes.OrderBy(e => e.Brend);
            return planes.OrderBy(orderQuery);
        }
    }
}