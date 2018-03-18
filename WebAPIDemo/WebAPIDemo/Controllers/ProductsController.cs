using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
	public class ProductsController : ApiController
	{
		private FabricsEntities db = new FabricsEntities();

		public ProductsController()
		{
			db.Configuration.LazyLoadingEnabled = false; //由於內部的model互相有關連，所以會不停的重複去找同樣的東西
		}

		/// <summary>
		/// 取得所有商品
		/// </summary>
		/// <returns></returns>
		// GET: api/Products
		public IQueryable<Product> Get() //預設只要用Get就可以與controller
		{
			return db.Product.OrderByDescending(x => x.ProductId).Take(10);
		}

		/// <summary>
		/// 取得單一商品
		/// </summary>
		/// <param name="id">產品id</param>
		/// <returns></returns>
		// GET: api/Products/5
		[ResponseType(typeof(Product))]
		public IHttpActionResult GetProduct(int id)
		{
			Product product = db.Product.Find(id);
			if (product == null)
			{
				return NotFound();
			}

			return Ok(product);
		}

		// PUT: api/Products/5
		[ResponseType(typeof(void))]
		public IHttpActionResult PutProduct(int id , Product product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != product.ProductId)
			{
				return BadRequest();
			}

			db.Entry(product).State = EntityState.Modified;

			try
			{
				db.SaveChanges();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProductExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return StatusCode(HttpStatusCode.NoContent);
		}

		// POST: api/Products
		[ResponseType(typeof(Product))]
		public IHttpActionResult PostProduct(Product product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			db.Product.Add(product);
			db.SaveChanges();

			return CreatedAtRoute("DefaultApi" , new { id = product.ProductId } , product);
		}

		// DELETE: api/Products/5
		[ResponseType(typeof(Product))]
		public IHttpActionResult DeleteProduct(int id)
		{
			Product product = db.Product.Find(id);
			if (product == null)
			{
				return NotFound();
			}

			db.Product.Remove(product);
			db.SaveChanges();

			return Ok(product);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool ProductExists(int id)
		{
			return db.Product.Count(e => e.ProductId == id) > 0;
		}
	}
}