using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using POI.repository.Entities;
using POI.repository.IRepositories;
using Microsoft.EntityFrameworkCore;


namespace POI.repository.Repositories
{
    public interface IVisitRepository : IGenericRepository<Visit>
    {
    }
    public class VisitRepository : GenericRepository<Visit> , IVisitRepository
    {
        public VisitRepository(POIContext context) : base(context)
        {

        }
    }
}
