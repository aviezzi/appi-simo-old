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
        protected readonly string _resourceUri;

        public EndPoint(DataServiceContext context, HttpClient client, string resourceUri)
        {
            _context = context;
            _client = client;

            _resourceUri = resourceUri;
        }

        public DataServiceQuery<TEntity> Entities => _context.CreateQuery<TEntity>(_resourceUri);

        public async Task<TEntity> Entity(Guid id, Func<DataServiceQuery<TEntity>, IQueryable<TEntity>> selector) =>
            (await selector(Entities).Where(u => u.Id == id).ToListAsync(_client)).Value.FirstOrDefault();

        public async Task Save(TEntity entity)
        {
            if (entity.Id == Guid.Empty)
            {
                await _client.SendJsonAsync<TEntity>(HttpMethod.Post, $"{_resourceUri}/Post", entity);
            }
            else
            {
                await _client.SendJsonAsync<TEntity>(HttpMethod.Put, $"{_resourceUri}/Put", entity);
            }
        }

        public async Task Delete(Guid id) =>
            await _client.DeleteAsync($"{_resourceUri}/Delete/{id}");
    }
}