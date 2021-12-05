using System;
using System.Collections.Generic;

#nullable disable

namespace ForumDBContext.ModelDB​
{
    public partial class TagInQuestion
    {
        public int QuestionId { get; set; }
        public string Tag { get; set; }

        public virtual Question Question { get; set; }
    }
}
