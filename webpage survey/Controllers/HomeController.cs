using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.IO;
using webpage_survey.Models;

namespace webpage_survey.Controllers
{
    public class HomeController : Controller
    {
        //TraitsEntities db = new TraitsEntities();
        //
        // GET: /Home/
        public ActionResult Index(string tweeterId = null)
        {
            Radio r = new Radio();
            Session["name"] = null;
            Session["valid"] = "true";
            if(tweeterId != null)
            {
                using (TraitsEntities db = new TraitsEntities())
                {
                    User u = db.Users.Where(m => m.Username == tweeterId).FirstOrDefault();
                    if (u != null)
                    {
                        r.username = tweeterId;
                        Session["name"] = tweeterId;
                    }
                }
            }
            return View(r);
            
 
            
        }

        public ActionResult Submit()
        {
            string t = "true";
            String name = Request["username"];
            if (name != null)
            {
                

                int extro = Convert.ToInt32(Request["r1"]) + (6 - Convert.ToInt32(Request["r6"])) + Convert.ToInt32(Request["r11"]) + Convert.ToInt32(Request["r16"]) + (6 - Convert.ToInt32(Request["r21"])) + Convert.ToInt32(Request["r26"]) + (6 - Convert.ToInt32(Request["r31"])) + Convert.ToInt32(Request["r36"]); //1, 6R,11, 16, 21R, 26, 31R, 36
                int agree = (6 - Convert.ToInt32(Request["r2"])) + Convert.ToInt32(Request["r7"]) + (6 - Convert.ToInt32(Request["r12"])) + Convert.ToInt32(Request["r17"]) + Convert.ToInt32(Request["r22"]) + (6 - Convert.ToInt32(Request["r27"])) + Convert.ToInt32(Request["r32"]) + (6 - Convert.ToInt32(Request["r37"])) + Convert.ToInt32(Request["r42"]); //2R, 7, 12R, 17, 22, 27R, 32, 37R, 42
                int neuro = Convert.ToInt32(Request["r4"]) + (6 - Convert.ToInt32(Request["r9"])) + Convert.ToInt32(Request["r14"]) + Convert.ToInt32(Request["r19"]) + (6 - Convert.ToInt32(Request["r24"])) + Convert.ToInt32(Request["r29"]) + (6 - Convert.ToInt32(Request["r34"])) + Convert.ToInt32(Request["r39"]); //4, 9R, 14, 19, 24R, 29, 34R, 39
                int conci = Convert.ToInt32(Request["r3"]) + (6 - Convert.ToInt32(Request["r8"])) + Convert.ToInt32(Request["r13"]) + (6 - Convert.ToInt32(Request["r18"])) + (6 - Convert.ToInt32(Request["r23"])) + Convert.ToInt32(Request["r28"]) + Convert.ToInt32(Request["r33"]) + Convert.ToInt32(Request["r38"]) + (6 - Convert.ToInt32(Request["r43"])); //3, 8R, 13, 18R, 23R, 28, 33, 38, 43R
                int openn = Convert.ToInt32(Request["r5"]) + Convert.ToInt32(Request["r10"]) + Convert.ToInt32(Request["r15"]) + Convert.ToInt32(Request["r20"]) + Convert.ToInt32(Request["r25"]) + Convert.ToInt32(Request["r30"]) + (6 - Convert.ToInt32(Request["r35"])) + Convert.ToInt32(Request["r40"]) + (6 - Convert.ToInt32(Request["r41"])) + Convert.ToInt32(Request["r44"]); //5, 10, 15, 20, 25, 30, 35R, 40, 41R, 44

                double extroversion = ((((extro / 8) - 3.2) / 0.8) * 10) + 50;
                double agreeableness = ((((agree / 9) - 3.8) / 0.6) * 10) + 50;
                double conscientiousness = ((((conci / 9) - 3.6) / 0.7) * 10) + 50;
                double neuroticism = ((((neuro / 8) - 3.0) / 0.8) * 10) + 50;
                double openness = ((((openn / 10) - 3.7) / 0.7) * 10) + 50;

                User u = new User();
                if (Session["name"] == null)
                {
                    u.Username = Request["username"];
                    u.Name = null;
                }
                else if (((string)Session["name"]) == name)
                {
                    u.Name = Request["username"];
                }
                u.City = Request["city"];
                u.nickname = Request["nickname"];
                u.email = Request["email"];
                u.timedate = DateTime.Now;
                u.Age = Request["age"];
                u.Gender = Request["gender"];
                u.Extroversion = extroversion;
                u.Agreeableness = agreeableness;
                u.Conscientiousness = conscientiousness;
                u.Neuroticism = neuroticism;
                u.Openness = openness;
                u.Status = Request["status"];
                using (TraitsEntities db = new TraitsEntities())
                {
                    db.Users.Add(u);
                    db.SaveChanges();

                }

                WriteTraits("Data/Traits.txt", u.ToString());
                Session.RemoveAll();
                return View("Feedback", u);
            }
            else
                return RedirectToAction("/Index");
        }

        public void WriteTraits(String filename, String data)
        {
            try
            {
                FileStream file = new FileStream(Server.MapPath("~/"+filename), FileMode.Append, FileAccess.Write);
                using (StreamWriter sr = new StreamWriter(file))
                {
                    sr.WriteLine(data);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found");
            }
            catch (IOException)
            {
                Console.WriteLine("IOException occur");
            }
        }

     }
            
}

