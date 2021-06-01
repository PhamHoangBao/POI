using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjects
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

        public virtual ICollection<DesHashtag> DesHashtags { get; set; }
    }
}
