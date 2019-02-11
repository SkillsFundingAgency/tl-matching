using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport.Security
{
    public class When_Data_Import_Controller_Index_Action_Allow_Anonymous_Attribute
    {
        private AllowAnonymousAttribute _allowAnonymousAttributeGet;
        private AllowAnonymousAttribute _allowAnonymousAttributePost;

        
        public void Setup()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

            var methodInfos = typeof(DataImportController)
                .GetMember(nameof(DataImportController.Index), MemberTypes.Method, flags)
                .Cast<MethodInfo>().ToList();

            _allowAnonymousAttributeGet = methodInfos[0].GetCustomAttribute(typeof(AllowAnonymousAttribute))
                as AllowAnonymousAttribute;

            _allowAnonymousAttributePost = methodInfos[1].GetCustomAttribute(typeof(AllowAnonymousAttribute))
                as AllowAnonymousAttribute;
        }

        [Fact]
        public void Then_Is_Not_On_Get_Method() =>
            Assert.Null(_allowAnonymousAttributeGet);

        [Fact]
        public void Then_Is_Not_On_Post_Method() =>
            Assert.Null(_allowAnonymousAttributePost);
    }
}