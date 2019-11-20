using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AirtableApiClient;

namespace YeetAnotherBlazorAirtable.Pages
{
    public class FetchAirtableDataModel : PageModel
    {
        readonly string baseId = "YOUR_BASE_ID";
        readonly string appKey = "YOUR_APP_KEY";

        public List<string> Names;

        public async Task<IActionResult> OnGetAsync()
        {
            Names = new List<string>();

            AirtableListRecordsResponse response = await getAirtableData();

            return Page();

        }

        public async Task<AirtableListRecordsResponse> getAirtableData()
        {
            string offset = null;
            string errorMessage = null;
            var records = new List<AirtableRecord>();


            AirtableBase airtableBase = new AirtableBase(appKey, baseId);

            Task<AirtableListRecordsResponse> task = airtableBase.ListRecords(
                               "People",
                               offset);
            AirtableListRecordsResponse response = await task;

            if (response.Success)
            {
                System.Diagnostics.Debug.WriteLine("SUCCESS!!! We received a response.");
                records.AddRange(response.Records.ToList());
                offset = response.Offset;
            }
            else if (response.AirtableApiError is AirtableApiException)
            {
                errorMessage = response.AirtableApiError.ErrorMessage;

            }
            else
            {
                errorMessage = "Unknown error";

            }

            foreach (var r in records)
            {
                string theName = r.GetField("Name").ToString();
                Names.Add(theName);
                System.Diagnostics.Debug.WriteLine("RECORD: " + Names.Last());
            }

            return response;
        }
    }
}
