using System.ComponentModel.DataAnnotations;

namespace WebApiwithEf.Model
{

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }   


        // 한명의 사람은 하나의 계좌만 가질 수 있음
        public Account Account {  get; set; }

    }

    public class Account
    {
        public int Id { get; set; }

        public int Balance { get; set; } = 0;

        public int UserId { get; set; }

        // 한 개의 계좌는 1명의 사람만 가지고 있을 수 있다. 
        public Person Person { get; set; } = null;

    }
}
