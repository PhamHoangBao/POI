using System;
using System.Collections.Generic;
using System.Text;

namespace POI.repository.ViewModels
{
    public class CreateProvinceViewModel
    {
        public string Name { get; set; }
    }

    public class UpdateProvinceViewModel
    {
        public Guid ProvinceId { get; set; }
        public string Name { get; set; }
    }
}
