// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleTests.cs" company="Somnath Mukherjee">
//    Copyright (c) Somnath Mukherjee. All rights reserved.
// </copyright>
// <summary>
//   The sample test
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fsd.Capsule.TaskManagerApi.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fsd.Capsule.TaskManagerApi.Controllers;
    using Fsd.Capsule.TaskManagerApi.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using Xunit;

    /// <summary>
    /// The validator helper tests.
    /// </summary>
    public class TasksControllerTests
    {
        /// <summary>
        /// Test method used to validate that GetAllTasks returns all the tasks
        /// </summary>
        [Fact]
        public void When_GetAllTasksIsCalled_AllTasksAreReturned()
        {
            // Arrange 
            var tasks = CreateTaskList();

            var mockDbSet = CreateMockTaskDbSet(tasks);
            var mockContext = CreateMockTaskContext(mockDbSet.Object);   

            TasksController tasksController = new TasksController(mockContext.Object);
            tasksController.ControllerContext = new ControllerContext();
            tasksController.ControllerContext.HttpContext = new DefaultHttpContext();
            tasksController.ControllerContext.HttpContext.Request.Headers["test-header"] = "test-header-value";

            // Act
            var results = tasksController.Get();

            // Assert
            Assert.Equal(tasks.Count, results.Value.Count);
        }

        /// <summary>
        /// Test method used to validate that GetTaskById retirns the correct task
        /// </summary>
        [Fact]
        public void When_GetTaskByIdIsCalled_TheMatchingTaskIsReturned()
        {
            // Arrange 
            var tasks = CreateTaskList();

            var mockDbSet = CreateMockTaskDbSet(tasks);
            var mockContext = CreateMockTaskContext(mockDbSet.Object);

            TasksController tasksController = new TasksController(mockContext.Object);
            tasksController.ControllerContext = new ControllerContext();
            tasksController.ControllerContext.HttpContext = new DefaultHttpContext();
            tasksController.ControllerContext.HttpContext.Request.Headers["test-header"] = "test-header-value";

            // Act
            var result = tasksController.Get(2);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.TaskID);
            Assert.Equal("Implement Front End Service", result.Value.Summary);
        }

        /// <summary>
        /// Test method used to validate that CreateTask adds and saves a task
        /// </summary>
        [Fact]
        public void When_CreateTaskIsCalled_TaskIsAddedAndSaved()
        {
            // Arrange 
            var tasks = CreateTaskList();

            var mockDbSet = CreateMockTaskDbSet(tasks);
            var mockContext = CreateMockTaskContext(mockDbSet.Object);

            TasksController tasksController = new TasksController(mockContext.Object);
            tasksController.ControllerContext = new ControllerContext();
            tasksController.ControllerContext.HttpContext = new DefaultHttpContext();
            tasksController.ControllerContext.HttpContext.Request.Headers["test-header"] = "test-header-value";

            Task newTask = new Task
                {
                    TaskID = 4,
                    Summary = "Implement Plugin",
                    Description =
                        "Implement Plugin for the feature which will invoke the Front End Service",
                    StartDate = DateTime.Parse("2018-07-16"),
                    EndDate = DateTime.Parse("2018-07-20"),
                    Priority = Priority.High,
                    Status = Status.NotStarted
                };

            // Act
            var result = tasksController.Post(newTask);

            // Assert
            mockDbSet.Verify(m => m.Add(It.IsAny<Task>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        /// <summary>
        /// Test method used to validate that UpdateTask updates and saves a task
        /// </summary>
        [Fact]
        public void When_UpdateTaskIsCalled_TaskIsUpdatedAndSaved()
        {
            // Arrange 
            var tasks = CreateTaskList();

            var mockDbSet = CreateMockTaskDbSet(tasks);
            var mockContext = CreateMockTaskContext(mockDbSet.Object);

            TasksController tasksController = new TasksController(mockContext.Object);
            tasksController.ControllerContext = new ControllerContext();
            tasksController.ControllerContext.HttpContext = new DefaultHttpContext();
            tasksController.ControllerContext.HttpContext.Request.Headers["test-header"] = "test-header-value";

            Task updatedTask = new Task
                {
                    TaskID = 2,
                    Summary = "Implement Front End Service - Updated",
                    Description =
                        "Implement Front End Service for the feature which will invoke the Channel Service",
                    StartDate = DateTime.Parse("2018-07-09"),
                    EndDate = DateTime.Parse("2018-07-13"),
                    Priority = Priority.High,
                    Status = Status.NotStarted
                };

            // Act
            var result = tasksController.Put(2, updatedTask);

            // Assert
            mockDbSet.Verify(m => m.Find(It.IsAny<int>()), Times.Once());
            mockDbSet.Verify(m => m.Update(It.IsAny<Task>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        /// <summary>
        /// Test method used to validate that DeleteTask deletes and saves a task
        /// </summary>
        [Fact]
        public void When_DeleteTaskIsCalled_TaskIsDeletedAndSaved()
        {
            // Arrange 
            var tasks = CreateTaskList();

            var mockDbSet = CreateMockTaskDbSet(tasks);
            var mockContext = CreateMockTaskContext(mockDbSet.Object);

            TasksController tasksController = new TasksController(mockContext.Object);
            tasksController.ControllerContext = new ControllerContext();
            tasksController.ControllerContext.HttpContext = new DefaultHttpContext();
            tasksController.ControllerContext.HttpContext.Request.Headers["test-header"] = "test-header-value";            

            // Act
            var result = tasksController.Delete(2);

            // Assert
            mockDbSet.Verify(m => m.Find(It.IsAny<int>()), Times.Once());
            mockDbSet.Verify(m => m.Remove(It.IsAny<Task>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        /// <summary>
        /// Creates the mock task DbSet.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        /// <returns>The Mocked DbSet</returns>
        private Mock<DbSet<Task>> CreateMockTaskDbSet(List<Task> tasks)
        {
            var data = tasks.AsQueryable();
            var mockDbSet = new Mock<DbSet<Task>>();
            mockDbSet.As<IQueryable<Task>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<Task>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Task>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Task>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockDbSet.Setup(ds => ds.Find(It.IsAny<object[]>())).Returns((object[] key) => tasks.FirstOrDefault(task => task.TaskID == (int)key.First()));
            mockDbSet.Setup(x => x.Add(It.IsAny<Task>())).Callback<Task>(task => tasks.Add(task));
            mockDbSet.Setup(x => x.Update(It.IsAny<Task>())).Callback<Task>(
                task =>
                    {
                        int index = tasks.FindIndex(t => t.TaskID == task.TaskID);
                        tasks[index] = task;
                    });
            mockDbSet.Setup(x => x.Remove(It.IsAny<Task>())).Callback<Task>(task => tasks.Remove(task));

            return mockDbSet;
        }

        /// <summary>
        /// Creates the mock task context.
        /// </summary>
        /// <param name="dbSet">The DbSet</param>
        /// <returns>The Mocked Task Context</returns>
        private Mock<TaskContext> CreateMockTaskContext(DbSet<Task> dbSet)
        {
            var mockContext = new Mock<TaskContext>();

            mockContext.Setup(c => c.Tasks).Returns(dbSet);

            return mockContext;
        }

        /// <summary>
        /// Creates a sample task list.
        /// </summary>
        /// <returns>The List of tasks</returns>
        private List<Task> CreateTaskList()
        {
            return new List<Task>
                {
                    new Task
                        {
                            TaskID = 1,
                            Summary = "Implement Channel Service",
                            Description =
                                "Implement Channel Service for the feature which will invoke the factories",
                            StartDate = DateTime.Parse("2018-07-02"),
                            EndDate = DateTime.Parse("2018-07-06"),
                            Priority = Priority.High,
                            Status = Status.NotStarted
                        },
                    new Task
                        {
                            TaskID = 2,
                            Summary = "Implement Front End Service",
                            Description =
                                "Implement Front End Service for the feature which will invoke the Channel Service",
                            StartDate = DateTime.Parse("2018-07-09"),
                            EndDate = DateTime.Parse("2018-07-13"),
                            Priority = Priority.High,
                            Status = Status.NotStarted
                        },
                    new Task
                        {
                            TaskID = 3,
                            Summary = "Implement Front End",
                            Description =
                                "Implement Front End for the feature which will invoke the Front End Service",
                            StartDate = DateTime.Parse("2018-07-16"),
                            EndDate = DateTime.Parse("2018-07-20"),
                            Priority = Priority.High,
                            Status = Status.NotStarted
                        }
                };
        }
    }
}
