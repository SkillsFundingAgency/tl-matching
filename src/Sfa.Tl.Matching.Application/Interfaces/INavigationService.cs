﻿using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface INavigationService
    {
        Task AddCurrentUrl(string path, string username);
        Task<string> GetBackLink(string username);
    }
}