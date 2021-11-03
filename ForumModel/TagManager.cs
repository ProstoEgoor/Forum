using System;
using System.Collections.Generic;
using System.Linq;

namespace ForumModel {
    public class TagManager {
        Dictionary<string, uint> TagDictionary { get; } = new Dictionary<string, uint>();

        public IReadOnlyList<(string tag, uint frequency)> TagFrequencies { get => TagDictionary.Select(item => (item.Key, item.Value)).ToList(); }

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
                        if (TagDictionary.ContainsKey(tag) && TagDictionary[tag] == 0) {
                            TagDictionary.Remove(tag);
                        }
                    } else {
                        TagDictionary[tag]++;
                    }
                } catch (Exception) {
                    if (remove) {
                        if (TagDictionary.ContainsKey(tag)) {
                            TagDictionary.Remove(tag);
                        }
                    } else {
                        TagDictionary[tag] = 1;
                    }
                }
            }
        }
    }
}
