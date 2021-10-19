using System;
using System.Collections.Generic;
using System.Text;

namespace ForumModel {
    public class CatalogQuestion {
        List<Question> ListQuestion { get; } = new List<Question>();

        CatalogTag catalogTag;

        public CatalogQuestion(CatalogTag catalogTag) {
            this.catalogTag = catalogTag;
        }

        public void AddQuestion(Question question) {
            ListQuestion.Add(question);
            catalogTag.UpdateTags(question.Tags);
        }
    }
}
