using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirGradientDataServer.Models;

namespace AirGradientDataServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly MeasurementContext _context;

        public MeasurementsController(MeasurementContext context)
        {
            _context = context;
        }

        // GET: api/Measurements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurements(string? order)
        {
            var measurements = _context.Measurements;
            order = order?.ToLower();

            IOrderedQueryable<Measurement> orderedMeasurements;
            if (string.IsNullOrEmpty(order) || order == "desc")
            {
                // Default is descending
                orderedMeasurements = measurements.OrderByDescending(e => e.MeasurementTime);
            }
            else if (order == "asc")
            {
                orderedMeasurements = measurements.OrderBy(e => e.MeasurementTime);
            }
            else
            {
                return BadRequest();
            }

            return await orderedMeasurements.ToListAsync();
        }

        // GET: api/Measurements/prometheus
        [HttpGet("prometheus")]
        [Produces("text/plain")]
        public async Task<ActionResult<string>> GetPrometheusData()
        {
            // For Prometheus we just want the latest measure for each device
            var measurementsForPrometheus = await _context.Measurements
                .GroupBy(measurement => measurement.DeviceId)
                .Select(measurementGroup => measurementGroup.OrderByDescending(measurement => measurement.MeasurementTime).FirstOrDefault())
                .ToListAsync();

            if (measurementsForPrometheus == null || measurementsForPrometheus.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("# HELP airgradient_pm02 Particulate Matter PM2.5 value");
            sb.AppendLine("# TYPE airgradient_pm02 gauge");
            foreach (var measurement in measurementsForPrometheus)
            {
                if (measurement == null) continue;
                sb.AppendFormat("airgradient_pm02{{id=\"{0}\"}}{1}", measurement.DeviceId, measurement.PM25).AppendLine();
            }

            sb.AppendLine("# HELP airgradient_rco2 CO2 value, in ppm");
            sb.AppendLine("# TYPE airgradient_rco2 gauge");
            foreach (var measurement in measurementsForPrometheus)
            {
                if (measurement == null) continue;
                sb.AppendFormat("airgradient_rco2{{id=\"{0}\"}}{1}", measurement.DeviceId, measurement.CO2).AppendLine();
            }

            sb.AppendLine("# HELP airgradient_atmp Temperature, in degrees Celsius");
            sb.AppendLine("# TYPE airgradient_atmp gauge");
            foreach (var measurement in measurementsForPrometheus)
            {
                if (measurement == null) continue;
                sb.AppendFormat("airgradient_atmp{{id=\"{0}\"}}{1}", measurement.DeviceId, measurement.Temperature).AppendLine();
            }

            sb.AppendLine("# HELP airgradient_rhum Relative humidity, in percent");
            sb.AppendLine("# TYPE airgradient_rhum gauge");
            foreach (var measurement in measurementsForPrometheus)
            {
                if (measurement == null) continue;
                sb.AppendFormat("airgradient_rhum{{id=\"{0}\"}}{1}", measurement.DeviceId, measurement.Humidity).AppendLine();
            }

            return sb.ToString();
        }

        // GET: api/Measurements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Measurement>> GetMeasurement(long id)
        {
            var measurement = await _context.Measurements.FindAsync(id);

            if (measurement == null)
            {
                return NotFound();
            }

            return measurement;
        }

        // PUT: api/Measurements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeasurement(long id, Measurement measurement)
        {
            if (id != measurement.Id)
            {
                return BadRequest();
            }

            _context.Entry(measurement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeasurementExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Measurements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Measurement>> PostMeasurement(Measurement measurement)
        {
            _context.Measurements.Add(measurement);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMeasurement), new { id = measurement.Id }, measurement);
        }

        // DELETE: api/Measurements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeasurement(long id)
        {
            var measurement = await _context.Measurements.FindAsync(id);
            if (measurement == null)
            {
                return NotFound();
            }

            _context.Measurements.Remove(measurement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MeasurementExists(long id)
        {
            return _context.Measurements.Any(e => e.Id == id);
        }
    }
}
