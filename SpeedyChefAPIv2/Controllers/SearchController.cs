using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using SpeedyChefAPIv2;
using System.Text.RegularExpressions;

namespace SpeedyChefApi.Controllers
{
    public class SearchController : Controller
    {
        // GET: /Search/
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: /Search/
        public void Post([FromBody]string value)
        {
        }

        // PUT: /Search/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: /Search/5
        public void Delete(int id)
        {
        }

        public class SearchSingleComparer : IEqualityComparer<SearchSingleKeywordResult>
        {
            public bool Equals(SearchSingleKeywordResult x, SearchSingleKeywordResult y)
            {
                return x.Recid == y.Recid;
            }

            public int GetHashCode(SearchSingleKeywordResult obj)
            {
                return obj.Recid.GetHashCode();
            }
        }

        public ActionResult Search(string inputKeywords, string ordertype, string ascending)
        {
            string[] keywordList = inputKeywords.Split(',');

            //At least one letter must be submitted to begin searching
            Regex rx = new Regex("^[A-Za-z,]+$");

            // Find matches.
            Match matched = rx.Match(inputKeywords);

            System.Diagnostics.Debug.WriteLine(matched.ToString());
            if (!matched.Success)
            {
                return Json(new List<SearchSingleKeywordResult>(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                Dictionary<string, List<SearchSingleKeywordResult>> resultListDict = new Dictionary<string, List<SearchSingleKeywordResult>>();
                SpeedyChefDataContext scdc = new SpeedyChefDataContext();
                IEnumerable<SearchSingleKeywordResult> tempRes = null;
                foreach (string keyword in keywordList)
                {
                    List<SearchSingleKeywordResult> newRes = scdc.SearchSingleKeyword(keyword, ordertype, ascending).ToList();
                    if (tempRes == null)
                    {
                        tempRes = newRes.ToList();
                    }
                    else
                    {
                        tempRes = tempRes.Intersect(newRes.ToList(), new SearchSingleComparer());
                    }
                }
                return Json(tempRes, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SearchByUnion(string inputKeywords, string ordertype, string ascending, string subgenre)
        {
            IEnumerable<SearchSingleKeywordResult> subgenreList = null;
            IEnumerable<SearchSingleKeywordResult> helperList = null;
            string[] keywordList = inputKeywords.Split(',');
            string[] subgenreKeywords = subgenre.Split(',');
            Regex rx = new Regex("^[A-Za-z,]+$");

            // Find matches.
            Match matched = rx.Match(subgenre);

            System.Diagnostics.Debug.WriteLine(matched.Value.ToString());
            if (!matched.Success)
            {
                return Json(new List<SearchSingleKeywordResult>(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                SpeedyChefDataContext scdc = new SpeedyChefDataContext();
                foreach (string subgenreKeyword in subgenreKeywords)
                {
                    List<SearchSingleKeywordResult> newRes = scdc.SearchSingleKeyword(subgenreKeyword, ordertype, ascending).ToList();
                    if (subgenreList == null)
                    {
                        subgenreList = newRes.ToList();
                        helperList = newRes.ToList();
                    }
                    else
                    {
                        subgenreList = subgenreList.Intersect(newRes.ToList(), new SearchSingleComparer());
                        subgenreList = subgenreList.Intersect(newRes.ToList(), new SearchSingleComparer());
                    }
                }
                foreach (string keyword in keywordList)
                {
                    List<SearchSingleKeywordResult> newRes = scdc.SearchSingleKeyword(keyword, ordertype, ascending).ToList();
                    subgenreList = subgenreList.Intersect(newRes.ToList(), new SearchSingleComparer());
                    helperList = helperList.Except(newRes.ToList(), new SearchSingleComparer());
                }
                return Json(subgenreList.Concat(helperList), JsonRequestBehavior.AllowGet);
            }
        }

        public class CompareIntegers : IComparer<DateTime>
        {
            public int Compare(DateTime s1, DateTime s2)
            {
                if (s1 >= s2)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        public ActionResult GenerateUpcomingMeals(string user, string date1, string date2)
        {
            SpeedyChefDataContext scdc = new SpeedyChefDataContext();
            IEnumerable<GetMealsBetweenDatesResult> mealList = scdc.GetMealsBetweenDates(user, date1, date2);
            return Json(mealList, JsonRequestBehavior.AllowGet);   
        }
    }
}