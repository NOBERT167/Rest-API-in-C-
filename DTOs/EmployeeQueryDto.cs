using System.Text.Json.Serialization;

namespace BusinessCentralApi.Dtos
{
    public class EmployeeQueryDto
    {
        [JsonPropertyName("employeeNo")]
        public required string EmployeeNo { get; set; }

        [JsonPropertyName("firstName")]
        public required string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public required string LastName { get; set; }

        [JsonPropertyName("jobTitle")]
        public required string JobTitle { get; set; }

        [JsonPropertyName("department")]
        public required string Department { get; set; }

        [JsonPropertyName("phoneNumber")]
        public required string PhoneNumber { get; set; }
    }
}