using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedySequencer
{
    public class Sequencer
    {
        public List<TaskData> sequenceTasks(ICollection<RecipeData> recipes)
        {
            List<TaskData> orderedTasks = new List<TaskData>();
            List<TaskData> inProgress = sortFinalSteps(recipes);
            List<TaskData> taskQueue;

            TaskData currentTask;
          //  int elapsedTime;
            while (true)
            {
                inProgress.Sort();
                currentTask = inProgress[0];
                for (int i = 1; i < inProgress.Count; i++)
                {
                    if (!inProgress[i].taskActive)
                    {
                        inProgress[i].timeRemaining -= currentTask.timeRemaining;
                    }
                }
                inProgress.Remove(currentTask);
                orderedTasks.Add(currentTask);

                foreach (TaskData t in currentTask.getPrereqs())
                {
                    inProgress.Add(t);
                }

                if (inProgress.Count == 0)
                    break;
            }

            orderedTasks.Reverse();
            return orderedTasks;
        }

        public List<TaskData> sortFinalSteps(ICollection<RecipeData> recipes)
        {
            List<TaskData> allSteps = new List<TaskData>();
            List<TaskData> activeSteps = new List<TaskData>();
            //Separate active steps from inactive steps
            foreach (RecipeData r in recipes)
            {
                if (r.finalStep.taskActive)
                    activeSteps.Add(r.finalStep);
                else
                    allSteps.Add(r.finalStep);
            }
            allSteps.Sort();
            activeSteps.Sort();
            activeSteps.AddRange(allSteps);
            return activeSteps;
        }
    }
}
