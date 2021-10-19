using System;
using System.Collections.Generic;
using System.Text;

namespace ForumModel {
    public class CatalogTag {
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

        public CatalogTag() { }
        public CatalogTag(IEnumerable<string> tags) {
            foreach (var tag in tags) {
                TagDictionary[tag] = 0;
            }
        }

        public void UpdateTags(IEnumerable<string> tags) {
            foreach (var tag in tags) {
                try {
                    TagDictionary[tag]++;
                } catch (Exception) {
                    TagDictionary[tag] = 1;
                }
            }
        }
    }
}
