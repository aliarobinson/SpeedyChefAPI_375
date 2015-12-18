using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedySequencer
{
    public class TaskData : IComparable<TaskData>
    {
        public TaskData(int id, int time, bool active)
        {
            taskId = id;
            taskTime = time;
            timeRemaining = time;
            taskActive = active;
            taskPrereqs = new List<TaskData>();
        }

        public int taskId { get; set; }
        public string taskName { get; set; }
        public string taskDesc { get; set; }
        public int taskTime { get; set; }
        public int timeRemaining { get; set; }
        public bool taskTimeable { get; set; }
        public bool taskActive { get; set; }
        private ICollection<TaskData> taskPrereqs;

        public void addPrereq(TaskData td)
        {
            taskPrereqs.Add(td);
        }

        public ICollection<TaskData> getPrereqs()
        {
            return taskPrereqs;
        }

        public int CompareTo(TaskData other)
        {
            return this.timeRemaining - other.timeRemaining;
        }
    }
}
