﻿using System;
using System.Collections.Generic;

namespace ForumModel {
    public class TagManager {
        Dictionary<string, uint> TagDictionary { get; } = new Dictionary<string, uint>();



        public uint this[string tag] {
            get {
                try {
                    return TagDictionary[tag];
                } catch (Exception) {
                    return 0;
                }
            }
        }

        public TagManager() { }
        public TagManager(IEnumerable<string> tags) {
            foreach (var tag in tags) {
                TagDictionary[tag] = 0;
            }
        }

        public void UpdateTags(IEnumerable<string> tags, bool remove = false) {
            foreach (var tag in tags) {
                try {
                    if (remove) {
                        TagDictionary[tag]--;
                    } else {
                        TagDictionary[tag]++;
                    }
                } catch (Exception) {
                    if (remove) {
                        TagDictionary[tag] = 0;
                    } else {
                        TagDictionary[tag] = 1;
                    }
                }
            }
        }
    }
}
