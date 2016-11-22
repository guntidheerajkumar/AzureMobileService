// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SampleAzure
{
	[Register ("EditEmployeeController")]
	partial class EditEmployeeController
	{
		[Outlet]
		UIKit.UITextField TxtEmployeeAge { get; set; }

		[Outlet]
		UIKit.UITextField TxtEmployeeDOB { get; set; }

		[Outlet]
		UIKit.UITextField TxtEmployeeName { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TxtEmployeeName != null) {
				TxtEmployeeName.Dispose ();
				TxtEmployeeName = null;
			}

			if (TxtEmployeeDOB != null) {
				TxtEmployeeDOB.Dispose ();
				TxtEmployeeDOB = null;
			}

			if (TxtEmployeeAge != null) {
				TxtEmployeeAge.Dispose ();
				TxtEmployeeAge = null;
			}
		}
	}
}
