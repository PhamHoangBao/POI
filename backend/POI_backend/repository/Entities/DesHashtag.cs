using System;
using System.Collections.Generic;

#nullable disable

namespace POI.repository.Entities
{
    public partial class DesHashtag
    {
        public Guid DesHashtagId { get; set; }
        public Guid DestinationId { get; set; }
        public Guid HashtagId { get; set; }
        public int Status { get; set; }

        public virtual Destination Destination { get; set; }
        public virtual Hashtag Hashtag { get; set; }
    }
}
