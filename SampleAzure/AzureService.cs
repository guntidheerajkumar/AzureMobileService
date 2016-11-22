using System;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SampleAzure
{
	public class AzureService
	{
		MobileServiceClient client { get; set; }
		IMobileServiceSyncTable<EmployeeInfo> table;

		public async Task Initialize()
		{
			if (client?.SyncContext?.IsInitialized ?? false)
			{
				return;
			}

			var azureUrl = "http://dotnetmobilesample.azurewebsites.net";
			client = new MobileServiceClient(azureUrl);
			var path = "employee.db";

			path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);
			var store = new MobileServiceSQLiteStore(path);
			store.DefineTable<EmployeeInfo>();

			await client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
			table = client.GetSyncTable<EmployeeInfo>();
		}

		public async Task<List<EmployeeInfo>> GetEmployees()
		{
			await Initialize();
			await SyncEmployees();
			return await table.ToListAsync();
		}

		public async Task InsertEmployee(EmployeeInfo employee)
		{
			await Initialize();
			await Task.WhenAll(table.InsertAsync(employee));
			await SyncEmployees();
		}

		public async Task<EmployeeInfo> GetEmployee(string id)
		{
			await Initialize();
			return await table.LookupAsync(id);
		}

		public async Task UpdateEmployee(EmployeeInfo employee)
		{
			await Initialize();
			await table.UpdateAsync(employee);
		}

		public async Task DeleteEmployee(EmployeeInfo employee)
		{
			await Initialize();
			await table.DeleteAsync(employee);
		}

		public async Task SyncEmployees()
		{
			await client.SyncContext.PushAsync();
			await table.PullAsync("allEmployees", table.CreateQuery());
		}
	}
}
