using System;
using System.Collections.Generic;
using System.Text;

namespace POI.repository.ViewModels
{
    public class CreateDesTypeViewModel
    {
        public string Name { get; set; }
    }

    public class UpdateDesTypeViewModel
    {
        public Guid DestinationTypeId { get; set; }
        public string Name { get; set; }
    }
}
