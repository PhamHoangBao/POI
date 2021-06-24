using System;
using System.Collections.Generic;
using Newtonsoft.Json;
#nullable disable

namespace POI.repository.Entities
{
    public partial class DesHashtag
    {
        public Guid DesHashtagId { get; set; }
        public Guid DestinationId { get; set; }
        public Guid HashtagId { get; set; }
        public int Status { get; set; }

        [JsonIgnore]
        public virtual Destination Destination { get; set; }
        [JsonIgnore]
        public virtual Hashtag Hashtag { get; set; }
    }
}
