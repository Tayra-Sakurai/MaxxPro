// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Cougar.Tools
{
    /// <summary>
    /// Basic interface for model I/O.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    public interface IModelTool<TModel>
        where TModel : class
    {
        /// <summary>
        /// Retreives all data that matches the model type.
        /// </summary>
        /// <returns>The task to return the data.</returns>
        [Description("Retreives all data matching the data type from the database.")]
        Task<ICollection<TModel>> GetAll();

        /// <summary>
        /// Add new model to the database.
        /// </summary>
        /// <param name="model">The model data.</param>
        /// <returns>The asynchronous task.</returns>
        [Description("Adds the model object data to the database.")]
        Task AddModelAsync([Description("The model data to be added to the database.")] TModel model);

        /// <summary>
        /// Removes the model object from the database.
        /// </summary>
        /// <param name="model">The target model.</param>
        /// <returns>The asynchrnous task to control the operation.</returns>
        [Description("Removes the model object from the database.")]
        Task RemoveModelAsync([Description("The model data to be removed from the database.")] TModel model);

        /// <summary>
        /// Updates the model data of the database.
        /// </summary>
        /// <param name="model">The target model data.</param>
        /// <returns>The asynchronous task.</returns>
        [Description("Updates the data of the database.")]
        Task UpdateModelAsync([Description("The model data to be updated.")] TModel model);

        /// <summary>
        /// Gets the tools for AI.
        /// </summary>
        /// <returns>The tools of this interface.</returns>
        IList<AITool> GetAITools();
    }
}
