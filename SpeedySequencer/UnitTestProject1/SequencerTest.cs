using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeedySequencer;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class SequencerTest
    {
        RecipeData r1;
        RecipeData r2;
        RecipeData r3;

        Sequencer seq;

        [TestInitialize]
        public void CreateRecipes()
        {
            //Init sequencer
            seq = new Sequencer();

            //Recipe 1
            TaskData t1 = new TaskData(1, 15, false);
            TaskData t2 = new TaskData(2, 5, true);
            TaskData t3 = new TaskData(3, 2, true);
            TaskData t4 = new TaskData(4, 13, true);
            TaskData t5 = new TaskData(5, 20, false);
            TaskData t6 = new TaskData(6, 4, true);
            TaskData t7 = new TaskData(7, 3, true);
            TaskData t8 = new TaskData(8, 10, false);
            TaskData t9 = new TaskData(9, 1, true);
            TaskData t10 = new TaskData(10, 3, true);

            t10.addPrereq(t9);
            t10.addPrereq(t8);
            t8.addPrereq(t7);
            t7.addPrereq(t6);
            t9.addPrereq(t5);
            t5.addPrereq(t4);
            t4.addPrereq(t3);
            t3.addPrereq(t2);
            t2.addPrereq(t1);

            r1 = new RecipeData();
            r1.finalStep = t10;
            
            //Recipe 2
            TaskData t11 = new TaskData(11, 10, false);
            TaskData t12 = new TaskData(12, 6, true);
            TaskData t13 = new TaskData(13, 4, true);
            TaskData t14 = new TaskData(14, 15, true);

            t14.addPrereq(t13);
            t13.addPrereq(t12);
            t12.addPrereq(t11);

            r2 = new RecipeData();
            r2.finalStep = t14;

            //Recipe 3
            TaskData t15 = new TaskData(15, 9, false);
            TaskData t16 = new TaskData(16, 3, true);
            TaskData t17 = new TaskData(17, 5, true);
            TaskData t18 = new TaskData(18, 4, true);
            TaskData t19 = new TaskData(19, 1, true);
            TaskData t20 = new TaskData(20, 15, false);

            t20.addPrereq(t19);
            t19.addPrereq(t18);
            t19.addPrereq(t17);
            t19.addPrereq(t16);
            t19.addPrereq(t15);

            r3 = new RecipeData();
            r3.finalStep = t20;

        }

        [TestMethod]
        public void TestOrderRecipe1()
        {
            //This single recipe should end up with their ids sorted 1-10
            ICollection<RecipeData> recs = new List<RecipeData>();
            recs.Add(r1);
            List<TaskData> ordered = seq.sequenceTasks(recs);
            Assert.AreEqual("1,2,3,4,5,6,7,8,9,10", createTestIdString(ordered));
        }

        [TestMethod]
        public void TestOrderRecipe2_3()
        {
            //This single recipe should end up with their ids sorted 1-10
            ICollection<RecipeData> recs = new List<RecipeData>();
            recs.Add(r2);
            recs.Add(r3);
            List<TaskData> ordered = seq.sequenceTasks(recs);
            Assert.AreEqual("11,12,17,18,15,13,16,19,20,14", createTestIdString(ordered));
        }

        [TestMethod]
        public void TestSortFirstSteps()
        {
            TaskData t1 = new TaskData(1, 10, false);
            TaskData t2 = new TaskData(2, 8, true);
            TaskData t3 = new TaskData(3, 2, false);
            TaskData t4 = new TaskData(4, 5, true);

            RecipeData rec1 = new RecipeData();
            rec1.finalStep = t1;
            RecipeData rec2 = new RecipeData();
            rec2.finalStep = t2;
            RecipeData rec3 = new RecipeData();
            rec3.finalStep = t3;
            RecipeData rec4 = new RecipeData();
            rec4.finalStep = t4;
            
            List<RecipeData> recs = new List<RecipeData>();
            recs.Add(rec1);
            recs.Add(rec2);
            recs.Add(rec3);
            recs.Add(rec4);

            Assert.AreEqual("4,2,3,1", createTestIdString(seq.sortFinalSteps(recs)));
        }

        private string createTestIdString(List<TaskData> data)
        {
            string s = "";
            foreach (TaskData d in data)
            {
                s += d.taskId + ",";
            }
            return s.Substring(0, s.Length - 1);
        }
    }
}
