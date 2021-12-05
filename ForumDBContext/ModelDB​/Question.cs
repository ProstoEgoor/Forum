using System;
using System.Collections.Generic;

#nullable disable

namespace ForumDBContext.ModelDB​
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            TagInQuestions = new HashSet<TagInQuestion>();
        }

        public int QuestionId { get; set; }
        public DateTime CreateDate { get; set; }
        public string AuthorName { get; set; }
        public string Topic { get; set; }
        public string QuestionText { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<TagInQuestion> TagInQuestions { get; set; }
    }
}
