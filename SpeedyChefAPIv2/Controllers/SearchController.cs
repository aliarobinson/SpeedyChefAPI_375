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
                    resultListDict[keyword] = new List<SearchSingleKeywordResult>(scdc.SearchSingleKeyword(keyword, ordertype, ascending));
                    if (tempRes == null)
                    {
                        tempRes = resultListDict[keyword];
                    }
                }
                foreach (string currKey in resultListDict.Keys)
                {
                    tempRes = tempRes.Intersect(resultListDict[currKey], new SearchSingleComparer());
                }
                return Json(tempRes, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SearchByUnion(string inputKeywords, string ordertype, string ascending, string subgenre)
        {
            Dictionary<string, List<SearchSingleKeywordResult>> resultListDict = new Dictionary<string, List<SearchSingleKeywordResult>>();
            Dictionary<string, List<SearchSingleKeywordResult>> resultSubgenreDict = new Dictionary<string, List<SearchSingleKeywordResult>>();
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
                    if (helperList == null)
                    {
                        subgenreList = new List<SearchSingleKeywordResult>(scdc.SearchSingleKeyword(subgenreKeyword, ordertype, ascending));
                        helperList = new List<SearchSingleKeywordResult>(scdc.SearchSingleKeyword(subgenreKeyword, ordertype, ascending));
                    }
                    resultSubgenreDict[subgenreKeyword] = new List<SearchSingleKeywordResult>(scdc.SearchSingleKeyword(subgenreKeyword, ordertype, ascending));
                }
                foreach (string subgenreKey in resultSubgenreDict.Keys)
                {
                    subgenreList = subgenreList.Intersect(resultSubgenreDict[subgenreKey], new SearchSingleComparer());
                    helperList = subgenreList.Intersect(resultSubgenreDict[subgenreKey], new SearchSingleComparer());
                }
                foreach (string keyword in keywordList)
                {
                    if (subgenreList == null)
                    {
                        subgenreList = new List<SearchSingleKeywordResult>(scdc.SearchSingleKeyword(keyword, ordertype, ascending));
                    }
                    resultListDict[keyword] = new List<SearchSingleKeywordResult>(scdc.SearchSingleKeyword(keyword, ordertype, ascending));
                }
                foreach (string currKey in resultListDict.Keys)
                {
                    subgenreList = subgenreList.Intersect(resultListDict[currKey], new SearchSingleComparer());
                    helperList = helperList.Except(resultListDict[currKey], new SearchSingleComparer());
                }
                return Json(subgenreList.Concat(helperList), JsonRequestBehavior.AllowGet);
            }
        }
    }
}