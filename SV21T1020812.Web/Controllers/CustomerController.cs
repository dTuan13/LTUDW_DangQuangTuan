﻿using Microsoft.AspNetCore.Mvc;
using SV21T1020812.BusinessLayers;
using SV21T1020812.DomainModels;

namespace SV21T1020812.Web.Controllers
{
	public class CustomerController : Controller
	{
		public const int PAGE_SIZE = 20;
		public IActionResult Index(int page = 1, string searchValue = "")
		{
			int rowCount;
			var data = CommonDataService.ListOfCustomers(out rowCount, page, PAGE_SIZE, searchValue ?? "");

			int pageCount = rowCount / PAGE_SIZE;
			if (rowCount % PAGE_SIZE > 0) pageCount += 1;

			ViewBag.Page = page;
			ViewBag.RowCount = rowCount;
			ViewBag.PageCount = pageCount;
			ViewBag.SearchValue = searchValue;
			
			return View(data);
		}

		public IActionResult Create()
		{
			ViewBag.Title = "Bổ sung khách hàng";
			var data = new Customer()
			{
				CustomerId = 0,
				IsLocked = false,

			};
			return View("Edit", data);
		}

		public IActionResult Edit(int id = 0)
		{
			ViewBag.Title = "Cập nhật thông tin khách hàng";
			var data = CommonDataService.GetCustomer(id);
			if (data == null) {
				return RedirectToAction("Index");
			}

			return View(data);
		}
		[HttpPost]
		public IActionResult Save(Customer data)
		{
            if(data.CustomerId == 0)
			{
				CommonDataService.AddCustomer(data);
			} else
			{
				CommonDataService.UpdateCustomer(data);
			}
			return RedirectToAction("Index");
        }

		public IActionResult Delete(int id = 0)
		{
			if(Request.Method == "POST")
			{
				CommonDataService.DeleteCustomer(id);
                return RedirectToAction("Index");
            }

			var data = CommonDataService.GetCustomer(id);
			if(data == null)
			{
				return RedirectToAction("Index");
			}
			return View(data);
		}
	}
}
