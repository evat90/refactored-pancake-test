using System;
using System.Linq;
using System.Web.Mvc;
using Quote.Contracts;
using Quote.Models;
using Quote.Responses;
using Quote;
using PruebaIngreso.Utils;
using Newtonsoft.Json;

namespace PruebaIngreso.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuoteEngine quote;

        public HomeController(IQuoteEngine quote)
        {
            this.quote = quote;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                TourCode = "E-U10-PRVPARKTRF",
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);
            var tour = result.Tours.FirstOrDefault();
            ViewBag.Message = "Test 1 Correcto";
            return View(tour);
        }

        public ActionResult Test2()
        {
            ViewBag.Message = "Test 2 Correcto";
            return View();
        }


        public ActionResult Test3()
        {
            RootMarginResponse rootMargin1=WebService.getInstance().invoke("https://refactored-pancake.free.beeceptor.com/margin/", "E-U10-UNILATIN 204");
            RootMarginResponse rootMargin2 = WebService.getInstance().invoke("https://refactored-pancake.free.beeceptor.com/margin/", "E-U10-DSCVCOVE 404");
            RootMarginResponse rootMargin3 = WebService.getInstance().invoke("https://refactored-pancake.free.beeceptor.com/margin/", "E-E10-PF2SHOW 500");
            ViewBag.prueba1 = JsonConvert.SerializeObject(rootMargin1.data);
            ViewBag.prueba2 = JsonConvert.SerializeObject(rootMargin1.data);
            ViewBag.prueba3 = JsonConvert.SerializeObject(rootMargin1.data);
            return View();
        }

        public ActionResult Test4()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);
            var result_tourQuotes = result.TourQuotes;
            for(int x = 0; x < result_tourQuotes.Count; x++)
            {
               RootMarginResponse rootMargin = WebService.getInstance().invoke("https://refactored-pancake.free.beeceptor.com/margin/", result_tourQuotes[x].TourCode);
                IMarginProviderDinamic marginProviderDinamic = new BaseMarginProvider();
                result_tourQuotes[x].margin = marginProviderDinamic.GetMargin(rootMargin);
            }
            return View(result_tourQuotes);
        }
    }
}