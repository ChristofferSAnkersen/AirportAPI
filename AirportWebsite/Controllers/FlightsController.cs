﻿using System;
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
        private readonly AirportContext _context;

        public FlightsController(AirportContext context)
        {
            _context = context;
            ApiUrl = new Uri("https://localhost:44356/api/flights/");
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
        //[HttpGet]
        //public async Task<IActionResult> Index(string searchString)
        //{
        //    //flight.FromLocation = searchString;

        //    //var response = await _httpClient.GetAsync(ApiUrl, HttpCompletionOption.ResponseHeadersRead);
        //    //response.EnsureSuccessStatusCode();
        //    //var jsonFlights = await response.Content.ReadAsStringAsync();
        //    //var flights = (JsonConvert.DeserializeObject<List<Flight>>(jsonFlights));

        //    var flights = from m in _context.Flights
        //                 select m;

        //    if (!String.IsNullOrEmpty(searchString))
        //        {
        //            flights = flights.Where(s => s.FromLocation.Contains(searchString));
        //        }

        //    return View(await flights.ToListAsync());

        //}

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
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
                _context.Add(flight);
                await _context.SaveChangesAsync();
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

            var flight = await _context.Flights.FindAsync(id);
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
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.FlightId))
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

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.FlightId == id);
        }
    }
}
