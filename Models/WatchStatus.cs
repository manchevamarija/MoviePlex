#nullable enable
using System;
using System.ComponentModel.DataAnnotations;  

namespace MoviesApp.Models
{
    public enum WatchStatus
    {
        [Display(Name = "Не е гледан")]
        NotWatched = 0,
        [Display(Name = "Во тек")]
        Watching = 1,
        [Display(Name = "Гледан")]
        Watched = 2,
        [Display(Name = "Омилен")]
        Favorite = 3
    }

}