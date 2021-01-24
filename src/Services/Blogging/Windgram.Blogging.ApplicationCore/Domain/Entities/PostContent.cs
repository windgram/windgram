using System.Collections.Generic;
using Windgram.Shared.Domain;

namespace Windgram.Blogging.ApplicationCore.Domain.Entities
{
    public class PostContent : ValueObject
    {
        public int PostId { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyword { get; set; }
        public string HtmlContent { get; set; }
        public Post Post { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PostId;
            yield return MetaDescription;
            yield return MetaKeyword;
            yield return HtmlContent;
        }
    }
}
