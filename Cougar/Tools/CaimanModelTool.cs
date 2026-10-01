// SPDX-FileCopyrightText: 2026 Tayra Sakurai
// SPDX-License-Identifier: GPL-3.0-or-later

using Caiman.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cougar.Tools
{
    /// <summary>
    /// <see cref="IModelTool{TModel}"/> implementation for <see cref="CaimanContext"/>.
    /// </summary>
    /// <typeparam name="TModel">The model type. Must be included by <see cref="CaimanContext"/>.</typeparam>
    public class CaimanModelTool<TModel> : IModelTool<TModel>
        where TModel : class
    {
        private readonly IDbContextFactory<CaimanContext> factory;

        public CaimanModelTool(IDbContextFactory<CaimanContext> factory)
        {
            this.factory = factory;
        }

        public async Task<ICollection<TModel>> GetAll()
        {
            using CaimanContext context = await factory.CreateDbContextAsync();

            return await context.Set<TModel>().ToListAsync();
        }

        public async Task AddModelAsync(TModel model)
        {
            using CaimanContext context = await factory.CreateDbContextAsync();

            context.Add(model);
            await context.SaveChangesAsync();
        }

        public async Task RemoveModelAsync(TModel model)
        {
            using CaimanContext context = await factory.CreateDbContextAsync();

            context.Remove(model);
            await context.SaveChangesAsync();
        }

        public async Task UpdateModelAsync(TModel model)
        {
            using CaimanContext context = await factory.CreateDbContextAsync();

            context.Update(model);
            await context.SaveChangesAsync();
        }

        public IList<AITool> GetAITools()
        {
            IList<AITool> aiTools = new List<AITool>();

            AIFunction getFunc = AIFunctionFactory.Create(GetAll);
            aiTools.Add(getFunc);

            AIFunction addFuncWithoutApproval = AIFunctionFactory.Create(AddModelAsync);
            AIFunction addFunc = new ApprovalRequiredAIFunction(addFuncWithoutApproval);
            aiTools.Add(addFunc);

            AIFunction removeFuncWithoutApproval = AIFunctionFactory.Create(RemoveModelAsync);
            AIFunction removeFunc = new ApprovalRequiredAIFunction(removeFuncWithoutApproval);
            aiTools.Add(removeFunc);

            AIFunction updateFuncWithoutApproval = AIFunctionFactory.Create(UpdateModelAsync);
            AIFunction updateFunc = new ApprovalRequiredAIFunction(updateFuncWithoutApproval);
            aiTools.Add(updateFunc);

            return aiTools;
        }
    }
}
