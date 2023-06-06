using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Statistics.Query.GetTagsAnalitics
{
    public class TagsAnaliticsResult
    {
        public List<TagAnaliticResult> TagsAnalitics { get; set; } = new List<TagAnaliticResult>();
    }
}
