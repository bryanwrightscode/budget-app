using FinancialPortal.Models.PlaidModels;
using FinancialPortal.Models.Viewmodels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FinancialPortal.Controllers
{
    public class BanksController : BaseController
    {
        public ActionResult Index()
        {
            string _banks = GetPlaidBanks();
            List<Institution> banks = Institutions.FromJson(_banks).PurpleInstitutions;
            List<Bank> Banks = new List<Bank>();
            banks.ForEach(b => Banks.Add(new Bank { BankName = b.Name }));

            //string _logoBank = GetPlaidBank();
            //LogoBank logoBank = LogoBank.FromJson(_logoBank);
            BanksViewModel vm = new BanksViewModel { BankNames = Banks };
            return View(vm);
        }

        public string GetPlaidBanks()
        {
            RestClient client = new RestClient("https://development.plaid.com/institutions/get");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\r\n\t\"client_id\":\"59fc9f084e95b8782b00bfc9\",\r\n\t\"secret\": \"a6baa3f3b001e603010fe864df459e\",\r\n\t\"count\": 50,\r\n\t\"offset\": 1\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        //public string GetPlaidBank()
        //{
        //    var client = new RestClient("https://development.plaid.com/institutions/get_by_id");
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("content-type", "application/json");
        //    request.AddParameter("application/json", "{\r\n\t\"institution_id\": \"ins_10\",\r\n    \"public_key\": \"e88c91e098c75bc3f62b8237470fe9\",\r\n    \"options\": {\"include_display_data\": true}\r\n}", ParameterType.RequestBody);
        //    IRestResponse response = client.Execute(request);
        //    return response.Content;
        //}

        public AppSettings GetAppSettings()
        {
            AppSettings settings = new AppSettings
            {
                ClientId = WebConfigurationManager.AppSettings["plaidclientid"],
                Secret = WebConfigurationManager.AppSettings["plaidsecret"]
            };
            return settings;
        }

        public class AppSettings
        {
            public string ClientId { get; set; }
            public string Secret { get; set; }
        }

        //public ActionResult Index2()
        //{
        //    //var client = new RestClient(PrivateClass.ApiGet);
        //    //var request = new RestRequest(Method.GET);
        //    //var response = new RestResponse();
        //    //Task.Run(async () =>
        //    //{
        //    //    response = await GetResponseContentAsync(client, request) as RestResponse;
        //    //}).Wait();
        //    //var content = response.Content;
        //}
    }
}