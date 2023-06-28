using DogGo.Models;
using DogGo.Repositories;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogGo.Controllers
{
    public class DogsController : Controller
    {
        private readonly ILogger<DogsController> _logger;

        private readonly IDogRepository _dogRepo;

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        // Constructor / ASP.NET will give us an instance of our Dog Repository when creating an instance of DogsController. This is called "Dependency Injection"
        public DogsController(IDogRepository dogRepository)
        {
            _dogRepo = dogRepository;
        }



        //// GET: DogsController Chapter 3
        //public ActionResult Index()
        //{
        //    List<Dog> dogslist = _dogRepo.GetAllDogs();
        //    return View(dogslist);
        //}

        [Authorize]//stops unauthenticated users from accessing /dogs...must be logged in 
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);

            return View(dogs);
        }

        // GET: DogsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DogsController/Create
        [HttpGet]//Specified the HTTP attributes ([HttpGet] and [HttpPost])=> make sure correct method invoked based on the request type since have 2 method with name create.
        [Authorize] //Stops unauthenticated users from accessing /dogs/create route...will be redirected to login page
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogsController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public ActionResult Create(Dog dog)
        //{
        //    try
        //    {
        //        _dogRepo.AddDog(dog);

        //        ModelState.Clear(); // Clear model state to remove previous values after form submission

        //        //redirects the user to the "Index" action (list of dogs) 
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        //return the same "Create" view with the submitted form data
        //        return View(dog);
        //    }
        //}


        // POST: DogController/Create:Defaulting the OwnerId when creating dogs =>so user dont have to add ownerID when creating a new dog profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                // update the dogs OwnerId to the current user's Id
                dog.OwnerId = GetCurrentUserId();
                _dogRepo.AddDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        // GET: DogsController/Edit/5

        [Authorize]
        public ActionResult Edit(int id)
        {

            Dog dog = _dogRepo.GetDogById(id);
            

            if (dog == null)
            {
                return NotFound("Ooops! Dog Not Found Database");
            }
            // Check if the current user owns the dog
            int currentUserId = GetCurrentUserId();
            
            if (dog.OwnerId != currentUserId)
            {
                return NotFound("Not Authorized!!!");
            }

            return View(dog);
        }
          
        // POST: DogsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                Dog dogInDB = _dogRepo.GetDogById(id);

                if (dogInDB == null)
                {
                    return NotFound("OOPS! Dog not fough in our database!");
                }
                // Check if the current user owns the dog
                int currentUserId = GetCurrentUserId();

                if (dogInDB.OwnerId != currentUserId)
                {
                    return NotFound("Not Authorized!!!");
                }
                else
                {
                    dogInDB.Name = dog.Name;
                    dogInDB.Breed = dog.Breed;
                    dogInDB.ImageUrl = dog.ImageUrl;
                    dogInDB.Notes = dog.Notes;
                    dogInDB.OwnerId = currentUserId;

                    _dogRepo.UpdateDog(dogInDB);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View(dog);
            }
        }


        // GET: DogsController/Delete/5

        [Authorize]
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);


            if (dog == null)
            {
                return NotFound("Oops! Dog not found!");
            }
            // Check if the current user owns the dog
            int currentUserId = GetCurrentUserId();

            if (dog.OwnerId != currentUserId)
            {
                return NotFound("Accesss Denied!");
            }

            return View(dog);
        }


        // POST: DogsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(int id, Dog dog)
        {
            Dog dogInDB = _dogRepo.GetDogById(id);


            if (dogInDB == null)
            {
                return NotFound();
            }
            // Check if the current user owns the dog
            int currentUserId = GetCurrentUserId();

            if (dogInDB.OwnerId != currentUserId)
            {
                return NotFound();
            }

            try
            {

                _dogRepo.DeleteDog(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(dog);
            }
        }
    }
}

