using System;
using System.Collections.Generic;
using Newtonsoft.Json;
#nullable disable

namespace POI.repository.Entities
{
    public partial class Hashtag
    {
        public Hashtag()
        {
            DesHashtags = new HashSet<DesHashtag>();
        }

        public Guid HashtagId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Status { get; set; }

        [JsonIgnore]
        public virtual ICollection<DesHashtag> DesHashtags { get; set; }
    }
}
