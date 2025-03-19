using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.LinkedIn.Domain.Entities
{
    public class Analytics
    {
        public int Id { get; set; }
        public List<DataPoint> viewsdata { get; set; }
        public List<DataPoint> commentsdata { get; set; }
        public List<DataPoint> sharedata { get; set; }
        public List<DataPoint> impressionsdata { get; set; }
    }
}
