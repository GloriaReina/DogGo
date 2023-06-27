using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    public class DogsController : Controller
    {
        private readonly ILogger<DogsController> _logger;

        private readonly IDogRepository _dogRepo;

        // Constructor / ASP.NET will give us an instance of our Dog Repository when creating an instance of DogsController. This is called "Dependency Injection"
        public DogsController(IDogRepository dogRepository)
        {
            _dogRepo = dogRepository;
        }



        // GET: DogsController
        public ActionResult Index()
        {
            List<Dog> dogslist = _dogRepo.GetAllDogs();
            return View(dogslist);
        }

        // GET: DogsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DogsController/Create
        [HttpGet]//Specified the HTTP attributes ([HttpGet] and [HttpPost])=> make sure correct method invoked based on the request type since have 2 method with name create.
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


        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                _dogRepo.AddDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        // GET: DogsController/Edit/5

        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }
            return View(dog);
        }

        // POST: DogsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                _dogRepo.UpdateDog(dog);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(dog);
            }
        }


        // GET: DogsController/Delete/5
      
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            return View(dog);
        }

        // POST: DogsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
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
