using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirportAPI.Data;
using AirportAPI.Entities.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace AirportWebsite.Controllers
{
    
    public class FlightsController : Controller
    {
        private Uri ApiUrl { get; set; }
        private readonly HttpClient _httpClient;

        public FlightsController(AirportContext context)
        {
            ApiUrl = new Uri("https://localhost:44356/api/flights");
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync(ApiUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var flights = await response.Content.ReadAsStringAsync();
            return View(JsonConvert.DeserializeObject<List<Flight>>(flights));
        }

        // GET: Flights
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] string fromLocation, [FromForm] string toLocation)
        {
            var response = await _httpClient.GetAsync(ApiUrl + $"/{fromLocation}/{toLocation}", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var flights = await response.Content.ReadAsStringAsync();
            return View(JsonConvert.DeserializeObject<List<Flight>>(flights));
        }

        // GET: Flights/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightId,AircraftType,FromLocation,ToLocation,DepartureTime,ArrivalTime")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync(ApiUrl, flight);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync(ApiUrl + $"/{id}", HttpCompletionOption.ResponseHeadersRead);
            var data = await response.Content.ReadAsStringAsync();
            var flight = JsonConvert.DeserializeObject<Flight>(data);

            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightId,AircraftType,FromLocation,ToLocation,DepartureTime,ArrivalTime")] Flight flight)
        {
            if (id != flight.FlightId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _httpClient.PutAsJsonAsync(ApiUrl + $"/{id}", flight);
                    response.EnsureSuccessStatusCode();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await FlightExists(flight.FlightId))
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
            return View(flight);
        }

        private async Task<bool> FlightExists(int id)
        {
            var response = await _httpClient.GetAsync(ApiUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var context = JsonConvert.DeserializeObject<List<Flight>>(data);
            return context.Any(e => e.FlightId == id);
        }
    }
}
