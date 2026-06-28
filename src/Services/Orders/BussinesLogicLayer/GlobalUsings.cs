global using System.Net.Http.Json;



global using BussinesLogicLayer.Policies;
global using BussinesLogicLayer.Services;
global using BussinesLogicLayer.Contracts;
global using BussinesLogicLayer.IServices;
global using BusinessLogicLayer.Validators;
global using BussinesLogicLayer.Contracts.Users;
global using BussinesLogicLayer.Contracts.Products;
global using BussinesLogicLayer.HttpClients.Users;
global using BussinesLogicLayer.HttpClients.Products;



global using DataAccessLayer.Entities;
global using DataAccessLayer.IRepository;



global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.DependencyInjection;



global using Polly;
global using Mapster;
global using Polly.Retry;
global using MongoDB.Driver;
global using FluentValidation;
global using FluentValidation.Results;