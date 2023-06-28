using System.ComponentModel;

namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }
        [DisplayName("Dog Name")]
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Notes { get; set; }

        [DisplayName("Picture")]
        public string ImageUrl { get; set; }
        
        
        [DisplayName("Owner Name")]
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
    }
}
