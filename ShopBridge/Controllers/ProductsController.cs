using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Products;

namespace ShopBridge.Controllers
{

    public class ProductsController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> AllProducts()
        {
            using (ShopBridgeEntities entities = new ShopBridgeEntities())
            {
                return await Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, entities.Products.ToList()));
            }
        }


        [HttpGet]
        public async Task<HttpResponseMessage> GetSpecificProduct(int productId)
        {
            using (ShopBridgeEntities entities = new ShopBridgeEntities())
            {
                var prods = await Task.FromResult(entities.Products.FirstOrDefault(e => e.ProductId == productId));
                if (prods != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, prods);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The Product " + productId + " is not found in the inventory");
                }
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> AddingProduct([FromBody]Product products)
        {
            try
            {

                using (ShopBridgeEntities entities = new ShopBridgeEntities())
                {
                    entities.Products.Add(products);
                    await Task.FromResult(entities.SaveChanges());
                    var msg = Request.CreateResponse(HttpStatusCode.Created, products);
                    msg.Headers.Location = new Uri(Request.RequestUri + products.Name.ToString());

                    return msg;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeletingProducts(int productId)
        {
            try
            {
                using (ShopBridgeEntities entities = new ShopBridgeEntities())
                {
                    var prod = entities.Products.Where(x => x.ProductId == productId).FirstOrDefault();
                    if (prod != null)
                    {
                        entities.Products.Remove(prod);
                        await Task.FromResult(entities.SaveChanges());

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The product " + productId + " was not found");
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdatingProducts(int productId, [FromBody]Product products)
        {
            try
            {
                using (ShopBridgeEntities entities = new ShopBridgeEntities())
                {
                    var prod = entities.Products.FirstOrDefault(x => x.ProductId == productId);
                    if (prod != null)
                    {
                        prod.Name = products.Name;
                        prod.Description = products.Description;
                        prod.Price = products.Price;
                        prod.ProdAddedDate = products.ProdAddedDate;
                        prod.Availability = products.Availability;
                        prod.Brand = products.Brand;
                        await Task.FromResult(entities.SaveChanges());

                        return Request.CreateResponse(HttpStatusCode.OK, products);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The product " + productId + " was not found to update");
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}
