using System;
using System.Collections.Generic;
using System.Text;

namespace POI.repository.ViewModels
{
    public class CreateHashtagViewModel
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
    }

    public class UpdateHashtagViewModel
    {
        public Guid HashtagId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
