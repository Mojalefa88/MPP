using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMP.Models;

namespace MMP.Controllers
{
    public class BondController : Controller
    {
       
        // GET: Bond
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BondCalculator()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BondCalculator(double pp, double da, double ir, int ny)
        {

            //double purchasePrice = Convert.ToDecimal()
           

            double totalMonths = 0;
            double monthlyInterestRate = 0;
            double loanValue = 0;
            double monthlyPayment = 0;

            //double monthlyPayment2 = 0;
            double totalPayment = 0;
            double totalInterest = 0;
            double tempMontlyPayment = 0;

            totalMonths = ny * 12;
            monthlyInterestRate =/* Math.Round((*/(ir / 100) / 12;/*, 3)*/

            //double fistRow = 0;
            //double secondRow = 0;
            //double thirdRow = 0;
            //double fRow = 0;
            //double final = 0;
            //fistRow =  Math.Pow((1 + monthlyInterestRate), totalMonths);

            //secondRow = (monthlyInterestRate * fistRow);      //double mm = 0;

            //thirdRow = fistRow - 1;

            //fRow = secondRow / thirdRow;
            //final = pp * fRow;
            //double mn = 0;
            //double mp = 0;


            //mm = Math.Round((pp * (monthlyInterestRate * Math.Pow((1 + monthlyInterestRate), totalMonths))), 0);
            //mn = Math.Round((Math.Pow((1 + monthlyInterestRate), (totalMonths - 1))), 1);
            //mp = mm / mn;

            monthlyPayment = (pp - da) * ( (monthlyInterestRate * Math.Pow((1 + monthlyInterestRate), totalMonths)) / (Math.Pow((1 + monthlyInterestRate), totalMonths) - 1));

            //monthlyPayment2 = pp * (monthlyInterestRate * ((1 + monthlyInterestRate)*Math.Exp(totalMonths))) / ((1 + totalMonths)* Math.Exp(totalMonths - 1));
            loanValue = pp - da;

            tempMontlyPayment = Math.Round(monthlyPayment, 0);

            totalPayment = tempMontlyPayment * totalMonths;
            totalInterest = totalInterest + (totalPayment - loanValue);

            ViewBag.monthlyPayment = tempMontlyPayment;
            ViewBag.loanValue = Math.Round(loanValue,0);
            ViewBag.totalInterest = Math.Round(totalInterest,0);
            ViewBag.totalPayment = Math.Round(totalPayment,0);
            ViewBag.interstRate = ir;
            ViewBag.years = ny;
            return View();
        }
    }
}