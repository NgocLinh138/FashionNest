using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Users.Responses;
using FashionNest.Domain.Common;
using FashionNest.Domain.Common.Repositories;
using System.Linq.Expressions;

namespace FashionNest.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, GetUsersResult>
    {
        private readonly IUserRepository userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<GetUsersResult>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Expression<Func<User, bool>> filter = user => true;

                if (request.IsActive.HasValue)
                {
                    filter = user => user.IsActive == request.IsActive.Value;
                }

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    var filterExpression = BuildFilterExpression(request.Filter);
                    filter = CombineFilters(filter, filterExpression);
                }

                Expression<Func<User, object>> sortBy = null;
                if (!string.IsNullOrWhiteSpace(request.SortBy))
                {
                    sortBy = request.SortBy switch
                    {
                        "Name" => user => user.Name,
                        "Email" => user => user.Email,
                        _ => user => user.Name
                    };
                }

                var users = await userRepository.GetAsync(
                    filter: filter,
                    orderBy: sortBy,
                    orderByAsc: request.SortOrderAscending,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize);

                var response = users.Select(user => new UserGetAllResponse(
                    user.Id.Value,
                    user.Name,
                    user.Email,
                    user.PhoneNumber,
                    user.Address,
                    user.IsActive)).ToList();

                return Result.Success(new GetUsersResult(new PaginatedResult<UserGetAllResponse>(response, response.Count(), request.PageIndex, request.PageSize)));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private Expression<Func<User, bool>> BuildFilterExpression(string filter)
        {
            return user => user.Name.Contains(filter) || user.Email.Contains(filter) || user.PhoneNumber.Contains(filter);
        }

        private Expression<Func<User, bool>> CombineFilters(Expression<Func<User, bool>> first, Expression<Func<User, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(User));

            var combinedBody = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter)
            );

            return Expression.Lambda<Func<User, bool>>(combinedBody, parameter);
        }
    }
}
