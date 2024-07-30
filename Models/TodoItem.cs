namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Title { get; set; } = "";
        public DateTime Created { get; set; }
        public bool IsComplete { get; set; }

        public override string ToString()
        {
            return $"[Id: {Id}, Title: {Title}, Created: {Created}, IsComplete: {IsComplete}]";
        }
    }
}
