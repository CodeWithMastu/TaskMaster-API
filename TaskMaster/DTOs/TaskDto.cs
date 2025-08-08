namespace TaskMaster.DTOs
{
    public class TaskDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public string? Priority { get; set; } // "low", "medium", "high"
        public bool IsCompleted { get; set; } = false;
    }

}
