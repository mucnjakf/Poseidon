﻿global using AutoMapper;
global using FluentValidation;
global using FluentValidation.Results;
global using Hangfire;
global using Hangfire.SqlServer;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.ResponseCompression;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.ML;
global using Microsoft.ML.Data;
global using Microsoft.ML.Calibrators;
global using Microsoft.ML.Trainers.FastTree;
global using System.Diagnostics.CodeAnalysis;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using Poseidon.DAL.Core;
global using Poseidon.DAL.Entities;
global using Poseidon.DAL.Repositories;
global using Poseidon.DAL.Repositories.Interfaces;
global using Poseidon.Server;
global using Poseidon.Server.Hubs;
global using Poseidon.Server.Services;
global using Poseidon.Server.Services.Interfaces;
global using Poseidon.Server.Validation.Account;
global using Poseidon.Server.Validation.Event;
global using Poseidon.Server.Validation.Services;
global using Poseidon.Server.Validation.Services.Interfaces;
global using Poseidon.Server.Validation.Vessel;
global using Poseidon.Shared.DTO.Account;
global using Poseidon.Shared.DTO.Event;
global using Poseidon.Shared.DTO.Vessel;
global using Poseidon.Shared.Models;
global using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;