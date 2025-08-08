namespace TaskMaster.Models
{
    public class TasksItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public string? Priority { get; set; }
        public bool IsCompleted { get; set; } = false;


        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
