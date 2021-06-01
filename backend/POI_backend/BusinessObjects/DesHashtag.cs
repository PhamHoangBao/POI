using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
{
    public partial class DesHashtag
    {
        public Guid DesHashtagId { get; set; }
        public Guid DestinationId { get; set; }
        public Guid HashtagId { get; set; }

        public virtual Destination Destination { get; set; }
        public virtual Hashtag Hashtag { get; set; }
    }
}
