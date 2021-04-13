using Newtonsoft.Json.Linq;
using RS_01.Models;

namespace RS_01.DtoMappers
{
    public static class StudentDto
    {
        public static Student FromJson(JObject json)
        {
            var id = json["id"].ToObject<int>();
            var firstname = json["firstname"].ToObject<string>();
            var lastname = json["lastname"].ToObject<string>();
            var jmbag = json["jmbag"].ToObject<string>();
            var email = json["email"].ToObject<string>();
            var ects = json["ects"].ToObject<int>();
            var isEnrolled = json["isEnrolled"].ToObject<bool>();

            return new Student(id, firstname, lastname, jmbag, email, ects, isEnrolled);
        }
    }
}