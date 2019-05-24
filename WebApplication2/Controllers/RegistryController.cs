using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{

    // comparators

    // compare by name
    class NameComparator : IComparer<ProgramModel>
    {
        public int Compare(ProgramModel p1, ProgramModel p2)
        {
            return p1.name.CompareTo(p2.name);
        }
    }


    // compare by guid
    class GUIDComparator : IComparer<ProgramModel>
    {
        public int Compare(ProgramModel p1, ProgramModel p2)
        {
            return p1.guid.CompareTo(p2.guid);
        }
    }

    public class RegistryController : Controller
    {

        public ActionResult Download()
        {
            Response.ContentType = "application/exe";
            Response.AddHeader("content-disposition", "attachment;filename=download.exe");
            Response.TransmitFile(Server.MapPath("~/App_Data/ConsoleApp1.exe"));
            Response.End();
            return View();
        }

        // GET: Registry
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult list(string q="", bool guid=false, string sortBy="none")
        {
            // where we save the programs
            List<ProgramModel> programs = new List<ProgramModel>();

            // determine the type of search to be performed
            if (guid)
                ProgramModel.findGUID(q, programs);
            else
                ProgramModel.findName(q, programs);

            // if we need to sort this
            if(sortBy != "none")
            {
                // define comparator
                IComparer<ProgramModel> comp;

                // select comparator
                if (sortBy == "name")
                    comp = new NameComparator();
                else
                    comp = new GUIDComparator();

                // sort
                programs.Sort(comp);
            }

            return View(programs);
        }
    }
}