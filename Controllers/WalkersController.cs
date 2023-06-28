using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly ILogger<WalkersController> _logger;

        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;
        private readonly IOwnerRepository _ownerRepo;

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        // Constructor / ASP.NET will give us an instance of our Walker Repository when creating an instance of WalkersController. This is called "Dependency Injection"
        public WalkersController(IWalkerRepository walkerRepository, IWalkRepository walkRepository, IOwnerRepository ownerRepo)
        {
            _walkerRepo = walkerRepository;
            _walkRepo = walkRepository;
            _ownerRepo = ownerRepo;
        }


        // Code will get all the walkers in the Walker table, convert it to a List and pass it off to the view.
        public ActionResult Index()
        {
            try
            {
                int ownerId = GetCurrentUserId();

                Owner logInOwner = _ownerRepo.GetOwnerById(ownerId);

                if (logInOwner == null || logInOwner.Id != ownerId)
                {
                    return NotFound();

                }

                else
                {

                    List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(logInOwner.NeighborhoodId);

                    return View(walkers);
                }
            }
            catch (Exception ex)
            {
                List<Walker> walkers = _walkerRepo.GetAllWalkers();
                return View(walkers);
            }

        }


        // GET: When the ASP.NET framework invokes this method for us, it will take whatever value is in the url and pass it to the Details method ie. Walkers/Details/5--> pass value of 5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            
            List<Walk> walkList = _walkRepo.GetWalksByWalkerId(walker.Id);
           
            if (walker == null)
            {
                return NotFound();
            }

            WalkerViewModel vm = new WalkerViewModel()
            {
                Walker = walker,
                WalkList = walkList
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}