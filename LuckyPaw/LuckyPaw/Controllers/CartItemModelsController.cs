using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuckyPaw.Data;
using LuckyPaw.Models;
using LuckyPaw.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace LuckyPaw.Controllers
{
    public class CartItemModelsController : Controller
    {
        private readonly LuckyPawContext _context;

        public CartItemModelsController(LuckyPawContext context)
        {
            _context = context;
        }

        // GET: CartItemModels
        public async Task<IActionResult> Index()
        {
            // Code below copied from http://learningprogramming.net/net/asp-net-core-mvc/build-shopping-cart-with-session-in-asp-net-core-mvc/
            // and updated

            // Get the puppyCart and training services cart
            var puppyCart = SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "puppyCart");
            var trainingServicesCart = SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "trainingServicesCart");

            // If the puppy cart is not null and the training services cart is not null, add the training services cart to 
            // the puppy cart
            if(puppyCart != null && trainingServicesCart != null)
                puppyCart.AddRange(trainingServicesCart);
            
            // If the puppy cart is not null, return the puppy cart to the view
            if (puppyCart != null)
                return View(puppyCart);
            // If the training services cart is not null, return the training services cart to the view
            else if (trainingServicesCart != null)
                return View(trainingServicesCart);
            // Else create a tempCartList to make sure the view model is not null, otherwise it would throw
            // an object reference not set to an object
            else
            {
                List<CartItemModel> tempCartList = new List<CartItemModel>();
                CartItemModel tempCart = new CartItemModel { PricePuppyID = 0, PricePuppyDesc = "", PricePuppy = 0, TrainingServicesPriceID = 0,
                    TrainingName = "", PriceTraining = 0, CartQty = 0, Email = "" };

                tempCartList.Add(tempCart);

                return View(tempCartList);
            }
        }

        // GET: AddOneToTrainingServicesCart
        public async Task<IActionResult> AddOneToTrainingServicesCart(int? id)
        {
            // Code below copied from http://learningprogramming.net/net/asp-net-core-mvc/build-shopping-cart-with-session-in-asp-net-core-mvc/
            // and updated it.

            // Get the training services cart
            List<CartItemModel> cart = SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "trainingServicesCart");

            // Check to see if the training service that was selected is in the training services cart
            int index = isExistTrainingService((int)id);

            // If the index is not equal -1
            if (index != -1)
            {
                // Update the training services cart cartQty
                cart[index].CartQty++;

                // Get the cartItemModel where the training services price id is equal to the selected training service id
                var cartItemModel = await _context.CartItemModel
                .FirstOrDefaultAsync(m => m.TrainingServicesPriceID == id);

                // Check to see if the cart item model is null
                if (cartItemModel == null)
                {
                    return NotFound();
                }

                // Update the cart item model cart quantity
                cartItemModel.CartQty++;

                // If the modelState is valid, update the cart item in the database
                //detail of model state
                //https://stackoverflow.com/questions/881281/what-is-modelstate-isvalid-valid-for-in-asp-net-mvc-in-nerddinner#targetText=ModelState.IsValid%20tells%20you%20if,validation%20system%20you're%20using.

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(cartItemModel);    // Will update the cart table
                        await _context.SaveChangesAsync(); // Save the changes
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        // If any errors, throw an exception
                        throw;
                    }
                }
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "trainingServicesCart", cart);

            return RedirectToAction("Index");
        }

        // GET: RemoveOneFromTrainingServicesCart
        public async Task<IActionResult> RemoveOneFromTrainingServicesCart(int? id)
        {
            // Code below copied from http://learningprogramming.net/net/asp-net-core-mvc/build-shopping-cart-with-session-in-asp-net-core-mvc/
            // and updated it.

            // Get the training services cart
            List<CartItemModel> cart = SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "trainingServicesCart");

            // Check to see if the training service that was selected is in the training services cart
            int index = isExistTrainingService((int)id);

            // If the index is not equal -1
            if (index != -1)
            {
                // If the training services cart for the selected training service cart quantity is 1
                if (cart[index].CartQty == 1)
                {
                    // Remove the selected training services cart item from the training services cart
                    cart.RemoveAt(index);

                    // Get the cart item for the selected training service from the training services cart
                    var cartItemModel = await _context.CartItemModel
                    .FirstOrDefaultAsync(m => m.TrainingServicesPriceID == id);

                    // If the cart item model is null, return not found
                    if (cartItemModel == null)
                    {
                        return NotFound();
                    }

                    // Remove the cart item for the selected training service from the database and save the changes
                    _context.CartItemModel.Remove(cartItemModel);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Else decrement the training 
                    cart[index].CartQty--;

                    var cartItemModel = await _context.CartItemModel
                    .FirstOrDefaultAsync(m => m.TrainingServicesPriceID == id);

                    if (cartItemModel == null)
                    {
                        return NotFound();
                    }

                    cartItemModel.CartQty--;

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(cartItemModel);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            throw;
                        }
                    }
                }
            }
            
            // Update the training services cart to reflect the changes made above to the cart
            SessionHelper.SetObjectAsJson(HttpContext.Session, "trainingServicesCart", cart);

            return RedirectToAction("Index");
        }

        // GET: BuyPuppy, Authorize code from https://www.red-gate.com/simple-talk/dotnet/asp-net/thoughts-on-asp-net-mvc-authorization-and-security/
        // Authorize attribute to redirect the user to the login page if they have not logged in
        [Authorize]
        public async Task<IActionResult> BuyPuppy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Code below copied from http://learningprogramming.net/net/asp-net-core-mvc/build-shopping-cart-with-session-in-asp-net-core-mvc/
            // and updated it.

            // Get the puppy from the PricePuppy table
            var pricingPuppyModel = await _context.PricingPuppyModel.FindAsync(id);

            if (pricingPuppyModel == null)
            {
                return NotFound();
            }

            // Check if the puppy cart has been created or not .i.e is it null 
            if (SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "puppyCart") == null)
            {
                //Create the cart
                List<CartItemModel> cart = new List<CartItemModel>();

                //Creating cart item model object
                CartItemModel newCartItem = new CartItemModel { PricePuppyID = pricingPuppyModel.PricePuppyID, PricePuppyDesc = pricingPuppyModel.PricePuppyDesc,
                    PricePuppy = pricingPuppyModel.PricePuppy, TrainingServicesPriceID = -1,
                    TrainingName = "", PriceTraining = 0, CartQty = 1, Email = User.Identity.Name };

                cart.Add(newCartItem);

                if (ModelState.IsValid)
                {
                    _context.Add(newCartItem);
                    await _context.SaveChangesAsync();
                }

                SessionHelper.SetObjectAsJson(HttpContext.Session, "puppyCart", cart);
            }
            else
            {
                //The cart has been created
                List<CartItemModel> cart = SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "puppyCart");
                int index = isExistPuppy((int)id);

                if (index == -1)
                {
                    CartItemModel newCartItm = new CartItemModel { PricePuppyID = pricingPuppyModel.PricePuppyID, PricePuppyDesc = pricingPuppyModel.PricePuppyDesc,
                                               PricePuppy = pricingPuppyModel.PricePuppy, TrainingServicesPriceID = -1, TrainingName = "", PriceTraining = 0, CartQty = 1,
                                               Email = User.Identity.Name };
                    cart.Add(newCartItm);

                    if (ModelState.IsValid)
                    {
                        _context.Add(newCartItm);
                        await _context.SaveChangesAsync();
                    }
                }

                SessionHelper.SetObjectAsJson(HttpContext.Session, "puppyCart", cart);
            }

   

            return RedirectToAction("Index");
        }


        // GET: BuyTrainingService, Authorize code from https://www.red-gate.com/simple-talk/dotnet/asp-net/thoughts-on-asp-net-mvc-authorization-and-security/
        [Authorize]
        public async Task<IActionResult> BuyTrainingService(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            // Code below copied from http://learningprogramming.net/net/asp-net-core-mvc/build-shopping-cart-with-session-in-asp-net-core-mvc/
            // and updated it.

            // Get the training service
            var trainingServicesPriceModel = await _context.TrainingServicesPriceModel.FindAsync(id);

            if (trainingServicesPriceModel == null)
            {
                return NotFound();
            }

            //Check if the TrainingServices Cart has been created or not .i.e. is it null
            if (SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "trainingServicesCart") == null)
            {
                List<CartItemModel> cart = new List<CartItemModel>();

                CartItemModel newCartItem = new CartItemModel { PricePuppyID = -1, PricePuppyDesc = "", PricePuppy = 0,
                                             TrainingServicesPriceID = trainingServicesPriceModel.TrainingServicesPriceID, TrainingName = trainingServicesPriceModel.TrainingName,
                                             PriceTraining = trainingServicesPriceModel.PriceTraining, CartQty = 1, Email = User.Identity.Name };

                cart.Add(newCartItem);

                if (ModelState.IsValid)
                {
                    _context.Add(newCartItem);
                    await _context.SaveChangesAsync();
                }

                //Update the cart with newly created cart
                SessionHelper.SetObjectAsJson(HttpContext.Session, "trainingServicesCart", cart);
            }
            else
            {
                //Training Services Cart is already exist
                List<CartItemModel> cart = SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "trainingServicesCart");
                int index = isExistTrainingService((int)id);
                if (index != -1)
                {
                    cart[index].CartQty++;

                    var cartItemModel = await _context.CartItemModel
                    .FirstOrDefaultAsync(m => m.TrainingServicesPriceID == id);

                    if (cartItemModel == null)
                    {
                        return NotFound();
                    }

                    cartItemModel.CartQty++;

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(cartItemModel);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                           throw;
                        }
                    }
                }
                else
                {
                    CartItemModel newCartItm = new CartItemModel {PricePuppyID = -1, PricePuppyDesc = "", PricePuppy = 0,
                                               TrainingServicesPriceID = trainingServicesPriceModel.TrainingServicesPriceID, TrainingName = trainingServicesPriceModel.TrainingName,
                                               PriceTraining = trainingServicesPriceModel.PriceTraining, CartQty = 1, Email = User.Identity.Name };

                    cart.Add(newCartItm);

                    if (ModelState.IsValid)
                    {
                        _context.Add(newCartItm);
                        await _context.SaveChangesAsync();
                    }
                }

                //Updating the session with newly creted or updated Cart
                SessionHelper.SetObjectAsJson(HttpContext.Session, "trainingServicesCart", cart);
            }

            return RedirectToAction("Index");

        }
        

        
        // GET: Remove puppy
        public async Task<IActionResult> RemovePuppy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<CartItemModel> puppyCart = SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "puppyCart");
            int index = isExistPuppy((int)id);

            if (index != -1)
            {
                puppyCart.RemoveAt(index);

                var cartItemModel = await _context.CartItemModel
                    .FirstOrDefaultAsync(m => m.PricePuppyID == id);

                if (cartItemModel == null)
                {
                    return NotFound();
                }

                //removed the cart item puppy from database
                _context.CartItemModel.Remove(cartItemModel); 
                await _context.SaveChangesAsync();
            }

            //Update the session of puppy cart
            SessionHelper.SetObjectAsJson(HttpContext.Session, "puppyCart", puppyCart); 
           
            return RedirectToAction("Index");
        }
        

        // GET: Remove training service
        public async Task<IActionResult> RemoveTrainingService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<CartItemModel> trainingServicesCart = SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "trainingServicesCart");
            int index = isExistTrainingService((int)id);

            if (index != -1)
            {
                trainingServicesCart.RemoveAt(index);

                var cartItemModel = await _context.CartItemModel
                   .FirstOrDefaultAsync(m => m.TrainingServicesPriceID == id);

                if (cartItemModel == null)
                {
                    return NotFound();
                }

                _context.CartItemModel.Remove(cartItemModel);
                await _context.SaveChangesAsync();
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "trainingServicesCart", trainingServicesCart);

            return RedirectToAction("Index");
        }


        //Checkout the products
        public async Task<IActionResult> CheckOut()
        {
            List<CartItemModel> puppyCart = SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "puppyCart");
            List<CartItemModel> trainingServicesCart = SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "trainingServicesCart");

            if (puppyCart != null && trainingServicesCart != null)
                puppyCart.AddRange(trainingServicesCart);

            double puppyTotal = 0;
            double trainingServicesTotal = 0;

            // If the puppy cart is not null, loop through the puppies in the cart, a puppy has a training Services price id
            // of -1 and update the puppyTotal and remove the puppy from the database. If the price puppy id is -1,
            // it means we have a training service, update the training services total
            if (puppyCart != null)
            {

                for (int i = 0; i < puppyCart.Count; i++)
                {
                    if (puppyCart[i].TrainingServicesPriceID == -1)
                    {
                        puppyTotal += puppyCart[i].PricePuppy;

                        // Get the puppy 
                        var pricingPuppyModel = await _context.PricingPuppyModel.FindAsync(puppyCart[i].PricePuppyID);

                        if (pricingPuppyModel == null)
                        {
                            return NotFound();
                        }

                        _context.PricingPuppyModel.Remove(pricingPuppyModel);
                        await _context.SaveChangesAsync();
                    }
                    else if (puppyCart[i].PricePuppyID == -1)
                    {
                        trainingServicesTotal += puppyCart[i].PriceTraining * puppyCart[i].CartQty;
                    }
                }

                // Update the view data for the puppy total and training services total
                ViewData["puppyTotal"] = puppyTotal;
                ViewData["trainingServicesTotal"] = trainingServicesTotal;

                // Empty the puppy cart and training services cart.
                List<CartItemModel> emptyCart = new List<CartItemModel>();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "puppyCart", emptyCart);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "trainingServicesCart", emptyCart);

                return View(puppyCart);
            }
            // If the training services cart is not null, update the training services total
            else if (trainingServicesCart != null) {

                for (int i = 0; i < trainingServicesCart.Count; i++)
                {
                    if (trainingServicesCart[i].PricePuppyID == -1)
                    {
                        trainingServicesTotal += trainingServicesCart[i].PriceTraining * trainingServicesCart[i].CartQty;
                    }
                }

                // Update the view data for the puppy total and training services total
                ViewData["puppyTotal"] = puppyTotal;
                ViewData["trainingServicesTotal"] = trainingServicesTotal;

                // Empty the puppy cart and traiing services cart
                List<CartItemModel> emptyCart1 = new List<CartItemModel>();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "puppyCart", emptyCart1);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "trainingServicesCart", emptyCart1);

                return View(trainingServicesCart);
            }

            // Update the view data for the puppy total and training services total
            ViewData["puppyTotal"] = puppyTotal;
            ViewData["trainingServicesTotal"] = trainingServicesTotal;

            // Empty the puppy cart and training services cart
            List<CartItemModel> emptyCart2 = new List<CartItemModel>();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "puppyCart", emptyCart2);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "trainingServicesCart", emptyCart2);

            return View();
        }

        // Cart items view for the manager and admin to see what puppies or training services were bought by the user
        public async Task<IActionResult> CartItemsView()
        {
            return View(await _context.CartItemModel.ToListAsync());
        }

        // GET: CartItemModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItemModel = await _context.CartItemModel
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cartItemModel == null)
            {
                return NotFound();
            }

            return View(cartItemModel);
        }

        // GET: CartItemModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CartItemModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,PricePuppyID,PricePuppyDesc,PricePuppy,TrainingServicesPriceID,TrainingName,PriceTraining,CartQty,Email")] CartItemModel cartItemModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartItemModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cartItemModel);
        }

        // GET: CartItemModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItemModel = await _context.CartItemModel.FindAsync(id);
            if (cartItemModel == null)
            {
                return NotFound();
            }
            return View(cartItemModel);
        }

        // POST: CartItemModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,PricePuppyID,PricePuppyDesc,PricePuppy,TrainingServicesPriceID,TrainingName,PriceTraining,CartQty,Email")] CartItemModel cartItemModel)
        {
            if (id != cartItemModel.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItemModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemModelExists(cartItemModel.CartId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cartItemModel);
        }

        // GET: CartItemModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItemModel = await _context.CartItemModel
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cartItemModel == null)
            {
                return NotFound();
            }

            return View(cartItemModel);
        }

        // POST: CartItemModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItemModel = await _context.CartItemModel.FindAsync(id);
            _context.CartItemModel.Remove(cartItemModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartItemModelExists(int id)
        {
            return _context.CartItemModel.Any(e => e.CartId == id);
        }

        // Code below copied from http://learningprogramming.net/net/asp-net-core-mvc/build-shopping-cart-with-session-in-asp-net-core-mvc/
        // and updated it.

        private int isExistPuppy(int id)
        {
            List<CartItemModel> cart = SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "puppyCart");

            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].PricePuppyID.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        private int isExistTrainingService(int id)
        {
            List<CartItemModel> cart = SessionHelper.GetObjectFromJson<List<CartItemModel>>(HttpContext.Session, "trainingServicesCart");

            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].TrainingServicesPriceID.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

    }
}
