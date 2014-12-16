﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jqgrid_asp.net_mvc.demo.web.Models;
using jqgrid_asp.net_mvc.Tests.BDD.Common;
using TechTalk.SpecFlow;
using System.Net.Http;
using System.Net.Http.Headers;
using TechTalk.SpecFlow;
using FluentAssertions;
using System.Net;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;

namespace jqgrid_asp.net_mvc.Tests.TDD.API.Steps
{
    [Binding]
    public class Uncategorized
    {
        [When(@"I read records\tvia jqGrid invoking API")]
        public void WhenIReadRecordsViaJqGridInvokingAPI()
        {
            Uri url = null;

            if (Uri.TryCreate(new Uri(Vars.DemoSiteWebHost), "/Home/IndexJsonList?_search=false&nd=1418698409753&rows=10&page=1&sidx=invid&sord=desc", out url))
            {
                Console.WriteLine("url is {0}", url);
            }

            HttpClient client = new HttpClient();
            // Add an Accept header for JSON format.            
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // List all products.
            HttpResponseMessage response = client.GetAsync(url).Result;  // Blocking call! 
            if (response.IsSuccessStatusCode)
            {
                JqGridReadingJsonData = response.Content.ReadAsAsync<JqGridReadingJsonData<Person, Person>>().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        public JqGridReadingJsonData<Person, Person> JqGridReadingJsonData
        {
            get
            {
                return ScenarioContext.Current["JqGridReadingJsonData"] as JqGridReadingJsonData<Person, Person>;
            }
            set
            {
                ScenarioContext.Current["JqGridReadingJsonData"] = value;
            }
        }

        [Then(@"then should get init records")]
        public void ThenThenShouldGetInitRecords()
        {
            var persons = JqGridReadingJsonData.rows as IEnumerable<Person>;

            foreach (var p in persons)
            {
                Console.WriteLine("FirstName: {0}\tLastName: {1}\tCity: {2}\tZip:{3}", p.FirstName, p.LastName, p.City, p.Zip);
            }
            persons.SingleOrDefault(p => p.FirstName == "Weimin" && p.LastName == "Ye" && p.City == "San Francisco" && p.Zip == "94112").Should().NotBeNull();
        }

    }
}