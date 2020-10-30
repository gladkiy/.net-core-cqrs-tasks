namespace Task.DTOModels.ViewModels
{
    public class TaskVm
    {
        public TaskVm(Task.DBModels.Entities.Task task)
        {
            Id = task.Id;
            Description = task.Description;
            Completed = task.Completed;
        }

        public TaskVm() { }

        public int Id { get; set; }

        public string Description { get; set; }

        public bool Completed { get; set; }
    }
}