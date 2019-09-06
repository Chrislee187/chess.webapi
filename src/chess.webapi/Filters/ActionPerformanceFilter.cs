using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace chess.webapi.Filters
{
    public class ActionPerformanceFilter : IActionFilter
    {
        private Stopwatch _timer;
        private readonly ILogger<ActionPerformanceFilter> _logger;


        public ActionPerformanceFilter(
            ILogger<ActionPerformanceFilter> logger
            )
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _timer = new Stopwatch();
            _timer.Start();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _timer.Stop();
            if (context.Exception == null)
            {
                _logger.LogRoutePerformance(context.HttpContext.Request.Path,
                    context.HttpContext.Request.Method,
                    _timer.ElapsedMilliseconds);
            }
        }
    }
}
