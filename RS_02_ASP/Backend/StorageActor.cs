using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Newtonsoft.Json.Linq;
using Shared;

namespace Backend
{
    public class StorageActor : ReceiveActor
    {
        private List<Student> _students;

        public StorageActor()
        {
            Receive<Get>(msg => HandleGet(msg));
            Receive<GetAll>(msg => HandleGetAll(msg));
        }

        private void HandleGet(Get msg)
        {
            var student = _students.FirstOrDefault(s => s.Id == msg.Id);
            var json = student == null ? new JObject() : JObject.FromObject(student);
            Sender.Tell(new GetResult(json));
        }

        private void HandleGetAll(GetAll msg)
        {
            var json = JArray.FromObject(_students);
            Sender.Tell(new GetAllResult(json));
        }


        protected override void PreStart()
        {
            _students = new List<Student>();
            _students.Add(new Student
            (
                1,
                "ante",
                "antic",
                "1234",
                "email@email.email",
                5,
                true
            ));
            _students.Add(new Student
            (
                2,
                "ivo",
                "ivic",
                "1432",
                "email@pmfst.com",
                40,
                true
            ));
            _students.Add(new Student
            (
                3,
                "mate",
                "matic",
                "4444",
                "mate@email.email",
                60,
                true
            ));

            base.PreStart();
        }
    }
}