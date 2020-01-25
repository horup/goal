using System;

namespace Goal.Common
{
    public class GoalEntry
    {
        public int Id { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
        public string Description { get; set; } = "";

        public override string ToString()
        {
            return string.Format("{0}\t{1}", this.Id, this.Description);
        }
    }
}
