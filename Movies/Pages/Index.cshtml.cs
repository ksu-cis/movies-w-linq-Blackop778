using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Movies.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<Movie> Movies;

        [BindProperty]
        public string search { get; set; }

        [BindProperty]
        public List<string> mpaa { get; set; } = new List<string>();

        [BindProperty]
        public float? minIMDB { get; set; }

        [BindProperty]
        public float? maxIMDB { get; set; }

        [BindProperty]
        public string Sort { get; set; }

        public void OnGet()
        {
            Movies = MovieDatabase.All.OrderBy(movie => movie.Title) ;
        }

        public void OnPost()
        {
            Movies = MovieDatabase.All;

            if (search != null)
            {
                Movies = Movies.Where(movie => (movie.Director != null && movie.Director.Contains(search, StringComparison.OrdinalIgnoreCase)) || movie.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if(mpaa.Count != 0)
            {
                Movies = Movies.Where(movie => mpaa.Contains(movie.MPAA_Rating));
            }

            if(minIMDB != null)
            {
                Movies = Movies.Where(movie => movie.IMDB_Rating != null && minIMDB <= movie.IMDB_Rating);
            }

            if (maxIMDB != null)
            {
                Movies = Movies.Where(movie => movie.IMDB_Rating != null && maxIMDB >= movie.IMDB_Rating);
            }
            
            if (Sort != null)
            {
                switch(Sort)
                {
                    default:
                    case "Title":
                        Movies = Movies.OrderBy(movie => movie.Title);
                        break;
                    case "Director":
                        Movies = Movies.OrderBy(movie => movie.Director ?? "");
                        break;
                    case "Year":
                        Movies = Movies.OrderBy(movie => movie.Release_Year);
                        break;
                    case "IMDB":
                        Movies = Movies.OrderBy(movie => movie.IMDB_Rating);
                        break;
                    case "Tomatoes":
                        Movies = Movies.OrderBy(movie => movie.Rotten_Tomatoes_Rating);
                        break;
                }
            }
        }
    }
}
