using System.ComponentModel.DataAnnotations;

namespace Q.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public int CategoryId { get; set; }
        public string period { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }
        public int N { get; set; }
        public string? Details { get; set; }
        public bool isFinish { get; set; }
        public int Score { get; set; }
    }
}
