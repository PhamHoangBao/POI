using System;
using System.Collections.Generic;
using System.Text;

namespace POI.repository.ViewModels
{
    public class CreatePoiTypeViewModel
    {
        public string Name { get; set; }
    }

    public class UpdatePoiTypeViewModel
    {
        public Guid PoitypeId { get; set; }
        public string Name { get; set; }
    }
}
