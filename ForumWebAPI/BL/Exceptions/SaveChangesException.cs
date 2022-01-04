using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumWebAPI.BL.Exceptions {
    public class SaveChangesException : Exception {
        public SaveChangesException(string message = "Произошла неизвестная ошибка при сохранении данных") : base(message) { }
        public SaveChangesException(Exception innerException) : base("Произошла неизвестная ошибка при сохранении данных", innerException) { }
        public SaveChangesException(string message, Exception innerException) : base(message, innerException) { }
    }
}
