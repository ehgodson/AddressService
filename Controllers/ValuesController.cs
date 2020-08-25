using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AddressService.Controllers
{
    [Route("")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly AppDB _db;

        public IndexController(AppDB db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<string> Get() => "Address Service 1.1.15";

        [HttpGet("streets/{query?}")]
        public ActionResult<string> SearchStreets(string query = "")
        {
            try
            {
                return Ok(
                    _db.Streets.Where(o => o.Description.ToLower().Contains(query.ToLower()) ||
                                           o.Area.Description.ToLower().Contains(query.ToLower()) ||
                                           o.Area.Town.Description.ToLower().Contains(query.ToLower()) ||
                                           o.Area.Town.State.Description.ToLower().Contains(query.ToLower()))
                             .Select(x => 
                                new {
                                    x.Id,
                                    description = $"{x.Description}, {x.Area.Description}, {x.Area.Town.Description}, {x.Area.Town.State.Description}"
                                })
                             .ToList());
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occured: {ex.Message}");
            }
        }

        [HttpGet("areas/{query?}")]
        public ActionResult<string> SearchAreas(string query = "")
        {
            try
            {
                // var areas = 
                //     DBContext.Areas.FromSql(
                //         "SELECT TOP * FROM Areas WHERE (Description = @query) ORDER BY Description",
                //         new[] { new SqlParameter("@query", query) });

                return Ok(
                    _db.Areas.Where(o => o.Description.ToLower().Contains(query.ToLower()) ||
                                         o.Town.Description.ToLower().Contains(query.ToLower()) ||
                                         o.Town.State.Description.ToLower().Contains(query.ToLower()))
                             .Select(x => 
                                new {
                                    x.Id,
                                    description = $"{x.Description}, {x.Town.Description}, {x.Town.State.Description}"
                                })
                             .ToList());
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occured: {ex.Message}");
            }
        }

        [HttpGet("towns/{query?}")]
        public ActionResult<string> SearchTowns(string query = "")
        {
            try
            {
                return Ok(
                    _db.Towns.Where(o => o.Description.ToLower().Contains(query.ToLower()) ||
                                         o.State.Description.ToLower().Contains(query.ToLower()))
                             .Select(x => 
                                new {
                                    x.Id,
                                    description = $"{x.Description}, {x.State.Description}"
                                })
                             .ToList());
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occured: {ex.Message}");
            }
        }

        [HttpGet("states/{query?}")]
        public ActionResult<string> SearchStates(string query = "")
        {
            try
            {
                return Ok(
                    _db.States.Where(o => o.Description.ToLower().Contains(query.ToLower()))
                             .Select(x => 
                                new {
                                    x.Id,
                                    description = $"{x.Description}"
                                })
                             .ToList());
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occured: {ex.Message}");
            }
        }

        // [HttpGet("save")]
        // public IActionResult SaveData()
        // {
        //    try
        //    {
        //        string json = string.Empty;

        //        using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\data\\states-towns-areas-streets.json"))
        //        {
        //            json = sr.ReadToEnd();
        //        }

        //        var states = JsonConvert.DeserializeObject<List<State>>(json);

        //        //_db.States.AddRange(states);
        //        //_db.SaveChanges();

        //        return Ok(states);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error occured: {ex.ToString()}");
        //    }
        // }

        

        //[HttpGet("load")]
        //public async Task<IActionResult> LoadData()
        //{
        //    string currentState = "", currentTown = "", currentArea = "";

        //    try
        //    {
        //        var states = Tools.GetStates().Where(x => x.Id == 25);

        //        foreach (var state in states)
        //        {
        //            currentState = $"{state.Id} - {state.Description}";

        //            string stateXml = await Tools.GetFromBaseAsync(UrlType.Town, state.Id);
        //            stateXml = stateXml.Replace("        </option>", string.Empty);

        //            XmlDocument stateDoc = new XmlDocument();
        //            stateDoc.LoadXml(stateXml.Replace("&", "&amp;"));

        //            foreach (XmlNode townNode in stateDoc.FirstChild)
        //            {
        //                if (townNode.Attributes[0].Value != "0")
        //                {
        //                    var town =
        //                        new Town { Id = Convert.ToInt32(townNode.Attributes[0].Value), Description = townNode.InnerText };

        //                    currentTown = $"{town.Id} - {town.Description}";

        //                    string townXml = await Tools.GetFromBaseAsync(UrlType.Area, town.Id);
        //                    townXml = townXml.Replace("        </option>", string.Empty);

        //                    XmlDocument townDoc = new XmlDocument();
        //                    townDoc.LoadXml(townXml.Replace("&", "&amp;"));

        //                    foreach (XmlNode areaNode in townDoc.FirstChild)
        //                    {
        //                        if (areaNode.Attributes[0].Value != "0")
        //                        {
        //                            var area =
        //                                new Area { Id = Convert.ToInt32(areaNode.Attributes[0].Value), Description = areaNode.InnerText };

        //                            currentArea = $"{area.Id} - {area.Description}";

        //                            string areaXml = await Tools.GetFromBaseAsync(UrlType.Street, area.Id);
        //                            areaXml = areaXml.Replace("        </option>", string.Empty);

        //                            XmlDocument areaDoc = new XmlDocument();
        //                            areaDoc.LoadXml(areaXml.Replace("&", "&amp;"));

        //                            foreach (XmlNode streetNode in areaDoc.FirstChild)
        //                            {
        //                                if (streetNode.Attributes[0].Value != "0")
        //                                    area.Streets.Add(new Street { Id = Convert.ToInt32(streetNode.Attributes[0].Value), Description = streetNode.InnerText });
        //                            }

        //                            town.Areas.Add(area);
        //                        }
        //                    }

        //                    state.Towns.Add(town);
        //                }
        //            }
        //        }

        //        return Ok(states);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error occured: {ex.ToString()}\nState: {currentState}\nTown: {currentTown}\nArea: {currentArea}");
        //    }
        //}

        //[HttpGet("str")]
        //public async Task<IActionResult> LoadStreetsAsync()
        //{
        //    string currentState = "", currentTown = "", currentArea = "";

        //    try
        //    {
        //        string json = string.Empty;

        //        using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\core\\data\\states-towns-areas-streets.json"))
        //        {
        //            json = sr.ReadToEnd();
        //        }

        //        var states = JsonConvert.DeserializeObject<List<State>>(json);

        //        // ====================\\

        //        foreach (var state in states.Where(x => x.Id == 25))
        //        {
        //            foreach (var town in state.Towns.Where(x => x.Id == 52))
        //            {
        //                currentTown = $"{town.Id} - {town.Description}";

        //                foreach (var area in town.Areas)
        //                {
        //                    currentArea = $"{area.Id} - {area.Description}";

        //                    string areaXml = await Tools.GetFromBaseAsync(UrlType.Street, area.Id);
        //                    areaXml = areaXml.Replace("        </option>", string.Empty);

        //                    XmlDocument areaDoc = new XmlDocument();
        //                    areaDoc.LoadXml(areaXml.Replace("&", "&amp;"));

        //                    foreach (XmlNode streetNode in areaDoc.FirstChild)
        //                    {
        //                        if (streetNode.Attributes[0].Value != "0")
        //                            area.Streets.Add(
        //                                new Street
        //                                {
        //                                    Id = Convert.ToInt32(streetNode.Attributes[0].Value),
        //                                    Description = streetNode.InnerText,
        //                                    AreaId = area.Id
        //                                });
        //                    }
        //                }
        //            }
        //        }

        //        return Ok(states);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error occured: {ex.ToString()}\nState: {currentState}\nTown: {currentTown}\nArea: {currentArea}");
        //    }
        //}

        //[HttpGet("states")]
        //public IActionResult GetStates()
        //{
        //    try
        //    {
        //        string json = string.Empty;

        //        using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\data\\states-towns-areas-streets.json"))
        //        {
        //            json = sr.ReadToEnd();
        //        }

        //        var states = JsonConvert.DeserializeObject<List<State>>(json);

        //        //foreach (var state in states.Where(x => x.Id == 25))
        //        //{
        //        //    foreach (var town in state.Towns.Where(x => x.Id == 50))
        //        //    {
        //        //        town.StateId = state.Id;
        //        //        foreach (var area in town.Areas.Where(x => x.Id == 778))
        //        //        {
        //        //            area.TownId = town.Id;

                         
        //        //        }
        //        //    }
        //        //}

        //        return Ok(states);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error occured: {ex.ToString()}");
        //    }
        //}
    }
}
