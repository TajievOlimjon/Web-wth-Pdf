namespace WebApi.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? Email { get; set; } = null;
        public int Age { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string? Address { get; set; } = null;
    }
}

