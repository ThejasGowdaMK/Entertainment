using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.SqlClient;
using WebApplication17.Models;
using System.Data.Entity;

namespace WebApplication17.Controllers
{
    [EnableCors(methods:"*",headers:"*",origins:"*")]
    public class DefaultController : ApiController
    {
        EntertainmentEntities2 db = new EntertainmentEntities2();

        [HttpGet]
        [Route("api/Default/Movies")]
        public List<Class1> GetMovies()
        {
            List<Class1> listOfMovies = new List<Class1>();

            var totalNoOfMovies = db.Movies.ToList();
            for (int i = 0; i < totalNoOfMovies.Count; i++)
            {
                string movieName = totalNoOfMovies[i].Movie_Name;
                DateTime releaseDate = totalNoOfMovies[i].Release_Date;
                int b = totalNoOfMovies[i].PID;
                var producer = db.Producers.Where(x => x.PID == b).SingleOrDefault().Producer_Name;
                int a = totalNoOfMovies[i].MID;
                var Actors = db.Actors_Movie.Where(x => x.MID==a).ToList();
                List<string> actors = new List<string>();
                for(int j = 0; j < Actors.Count; j++)
                {
                    int z = Actors[j].AID;
                    actors.Add(db.Actors.Where(x => x.AID == z).SingleOrDefault().C_Name);
                }
                listOfMovies.Add(new Class1(movieName, releaseDate, producer, actors));
            }
            return listOfMovies;


        }

        [HttpPost]
        [Route("api/Default/Movies")]

        public bool InsertMovie(Class1 newMovie)
        {
            try
            {
                if(string.IsNullOrEmpty(newMovie.Movie_Name)|| string.IsNullOrEmpty(newMovie.Producer) || newMovie.Actors.Count == 0 || newMovie.Release_Date == null)
                {
                    throw new Exception("All the fileds of film are mandatory, please check if any of the filed is empty");
                }
                string movieName = newMovie.Movie_Name;
                var checkIfMovieIsAlreadyPresent = db.Movies.Where(x => x.Movie_Name == movieName).SingleOrDefault();
                if (checkIfMovieIsAlreadyPresent != null)
                {
                    throw new Exception("Movie is already present in Database");
                }
                else
                {
                    Movy movie = new Movy();
                    movie.Movie_Name = newMovie.Movie_Name;
                    movie.Release_Date = newMovie.Release_Date;
                    movie.PID = db.Producers.Where(x => x.Producer_Name == newMovie.Producer).SingleOrDefault().PID;
                    db.Movies.Add(movie);
                    db.SaveChanges();
                    Actors_Movie actors_Movie = new Actors_Movie();
                    actors_Movie.MID = db.Movies.Max(x => x.MID);
                    var res = 0;
                    for (int i = 0; i < newMovie.Actors.Count; i++)
                    {
                        string Actors_Name = newMovie.Actors[i].ToString();
                        actors_Movie.AID = db.Actors.Where(x => x.C_Name == Actors_Name).SingleOrDefault().AID;
                        db.Actors_Movie.Add(actors_Movie);
                        res = db.SaveChanges();
                    }

                    if (res > 0)
                    {
                        return true;
                    }
                    else
                        throw new Exception("Movie Cannot be inserted, please verify all the detalis and try again");
                }
            }
            catch
            {
                throw;
            }
        }


        [HttpPut]
        [Route("api/Default/editMovie")]
        public bool movie(Class1 editmovies)
        {
            try
            {
                var isMovieInDb = db.Movies.Where(x => x.Movie_Name == editmovies.Movie_Name).SingleOrDefault();
                if (isMovieInDb != null)
                {
                    Actors_Movie actors_Movie = new Actors_Movie();
                    actors_Movie.MID = db.Movies.Where(x => x.Movie_Name == editmovies.Movie_Name).SingleOrDefault().MID;
                    var movie = db.Movies.Where(x => x.Movie_Name == editmovies.Movie_Name).SingleOrDefault();
                    movie.Release_Date = editmovies.Release_Date;
                    movie.PID = db.Producers.Where(x => x.Producer_Name == editmovies.Producer).SingleOrDefault().PID;
                    var actors = movie.Actors_Movie;
                    db.SaveChanges();
                    var remove = db.Actors_Movie.Where(x => x.MID == actors_Movie.MID).ToList();
                    for (int r = 0; r < remove.Count; r++)
                    {
                        db.Actors_Movie.Remove(remove[r]);
                    }
                    db.SaveChanges();
                    var res = 0;
                    for (int a = 0; a < editmovies.Actors.Count(); a++)
                    {
                        string Actors_Name = editmovies.Actors[a].ToString();
                        int add = db.Actors.Where(x => x.C_Name == Actors_Name).SingleOrDefault().AID;
                        actors_Movie.AID = add;
                        db.Actors_Movie.Add(actors_Movie);
                        res = db.SaveChanges();
                    }
                    if (res > 0)
                    {
                        return true;
                    }
                    else
                        throw new Exception("Movie Cannot be edited, please verify all the detalis and try again");
                }
                else
                {
                    throw new Exception("Movie is not present in database please verify");
                }
            }
            catch
            {
                throw;
            }
        }
    }
}