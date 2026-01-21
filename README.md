
#  MoviePlex MVC

MoviePlex MVC is a web application built with **ASP.NET Core MVC** that allows users to search for movies via the **OMDb API**, view detailed information, save movies to a personal list, add personal notes, and set statuses such as *"Want to Watch"*. All user data is securely stored in a local database.

---

##  Features

-  **Search movies by title** using the OMDb API  
-  **View detailed movie information**: title, year, genre, description, poster
-  **Save movies** to a personal list  
-  **Add personal notes** to movies  
-  **Set statuses** for movies (e.g., “Want to Watch”)  
-  **Local database storage** for all user data  

---

##  Cineplexx Macedonia Integration

MoviePlex MVC now includes **real-time integration with Cineplexx Macedonia** through **web scraping**, adding localized and dynamic content:

-  **Web scraping** of Cineplexx Macedonia’s official website  
-  **Display movies currently showing** in theaters this week  
-  **Show upcoming movie releases**  
-  **Reserve tickets directly** by redirecting users to the official Cineplexx reservation page  
-  **Watch official trailers** for movies 

This integration enriches the app with **live cinema data**, allowing users to browse, plan, and book tickets all in one place.

---

## Use Case Diagram

The following diagram presents the main use cases of the MoviePlex MVC application, showing how users interact with the system to search, view, save, and manage movies, as well as how the application communicates with external APIs.

![Use Case Diagram](use-case-diagram)

---


##  Technologies

- **ASP.NET Core MVC**  
- **Entity Framework Core**  
- **SQL Server / LocalDB**  
- **Razor Views**  
- **OMDb API Integration**  
- **Web Scraping** (Cineplexx Macedonia)
