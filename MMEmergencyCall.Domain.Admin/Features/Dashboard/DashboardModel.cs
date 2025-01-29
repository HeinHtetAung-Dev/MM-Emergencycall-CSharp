using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.Dashboard
{
	public class DashboardModel
	{
		public RequestSummaryModel RequestSummary { get; set; }
		public List<TopTenServicePerTownshipModel> TopTenServicePerTownship { get; set; }
		public List<ServiceProviderActivityModel> ServiceProviderActivity { get; set; }
		public List<TopTenRequestPerUserModel> TopTenRequestPerUser { get; set; }
	}

	public class RequestSummaryModel
	{
		public int DailyRequest { get; set; }
		public int WeeklyRequest { get; set; }
		public int MonthlyRequest { get; set; }
	}

	public class ServiceProviderActivityModel
	{
		public int ServiceId { get; set; }
		public string ServiceName { get; set; }
		public string ServiceType { get; set; }
		public string PhoneNumber { get; set; }
		public string Location { get; set; }
		public int Count { get; set; }
	}

	public class TopTenRequestPerUserModel
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string UserEmail { get; set; }
		public string UserPhoneNumber { get; set; }
		public string UserAddress { get; set; }
		public int Count { get; set; }
	}

	public class TopTenServicePerTownshipModel
	{
		public string TownshipCode { get; set; }
		public string TownshipNameEn { get; set; }
		public string TownshipNameMM { get; set; }
		public int Count { get; set; }
	}
}
