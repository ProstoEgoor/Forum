﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Model.DTO;

namespace ForumWebAPI.BL.Model {
    public class TagApiDto {
        public string Name { get; set; }

        public TagApiDto() { }

        public TagApiDto(TagInQuestionDbDTO tag) {
            Name = tag.TagName;
        }
    }

    public class TagFrequencyApiDto : TagApiDto {
        public int Frequency { get; set; }

        public TagFrequencyApiDto() { }

        public TagFrequencyApiDto(TagFrequencyDbDTO tag) {
            Name = tag.TagName;
            Frequency = tag.Frequency ?? 0;
        }
    }
}
