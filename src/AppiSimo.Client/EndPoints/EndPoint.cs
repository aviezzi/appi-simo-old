﻿namespace AppiSimo.Client.EndPoints
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AppiSimo.Shared.Abstract;
    using Microsoft.AspNetCore.Blazor;
    using Microsoft.OData.Client;

    public class EndPoint<TEntity> : IEndPoint<TEntity>
        where TEntity : class, IEntity, new()
    {
        public readonly HttpClient _client;

        readonly DataServiceContext _context;
        readonly string _resourceUri;

        public EndPoint(DataServiceContext context, HttpClient client, string resourceUri)
        {
            _context = context;
            _client = client;

            _resourceUri = resourceUri;
        }

        public DataServiceQuery<TEntity> Entities => _context.CreateQuery<TEntity>(_resourceUri);

        public async Task<TEntity> Entity(Guid id) =>
        (
            await Entities
                .Where(u => u.Id == id)
                .ToListAsync(_client)
        ).Value.FirstOrDefault();

        // TODO: move odata uri from string builder.

        public async Task Save(TEntity entity)
        {
            if (entity.Id == Guid.Empty)
            {
                await _client.SendJsonAsync<TEntity>(HttpMethod.Post, $"/odata/{_resourceUri}/Post", entity);
            }
            else
            {
                await _client.SendJsonAsync<TEntity>(HttpMethod.Put, $"/odata/{_resourceUri}/Put", entity);
            }
        }

        public async Task Delete(Guid id) => await _client.DeleteAsync($"/odata/{_resourceUri}/Delete/{id}");
    }
}