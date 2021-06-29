using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;

namespace POI.repository.Repositories
{
    public interface IPoiBlogRepository : IGenericRepository<Poiblog>
    {

    }
    public class PoiBlogRepository : GenericRepository<Poiblog>, IPoiBlogRepository
    {
        public PoiBlogRepository(POIContext context) : base(context)
        {

        }
    }
}
