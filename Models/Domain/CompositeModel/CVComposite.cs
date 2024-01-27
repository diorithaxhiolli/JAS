using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JAS.Models.Domain;

namespace JAS.Models.Domain.CompositeModel
{
    public class CVComposite
    {
        public CV? CV {  get; set; }

        public Education? Education { get; set; }

        public Experience? Experience { get; set; }
    }
}
