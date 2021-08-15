using Sciensano.CovidJson.Parser.SciensanoModels;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Sciensano.CovidJson.Parser.Infrastructure
{
    public static class SciensanoCovidDownloader
    {
        public async static Task<string> GetCovidData( Type sourceType )
        {
            var fileName = "";
            switch (sourceType.Name)
            {
                case nameof(SciensanoHospitalisationModel):
                    fileName = "COVID19BE_HOSP.json";
                    break;
                case nameof(SciensanoCasesModel):
                    fileName = "COVID19BE_CASES_AGESEX.json";
                    break;
                case nameof(SciensanoTestsModel):
                    fileName = "COVID19BE_tests.json";
                    break;
                case nameof(SciensanoVaccinationModel):
                    fileName = "COVID19BE_VACC.json";
                    break;
                default:
                    throw new Exception($"Invalid source type '{sourceType.Name}'");
            }

            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("Invalid filename selected to download.");

            var data = "";
            using (var client = new WebClient())
            {
                data = await client.DownloadStringTaskAsync($"https://epistat.sciensano.be/Data/{fileName}");
            }

            if (string.IsNullOrWhiteSpace(data))
                throw new Exception($"failed to download {fileName}. Data is empty");

            return data;            
    }
    }
}
