// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Task.cs" company="Somnath Mukherjee">
//   Copyright (c) Somnath Mukherjee. All rights reserved.
// </copyright>
// <summary>
//  Task Model Class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fsd.Capsule.TaskManagerApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The model class for a Task
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Required]
        [Display(Name = "Task Id")]
        public int TaskID { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        [ForeignKey(nameof(Parent))]
        [Display(Name = "Parent Id")]
        [DisplayFormat(NullDisplayText = "-")]
        public int? ParentID { get; set; }

        /// <summary>
        /// Gets or sets the task summary.
        /// </summary>
        [Required]
        [StringLength(255, ErrorMessage = "Summary cannot be longer than 255 characters.")]
        [DataType(DataType.Text)]
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the task description.
        /// </summary>
        [StringLength(5000, ErrorMessage = "Description cannot be longer than 5000 characters.")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", NullDisplayText = "-", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", NullDisplayText = "-", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the task priotity
        /// </summary>
        public Priority? Priority { get; set; }

        /// <summary>
        /// Gets or sets the task status.
        /// </summary>
        public Status? Status { get; set; }

        /// <summary>
        /// Gets or sets the parent task.
        /// </summary>
        public Task Parent { get; set; }

        /// <summary>
        /// Gets or sets the child tasks.
        /// </summary>
        public ICollection<Task> ChildTasks { get; set; }
    }
}
