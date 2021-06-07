using System;
using System.Collections.Generic;

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

        public virtual ICollection<DesHashtag> DesHashtags { get; set; }
    }
}
