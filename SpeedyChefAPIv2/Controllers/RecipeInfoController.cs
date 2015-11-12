using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpeedyChefAPIv2.Controllers
{
    public class RecipeInfoController : Controller
    {
        //
        // GET: /RecipeInfo/
        /// <param name="recid">ID of the recipe to get info for</param>
        public ActionResult RecipeInfo(int recid)
        {
            SpeedyChefDataContext scdc = new SpeedyChefDataContext();
            IEnumerable<RecipeInfoResult> recipeInfoResult = null;

            recipeInfoResult = scdc.RecipeInfo(recid);
            return Json(recipeInfoResult, JsonRequestBehavior.AllowGet);
        }

        //GET: /RecipeTasks/
        /// <summary>
        ///  
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        public ActionResult RecipeTasks(int recid)
        {
            SpeedyChefDataContext scdc = new SpeedyChefDataContext();
            IEnumerable<RecipeTasksResult> recipeTasks = null;

            recipeTasks = scdc.RecipeTasks(recid);
            return Json(recipeTasks, JsonRequestBehavior.AllowGet);
        }

        //GET: /RecipeIngredients/
        /// <summary>
        ///  
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        public ActionResult RecipeIngredients(int recid)
        {
             SpeedyChefDataContext scdc = new SpeedyChefDataContext();
             IEnumerable<RecipeIngredientsResult> recipeIngredients = null;

             recipeIngredients = scdc.RecipeIngredients(recid);
             return Json(recipeIngredients, JsonRequestBehavior.AllowGet);
           /* Food_Item[] tempResult = new Food_Item[4];
            tempResult[0] = new Food_Item();
            tempResult[0].Foodname = "2 small garlic cloves";
            tempResult[1] = new Food_Item();
            tempResult[1].Foodname = "2 sticks unsalted butter";
            tempResult[2] = new Food_Item();
            tempResult[2].Foodname = "1 1/2 teaspoons finely chopped flat-leaf parsley";
            tempResult[3] = new Food_Item();
            tempResult[3].Foodname = "2 (1 1/4-pound) live lobsters"; 
            return Json(tempResult, JsonRequestBehavior.AllowGet); */
        }
    }
}