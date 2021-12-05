using System;
using System.Collections.Generic;

#nullable disable

namespace ForumDBContext.ModelDB​
{
    public partial class SearchByText
    {
        public int QuestionId { get; set; }
        public DateTime CreateDate { get; set; }
        public string AuthorName { get; set; }
        public string Topic { get; set; }
        public string QuestionText { get; set; }
    }
}
