using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using RMS.Queries.Container;

namespace RMS.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}