using System.Text.Json.Serialization;

namespace AwesomeBank.API.Application.Models.ViewModels
{
    public class InterestRuleViewModel
    {
        public string RuleId { get; set; }
        public DateTime Date { get; set; }

        [JsonIgnore]
        public DateTime? EndDate { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        public decimal Rate { get; set; }
    }
}