// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TasksController.cs" company="Somnath Mukherjee">
// Copyright (c) Somnath Mukherjee. All rights reserved.
// </copyright>
// <summary>
// Web Api for Task Management
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fsd.Capsule.TaskManagerApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using Exception;
    using Fsd.Capsule.TaskManagerApi.Helpers;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Task Manager Api 
    /// </summary>
    [Produces("application/json")]
    [Route("/[controller]")]
    public class TasksController : Controller
    {
        /// <summary>
        /// The model state error
        /// </summary>
        private const string ModelStateError = "Unable to create model. Please see error details for more information";

        /// <summary>
        /// The GET method for retrieving all tasks
        /// </summary>
        /// <returns>
        /// The json serialized list of task.
        /// </returns>
        /// <remarks>GET: /Tasks</remarks>
        [HttpGet]
        public IActionResult Get()
        {
            IActionResult result;

            if (!ModelState.IsValid)
            {
                throw new ApiException(ModelStateError, 500, ModelState);
            }

            try
            {
                var headerInformation = new HeaderInformation(Request.Headers);

                var tasksList = new List<string>() { "Task 1", "Task 2" };

                result = Ok(tasksList);
            }
            catch (ArgumentException argumentException)
            {
                result = BadRequest(argumentException.Message);
            }

            return result;
        }

        /// <summary>
        /// The GET method for retrieving a tasks
        /// </summary>
        /// <param name="id">Id of the Task</param>
        /// <returns>
        /// The json serialized task.
        /// </returns>
        /// <remarks>GET: /Task/5</remarks>
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            IActionResult result;

            result = Ok("value");

            return result;
        }

        /// <summary>
        /// The POST method for creating a tasks
        /// </summary>
        /// <param name="value">Task payload</param>
        /// <remarks>POST: /Task</remarks>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// The POST method for updating a tasks
        /// </summary>
        /// <param name="id">Id of the Task</param>
        /// <param name="value">Task payload</param>
        /// <remarks>PUT: /Task/5</remarks>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        /// The POST method for deleting a tasks
        /// </summary>
        /// <param name="id">Id of the Task</param>
        /// <remarks>DELETE: /Task/5</remarks>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}