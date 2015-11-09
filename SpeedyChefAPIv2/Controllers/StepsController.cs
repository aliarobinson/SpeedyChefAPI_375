using SpeedyChefApi.Models;
using SpeedyChefAPIv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpeedyChefApi.Controllers
{
    public class StepsController : Controller
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="mealid"></param>
        /// <returns></returns>
        public ActionResult Index(int mealid)
        {
            SpeedyChefDataContext scdc = new SpeedyChefDataContext();
            IEnumerable<TasksForMealResult> mealTasks = null;

            mealTasks = scdc.TasksForMeal(mealid);
            return Json(mealTasks, JsonRequestBehavior.AllowGet);
        }

	}
}